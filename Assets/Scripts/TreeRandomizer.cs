using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TreeRandomizer : MonoBehaviour
{
    public TreeData[] trees;

    private MeshRenderer _meshRenderer;


    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        var random = Random.Range(0f, 1f);
        var last = 0f;
        foreach (var treeData in trees)
        {
            last += treeData.chance;
            if (random <= last)
            {
                _meshRenderer.material = treeData.material;
                break;
            }
        }
    }

    
    [Serializable]
    public class TreeData
    {
        public Material material;
        
        [Range(0f,1f)]
        public float chance;
    }
}