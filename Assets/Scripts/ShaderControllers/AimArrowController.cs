using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimArrowController : MonoBehaviour
{

    [SerializeField]
    float aimArrowSpeed = 0.1f;
    [SerializeField]
    float aimArrowMaxSize = 1;
    [SerializeField]
    float zOffset = -1;

    void OnDisable()
    {
        transform.localScale = new Vector3(transform.localScale.x,
                                  transform.localScale.y,
                                  0);
        transform.localPosition = new Vector3(0, 0.001f, 0);
    }

    void OnEnable()
    {
        StartCoroutine(IAimArrow());
    }

    IEnumerator IAimArrow()
    {
        while (transform.localScale.z < aimArrowMaxSize)
        {
            transform.localScale = new Vector3(transform.localScale.x,
                                               transform.localScale.y,
                                               transform.localScale.z + aimArrowSpeed * Time.deltaTime);
            transform.localPosition = new Vector3(0, 0.001f, 4.9f * transform.localScale.z + zOffset);
            yield return null;
        }
        //_isAiming = false;
    }

}
