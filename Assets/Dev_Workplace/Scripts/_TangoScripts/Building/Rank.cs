using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rank : MonoBehaviour
{
    int _currentLevel;
    [SerializeField] List<Transform> _ranks = new();
    Dictionary<int, Transform> _rankPairs = new();

    private void Start()
    {
        for (int i = 1; i < 4; i++)
        {
            _rankPairs.Add(i, _ranks[i - 1]);
        }
    }

    private void Update()
    {
        _currentLevel = GetComponentInParent<Architect>().level;
        UpdateRankTransform();
    }

    void UpdateRankTransform()
    {
        foreach (var item in _rankPairs)
        {
            if (item.Key == _currentLevel) item.Value.gameObject.SetActive(true);
            else item.Value.gameObject.SetActive(false);
        }
    }
}
