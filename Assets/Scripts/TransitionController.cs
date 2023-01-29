using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionController : MonoBehaviour
{

    public Animator Transition;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) 
        {
            LoadTransition();
        }
    }

    public void LoadTransition()
    {
        Transition.ResetTrigger("Start");
    }

}
