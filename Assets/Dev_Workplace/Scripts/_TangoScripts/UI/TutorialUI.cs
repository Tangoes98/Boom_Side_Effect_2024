using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    public static TutorialUI Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public int TutorialStep;
    [SerializeField] List<GameObject> _tutorialScenes;

    private void Start()
    {
        TutorialStep = 0;
    }


    private void Update()
    {
        UpdateTutorialScene();
    }

    void UpdateTutorialScene()
    {

    }
}
