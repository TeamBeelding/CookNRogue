using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    [SerializeField]
    private float m_shakeDuration = 1f;

    [SerializeField]
    private AnimationCurve m_shakeCurve;

    [SerializeField]
    private bool _shake = false;

    [SerializeField]
    private Transform m_cameraTransform;


    Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = m_cameraTransform.position;
        transform.rotation = m_cameraTransform.rotation;

        if (_shake)
        {
            _shake = false;
            StartCoroutine(Shake());
        }

        
        //m_cameraTransform  = GetComponentInParent<Transform>();
        //transform.position = GetComponentInParent<Transform>().position;

    }


    IEnumerator Shake()
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        while (elapsedTime < m_shakeDuration)
        {
            elapsedTime += Time.deltaTime;
            float strength = m_shakeCurve.Evaluate(elapsedTime / m_shakeDuration);
            transform.position = transform.position + Random.insideUnitSphere * strength;
            yield return null;
        }
        transform.position = startPosition;
    }
}
