using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    
    public Wave[] waves;
    public EnemyBase[] enemies;
    
    public int WaveNumber;
    
    public List<Minion> EnemyOnStage = new(); //全打完下一波

    private Dictionary<string,EnemyBase> enemyDict;
    private Queue<Wave> _waveQueue;
    private Wave _currentWave;
    private Queue<SpawnEnemyBase[]> _spawnQueue;

    private void Awake() {
        _instance = this;
        enemyDict = enemies.ToDictionary(e => e.code, e => e);
        EnemyOnStage = new();
        _waveQueue = new(waves);
        WaveNumber = 0;
    }
    private void Start() {
        
        NextWave();
    }

    public bool IsCurrentWaveDone()  => (_spawnQueue==null || _spawnQueue.Count==0) && EnemyOnStage.Count == 0;

    private void NextWave() {
        WaveNumber++;
        _currentWave = _waveQueue.Dequeue();
        _spawnQueue = new Queue<SpawnEnemyBase[]> (_currentWave.timeline.Select(se => se.spawns));
        foreach(var se in _currentWave.timeline) {
            Invoke(nameof(Spawn), se.timePoint);
        }
    }

    private void Spawn() {
        SpawnEnemyBase[] enemies = _spawnQueue.Dequeue();
        foreach(var seb in enemies) {
            EnemyBase eb = enemyDict[seb.code];
            for(int i = 0; i<seb.number; i++) {
                GameObject enemy = Instantiate(eb.enemyPrefab, seb.spawnLocation, Quaternion.identity);
                var script = enemy.GetComponent<Minion>();
                script.code = eb.code;
                EnemyOnStage.Add(script);
            }
        }
    }

    public void EnemyDie(Minion enemy) {
        EnemyOnStage.Remove(enemy);
        EnemyBase eb = enemyDict[enemy.code];
        ResourceManager.Instance.DropResource(eb.resource);
        Destroy(enemy.gameObject);

        if(IsCurrentWaveDone()) {
            if(_waveQueue.Count == 0) {
                Debug.Log("WIN");
            } else {
                Invoke(nameof(NextWave), _currentWave.interval);
            }
        }
    }
}
