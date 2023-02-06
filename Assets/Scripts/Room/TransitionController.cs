using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionController : MonoBehaviour
{

    public Animator Transition;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.T)) 
        {
            LoadTransition();
        }
    }

    public void LoadTransition()
    {
        Transition.SetTrigger("Start");
        //if(Transition.GetBool("Start"))
            //Transition.ResetTrigger("Start");
    }

}
