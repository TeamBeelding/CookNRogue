using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TreeRandomizer : MonoBehaviour
{
    public TreeData[] trees;
    public TreeSize forceSize;
    
    private MeshFilter _meshFilter;
    

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
    }

    private void Start()
    {
        var random = Random.Range(0, 3);
        TreeSize treeSize = TreeSize.small;

        if (random == 0) treeSize = TreeSize.small;
        else if (random == 1) treeSize = TreeSize.medium;
        else if (random == 2) treeSize = TreeSize.big;

        List<TreeData> availableTrees = new List<TreeData>();
        foreach (var treeData in trees)
        {
            if(treeData.size == treeSize)
                availableTrees.Add(treeData);
        }

        random = Random.Range(0, availableTrees.Count);
        _meshFilter.mesh = availableTrees[random].mesh;
    }

    public enum TreeSize
    {
        small,
        medium,
        big
    }
    public enum TreeColor
    {
        green,
        yellow,
        red
    }
    
    [Serializable]
    public class TreeData
    {
        public TreeSize size;
        public TreeColor color;
        public Mesh mesh;
    }
}
