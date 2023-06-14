using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventCorn : MonoBehaviour
{
    [SerializeField]
    private AK.Wwise.Event _Play_SFX_Corn_Footsteps;
    
    public void CornFootstep()
    {
        _Play_SFX_Corn_Footsteps.Post(gameObject);
    }
}
