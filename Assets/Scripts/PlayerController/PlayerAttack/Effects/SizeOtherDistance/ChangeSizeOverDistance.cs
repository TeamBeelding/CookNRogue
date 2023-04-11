using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSizeOverDistance : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] AnimationCurve _sizeOverDistanceFactor;
    Transform sphere;
    float timePassed;

    private void Start()
    {
        sphere = transform.GetChild(0);
    }
    // Update is called once per frame
    private void Update()
    {
        
        timePassed += Time.deltaTime * _speed;

        if(timePassed * _speed <= 1)
            sphere.localScale = new Vector3(1,1,1) * _sizeOverDistanceFactor.Evaluate(timePassed);
        
    }

    public void SetParameters(float speed, AnimationCurve sizeOverDistanceFactor)
    {
        _speed = speed;
        _sizeOverDistanceFactor = sizeOverDistanceFactor;
    }
}
