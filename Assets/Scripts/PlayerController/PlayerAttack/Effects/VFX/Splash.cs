using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _particles;
    public void TriggerSplash()
    {
        foreach (ParticleSystem particle in _particles)
        {
            particle.Play();
        }
        Invoke("DisableSplash", 1);
    }

    void DisableSplash()
    {
        gameObject.SetActive(false);
    }
}
