using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve PlayerKnockCurve;

    public bool isKnocked = false;

    [SerializeField]
    private float knockTime = 0.1f;

    [SerializeField]
    private float knockDistance = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isKnocked)
        {
            StartKnockback();
            isKnocked = false;
        }
    }

    public void StartKnockback() 
    {
        StartCoroutine(IKnockback());
    }

    IEnumerator IKnockback()
    {
        // set a variable for the elapse
        float elapsedTime = 0f;
        // Getting start position of shake gimble in local space
        while (elapsedTime < knockTime)
        {

            // adding time to counter
            elapsedTime += Time.deltaTime;
            // strength of the curve at specific time. So strength over time (The y axis being strength, and x being time)
            float movement = PlayerKnockCurve.Evaluate(elapsedTime / knockTime) * knockDistance;
            Debug.Log(movement);
            // changing the local postion of shake gimble inside the unit circle, so random position in a circle and adding the start position.
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -movement);
            yield return null;
        }
    }

}
