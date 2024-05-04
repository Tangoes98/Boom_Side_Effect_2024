using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [field: SerializeField] public Animator animator;
    [SerializeField] float _crossfadeDuration;

    [Serializable]
    public struct AnimationClipPairs
    {
        public string StateName;
        public string ClipName;
    }

    [SerializeField] List<AnimationClipPairs> _animationClipPairs = new();
    Dictionary<string, string> _animationClipDic = new();

    private Tuple<string,float> _lastState;
    private Coroutine _animWaiting;





    // private void Start()
    // {
    //     foreach (var item in _animationClipPairs)
    //     {
    //         _animationClipDic.Add(item.StateName, item.ClipName);
    //     }
    // }

    public void InitializeAnimationClipPairs()
    {
        foreach (var item in _animationClipPairs)
        {
            _animationClipDic.Add(item.StateName, item.ClipName);
        }
    }

    public void SwitchAnimState(string stateName)
    {
        if(_lastState!=null && _lastState.Item1 == stateName) {
                return;
        }
        if(_lastState ==null || Time.time - _lastState.Item2 >1.5f) {
            // Debug.Log(_animationClipDic[stateName]);
            animator.CrossFadeInFixedTime(_animationClipDic[stateName], _crossfadeDuration, 0);
            _lastState = new(stateName,Time.time);
            return;
        } 

        if(_animWaiting != null) {
            StopCoroutine(_animWaiting);
            _animWaiting = null;
        }
        _animWaiting = StartCoroutine(PlayAnimation(stateName,Time.time - _lastState.Item2 - 1.5f));
        
    }

    IEnumerator PlayAnimation(string stateName, float waitTime) {
        
        yield return new WaitForSeconds(waitTime);
        // Debug.Log(_animationClipDic[stateName]);
        animator.CrossFadeInFixedTime(_animationClipDic[stateName], _crossfadeDuration, 0);
        _lastState = new(stateName,Time.time);
        _animWaiting = null;
    }
}
