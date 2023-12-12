using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventCarrot : MonoBehaviour
{
    [SerializeField]
    private AK.Wwise.Event _Play_SFX_Carrot_Dive;
    [SerializeField]
    private AK.Wwise.Event _Play_SFX_Carrot_Erupt;

    public void CarrotDive()
    {
        _Play_SFX_Carrot_Dive.Post(gameObject);
    }
    public void CarrotErupt()
    {
        _Play_SFX_Carrot_Erupt.Post(gameObject);
    }
}
