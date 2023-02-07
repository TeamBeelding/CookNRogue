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
        Transition.SetBool("Start", true);

        StartCoroutine(Countdown(2));
    }

    IEnumerator Countdown(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        Transition.SetBool("Start", false);
    }

}
