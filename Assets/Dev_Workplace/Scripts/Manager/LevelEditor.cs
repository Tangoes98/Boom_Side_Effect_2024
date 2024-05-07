using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour
{
    private static LevelEditor _instance;
    public static LevelEditor Instance {
        get {
            if(_instance==null) {
                throw new Exception("LevelEditor not in scene");
            }  
            return _instance; 
        } 
    }



    [SerializeField] private float spawnInterval =1f;
    [SerializeField] private GameObject _startLevelButton;

    [SerializeField] private Transform[] _spawnLocations;

    [SerializeField] private Level[] levels;
    [SerializeField] private EnemyBase[] enemies;
    [SerializeField] private Transform enemyParentObject;

    [SerializeField] private GameObject[] _gearPrefabs;


    [Header("以下请勿修改")]
    
    public int LevelNumber; // start from 1
    public int WaveNumber; // start from 1

    public LevelState currentState;

    public List<Minion> EnemyOnStage = new(); //全打完下一波

    private Dictionary<string,EnemyBase> enemyDict;
    private Queue<Wave> _waveQueue;
    private Wave _currentWave;
    private Queue<SpawnEnemyBase[]> _spawnQueue;
    private float _currentStateStartTime; 

    private void Awake() {
        _instance = this;
        enemyDict = enemies.ToDictionary(e => e.code, e => e);
        EnemyOnStage = new();
        LevelNumber = 0;
        ChangeState(LevelState.BUILD);
    }

    public void NextLevel() {
        LevelNumber ++;
        var curLvl = levels[LevelNumber-1];
        if(curLvl.boomEntrances!=null && curLvl.boomEntrances.Length>0) {
            foreach(int e in curLvl.boomEntrances) {
                EnvironmentFX.Instance.PlayBoomVFX(1, e - 1);
            }
        }
        _waveQueue = new(curLvl.waves);
        WaveNumber = 0;
        NextWave();
    }

    public bool IsCurrentWaveDone()  => (_spawnQueue==null || _spawnQueue.Count==0) && EnemyOnStage.Count == 0;

    private void NextWave() {
        WaveNumber++;
        _currentWave = _waveQueue.Dequeue();
        ChangeState(LevelState.FIGHT_WAVE);
        _spawnQueue = new Queue<SpawnEnemyBase[]> (_currentWave.timeline.Select(se => StrToSpawnEnemyBase(se.spawns)));
        foreach(var se in _currentWave.timeline) {
            Invoke(nameof(Spawn), se.timePoint);
        }
    }

    private SpawnEnemyBase[] StrToSpawnEnemyBase(string spawns) {
        List<SpawnEnemyBase> bases = new();
        foreach(string spawn in spawns.Split(";")) {
            var arr = spawn.Split(",");
            bases.Add(new(){code=arr[0], number= int.Parse(arr[1]), spawnLocation = _spawnLocations[int.Parse(arr[2])]});
        }
        return bases.ToArray();
    }

    private void Spawn() {
        SpawnEnemyBase[] enemies = _spawnQueue.Dequeue();
        foreach(var seb in enemies) {
            EnemyBase eb = enemyDict[seb.code];
            StartCoroutine(SpawnSameEnemy(seb, eb));
        }
    }

    private IEnumerator SpawnSameEnemy(SpawnEnemyBase seb, EnemyBase eb) {
        for(int i = 0; i<seb.number; i++) {
            GameObject enemy = enemyParentObject==null? 
                Instantiate(eb.enemyPrefab,seb.spawnLocation.position, Quaternion.identity) :
                Instantiate(eb.enemyPrefab,seb.spawnLocation.position, Quaternion.identity,enemyParentObject);
            var script = enemy.GetComponent<Minion>();
            script.code = eb.code;
            EnemyOnStage.Add(script);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void EnemyDie(Minion enemy) {
        EnemyOnStage.Remove(enemy);
        EnemyBase eb = enemyDict[enemy.code];
        Vector3 pos = enemy.transform.position;
        Destroy(enemy.gameObject);

        StartCoroutine(DropGear(eb.resource, pos));
        
        if(IsCurrentWaveDone()) {
            if(_waveQueue.Count == 0) {
                if(levels.Length < LevelNumber + 1) {
                    Debug.Log("WIN");
                    SceneDataManager.Instance.IsWinning = true;
                    UIManager.Instance.GameOver();
                    return;
                }
                ChangeState(LevelState.BUILD);
            } else {
                ChangeState(LevelState.FIGHT_INTERVAL);
                Invoke(nameof(NextWave), _currentWave.interval);
            }
        }
    }

    IEnumerator DropGear(int resource, Vector3 position) {
        GameObject gear = Instantiate(_gearPrefabs[resource<=3? 0 : 1], position, Quaternion.identity);
        yield return new WaitForSeconds(3);
        var tf = gear.transform;
        var currentPos = tf.position;
        var target = new Vector3(293,53,-56);
        var t = 0f;
        while(t <= 1f)
        {
            t += Time.deltaTime / 2;
            tf.position = Vector3.Lerp(currentPos, target, t);
            yield return null;
        }
        Destroy(tf.gameObject);
        ResourceManager.Instance.DropResource(resource);

    }

    private void ChangeState(LevelState state) {
        _currentStateStartTime = Time.time;
        currentState = state;
        _startLevelButton.SetActive(state == LevelState.BUILD);
    }

    public float CurrentStateProgress() {
        float progress = 0;
        switch(currentState) {
            case LevelState.BUILD:
                progress = 1;
                break;
            case LevelState.FIGHT_WAVE:
                progress = (Time.time - _currentStateStartTime)/_currentWave.totalFightTime;
                break;
            case LevelState.FIGHT_INTERVAL:
                progress = (Time.time - _currentStateStartTime)/_currentWave.interval;
                break;
        }
        if(progress>1) return 1;
        if(progress<0) return 0;
        return progress;
    }
}

public enum LevelState {
    BUILD, FIGHT_WAVE, FIGHT_INTERVAL 
}
