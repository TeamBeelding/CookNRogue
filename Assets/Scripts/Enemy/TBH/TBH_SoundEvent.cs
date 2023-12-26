using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBH_SoundEvent : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField]
    private AK.Wwise.Event _Play_SFX_Carrot_Dive;
    [SerializeField]
    private AK.Wwise.Event _Play_SFX_Carrot_Erupt;

    [SerializeField] private Outline outline;

    private void Awake()
    {
        outline = GetComponentInParent<Outline>();

        outline.enabled = true;
    }

    public void PlayDive()
    {
        outline.enabled = false;
        
        _Play_SFX_Carrot_Dive.Post(gameObject);
    }

    public void PlayErupt()
    {
        outline.enabled = true;

        _Play_SFX_Carrot_Erupt.Post(gameObject);
    }
}
