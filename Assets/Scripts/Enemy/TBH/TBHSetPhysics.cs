using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBHSetPhysics : MonoBehaviour
{
    [SerializeField]
    GameObject physics;

    public void EnableCollider()
    {
        physics.SetActive(true);
    }

    public void DisableCollider()
    {
        physics.SetActive(false);
    }
}
