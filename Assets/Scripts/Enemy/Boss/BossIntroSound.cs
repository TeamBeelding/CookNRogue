using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIntroSound : MonoBehaviour
{

    [Header("Sound")]
    [SerializeField]
    private AK.Wwise.Event _Play_SFX_Boss_Intro;
    public void PlayBossIntroSound()
    {
        _Play_SFX_Boss_Intro.Post(gameObject);
        Debug.Log("hello");
    }
}
