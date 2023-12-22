using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public sealed class RoomTransition : MonoBehaviour
{
    public static RoomTransition instance;
    [SerializeField] Animator _transitionAnimator;
    [SerializeField] AnimationClip _transitionClip;
    private string _transitionAnimationName;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }
    private void Start()
    {
        _transitionAnimationName = _transitionClip.name;
    }
    public void TriggerTransitionAnimation()
    {
        _transitionAnimator.Play(_transitionAnimationName);
    }

    public float GetAnimationDuration()
    {
        return _transitionClip.length;
    }
}
