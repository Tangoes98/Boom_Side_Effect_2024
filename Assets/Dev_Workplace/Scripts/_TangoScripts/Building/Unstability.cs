using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unstability : MonoBehaviour
{
     int _currentUnstabilityLevel;
    [SerializeField] List<Transform> _unstability = new();
    Dictionary<int, Transform> _unstabilityPairs = new();

    private void Start()
    {
        for (int i = 1; i < 5; i++)
        {
            _unstabilityPairs.Add(i, _unstability[i - 1]);
        }
    }

    private void Update()
    {
        UpdateRankTransform();
    }

    void UpdateRankTransform()
    {
        foreach (var item in _unstabilityPairs)
        {
            if (item.Key == _currentUnstabilityLevel) item.Value.gameObject.SetActive(true);
            else item.Value.gameObject.SetActive(false);
        }
    }
}
