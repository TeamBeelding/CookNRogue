using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Serialization;


public class LevelData
{
    [SerializeField]
    private GameObject _level;
    [SerializeField]
    private int _prob;

    public GameObject Level
    {
        get => _level;
        set { _level = value; }
    }

    public int Prob
    {
        get => _prob;
        set { _prob = value; }
    }
}