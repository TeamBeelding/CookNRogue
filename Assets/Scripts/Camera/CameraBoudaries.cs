using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CameraBoudaries : MonoBehaviour
{
    public static CameraBoudaries instance;

    Bounds bounds;
    [SerializeField] Vector3 _extent;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }

        instance = this;

        bounds.center = transform.position;
        bounds.min = -Vector3.one;
        bounds.max = Vector3.one;
        bounds.extents = _extent;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        bounds.center = transform.position;
        bounds.min = transform.position - Vector3.one;
        bounds.max = transform.position+ Vector3.one;
        bounds.extents = _extent;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }

    public bool CheckCameraBoundaries(Vector3 CamPosition)
    {
        return bounds.Contains(CamPosition);
    }
}
