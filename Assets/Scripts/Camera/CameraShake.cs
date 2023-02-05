using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    [SerializeField]
    private float ShakeDuration = 1f;

    [SerializeField]
    private AnimationCurve ShakeCurve;

    [SerializeField]
    private bool shake = false;

    [SerializeField]
    private Transform CameraTransform;


    Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = CameraTransform.position;
        transform.rotation = CameraTransform.rotation;

        if (shake)
        {
            shake = false;
            StartCoroutine(Shake());
        }

        
        //CameraTransform  = GetComponentInParent<Transform>();
        //transform.position = GetComponentInParent<Transform>().position;

    }


    IEnumerator Shake()
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        while (elapsedTime < ShakeDuration)
        {
            elapsedTime += Time.deltaTime;
            float strength = ShakeCurve.Evaluate(elapsedTime / ShakeDuration);
            transform.position = transform.position + Random.insideUnitSphere * strength;
            yield return null;
        }
        transform.position = startPosition;
    }
}
