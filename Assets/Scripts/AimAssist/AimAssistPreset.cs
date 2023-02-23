using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newAimAssistPreset", menuName = "Player/AimAssistPreset")]
public class AimAssistPreset : ScriptableObject
{
    [SerializeField]
    float m_maxAngle;

    [SerializeField]
    float m_maxDistance;

    [SerializeField]
    AnimationCurve m_assistOffsetCurve;

    #region Getters
    public float GetMaxAngle
    {
        get
        {
            return m_maxAngle;
        }
    }

    public float GetMaxDistance
    {
        get
        {
            return m_maxDistance;
        }
    }

    public AnimationCurve GetAssistOffsetCurve
    {
        get
        {
            return m_assistOffsetCurve;
        }
    }
    #endregion
}
