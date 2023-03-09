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

    private PlayerController playerController;


    private void Start()
    {
        playerController = gameObject.GetComponentInParent<PlayerController>();
    }

    void OnDisable()
    {
        transform.localScale = new Vector3(transform.localScale.x,
                                  transform.localScale.y,
                                  0);
        transform.localPosition = new Vector3(0, 0.001f, 0);
    }


    private void Update()
    {
        if (transform.localScale.z <= aimArrowMaxSize * playerController.PlayerAimMagnitude)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Mathf.Lerp(transform.localScale.z, transform.localScale.z + playerController.PlayerAimMagnitude, aimArrowSpeed * Time.deltaTime));
            transform.localPosition = new Vector3(0, 0.001f, 4.9f * transform.localScale.z + zOffset);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Mathf.Lerp(transform.localScale.z, 0, aimArrowSpeed * 10 * Time.deltaTime));
            transform.localPosition = new Vector3(0, 0.001f, 4.9f * transform.localScale.z + zOffset);
        }

        //if (playerController.AimInputValue <= 0)
        //{
        //    gameObject.SetActive(false);
        //}
    }

    //void OnEnable()
    //{
    //    StartCoroutine(IAimArrow());
    //}

    IEnumerator IAimArrow()
    {
        //Debug.Log(playerController.PlayerAimMagnitude);
        while (transform.localScale.z < aimArrowMaxSize * playerController.PlayerAimMagnitude)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Mathf.Lerp(transform.localScale.z, transform.localScale.z + playerController.PlayerAimMagnitude, aimArrowSpeed * Time.deltaTime)); //* aimArrowSpeed * Time.deltaTime);

            yield return null;
        }
        //_isAiming = false;
    }

}
