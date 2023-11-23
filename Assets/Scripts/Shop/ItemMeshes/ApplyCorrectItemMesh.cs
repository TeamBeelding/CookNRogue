using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyCorrectItemMesh : MonoBehaviour
{
    [SerializeField] private MeshFilter _visibleMesh;
    [SerializeField] private MeshFilter _maskMesh;

    private void Start()
    {
        _maskMesh.sharedMesh = _visibleMesh.sharedMesh;
    }
}
