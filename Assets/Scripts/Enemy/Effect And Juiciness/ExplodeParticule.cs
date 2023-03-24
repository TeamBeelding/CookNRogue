using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeParticule : MonoBehaviour
{
    [SerializeField] private float _timeToDestroy = 1f;
    
    // Start is called before the first frame update
    private void Start()
    {
        Destroy(gameObject, _timeToDestroy);
    }

    private void Reset()
    {
        _timeToDestroy = 1f;
    }
}
