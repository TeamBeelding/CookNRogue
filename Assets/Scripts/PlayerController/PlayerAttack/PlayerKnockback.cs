using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve m_playerKnockCurve;

    public bool isKnocked = false;

    [SerializeField]
    private float m_knockTime = 0.1f;

    [SerializeField]
    private float knockDistance = 0.5f;

    public void StartKnockback() 
    {
        StartCoroutine(IKnockback());
    }

    IEnumerator IKnockback()
    {
        // set a variable for the elapse
        float elapsedTime = 0f;
        isKnocked = true;
        // Getting start position of shake gimble in local space
        while (elapsedTime < knockTime)
        {

            // adding time to counter
            elapsedTime += Time.deltaTime;
            // strength of the curve at specific time. So strength over time (The y axis being strength, and x being time)
            float movement = m_playerKnockCurve.Evaluate(elapsedTime / m_knockTime) * knockDistance;
            // changing the local postion of _shake gimble inside the unit circle, so random position in a circle and adding the start position.
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -movement);
            yield return null;
        }
        isKnocked= false;
    }

}
