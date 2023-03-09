using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimArrowController : MonoBehaviour
{

    [SerializeField]
    private float m_aimArrowSpeed = 7f;
    [SerializeField]
    private float m_aimArrowMaxSize = 1.1f;
    [SerializeField]
    private float m_zOffset = -0.1f;

    private PlayerController _playerController;


    private void Start()
    {
        _playerController = gameObject.GetComponentInParent<PlayerController>();
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
        if (transform.localScale.z <= m_aimArrowMaxSize * _playerController.PlayerAimMagnitude)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Mathf.Lerp(transform.localScale.z, transform.localScale.z + _playerController.PlayerAimMagnitude, m_aimArrowSpeed * Time.deltaTime));
            transform.localPosition = new Vector3(0, 0.001f, 4.9f * transform.localScale.z + m_zOffset);
        }
        else if (transform.localScale.z > m_aimArrowMaxSize * _playerController.PlayerAimMagnitude)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Mathf.Lerp(transform.localScale.z, 0, m_aimArrowSpeed * 2 * Time.deltaTime));
            transform.localPosition = new Vector3(0, 0.001f, 4.9f * transform.localScale.z + m_zOffset);
        }

        //if (_playerController.AimInputValue <= 0)
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
        //Debug.Log(_playerController.PlayerAimMagnitude);
        while (transform.localScale.z < m_aimArrowMaxSize * _playerController.PlayerAimMagnitude)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Mathf.Lerp(transform.localScale.z, transform.localScale.z + _playerController.PlayerAimMagnitude, m_aimArrowSpeed * Time.deltaTime)); //* m_aimArrowSpeed * Time.deltaTime);

            yield return null;
        }
        //_isAiming = false;
    }

}
