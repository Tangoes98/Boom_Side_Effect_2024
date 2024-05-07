using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    
    [SerializeField] private float _fxLastTime = 3;
    
    private void Start() {
        StartCoroutine(CleanUp());
    }
    IEnumerator CleanUp() {
        yield return new WaitForSeconds(_fxLastTime);
        Destroy(this.gameObject);
    }
}
