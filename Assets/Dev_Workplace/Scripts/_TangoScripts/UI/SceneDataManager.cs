using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDataManager : MonoBehaviour
{
    public static SceneDataManager Instance;
    [SerializeField] List<GameObject> _savingGameobjects = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        foreach (var gameobject in _savingGameobjects)
        {
            DontDestroyOnLoad(gameobject);
        }
        
        DontDestroyOnLoad(this);
    }

    public string CurrentLanguage;






}
