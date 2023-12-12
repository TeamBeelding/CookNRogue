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

    public void PlayDive()
    {
        _Play_SFX_Carrot_Dive.Post(gameObject);
    }

    public void PlayErupt()
    {
        _Play_SFX_Carrot_Erupt.Post(gameObject);
    }
}
