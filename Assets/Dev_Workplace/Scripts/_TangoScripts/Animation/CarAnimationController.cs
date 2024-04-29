using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAnimationController : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] string _moveAnimation;
    [SerializeField] string _stopAnimation;
    [SerializeField] float _crossfadeDuration;

    //[SerializeField] CarAnimationState _carAnimationStates;




    // private void Update()
    // {
    //     //Test Button
    //     if (Input.GetKeyDown(KeyCode.T))
    //     {
    //         Debug.Log("T Down");
    //         MoveState();
    //     }
    //     if (Input.GetKeyDown(KeyCode.G))
    //     {
    //         Debug.Log("G Down");
    //         StopState();
    //     }

    // }

    public void MoveState()
    {
        //_animator.Play(_moveAnimation, 0);
        _animator.CrossFadeInFixedTime(_moveAnimation, _crossfadeDuration);
    }
    public void StopState()
    {
        //_animator.Play(_stopAnimation, 0);
        _animator.CrossFadeInFixedTime(_stopAnimation, _crossfadeDuration);
    }



}

// public enum CarAnimationState
// {
//     Stop, Move
// }
