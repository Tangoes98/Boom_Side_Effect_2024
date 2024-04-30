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
        animator.CrossFadeInFixedTime(_animationClipDic[stateName], _crossfadeDuration, 0);
    }
}
