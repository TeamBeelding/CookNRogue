using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowColliderGizmos : MonoBehaviour
{
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] bool draw = true;
    [SerializeField, Range(0.0f, 1.0f)] float colorTransparency = 0.5f;

    private void DrawGizmosOnRunTime(Color color)
    {
        if (boxCollider != null && draw)
        {
            Gizmos.color = color;
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(boxCollider.transform.position, boxCollider.transform.rotation, boxCollider.transform.lossyScale);
            Gizmos.matrix = rotationMatrix;

            Gizmos.DrawCube(boxCollider.center, boxCollider.size);
            
        }
    }

    private void OnDrawGizmos()
    {
        Color c = Color.red;
        c.a = colorTransparency;
        DrawGizmosOnRunTime(c);
    }
}
