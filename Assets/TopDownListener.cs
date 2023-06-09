using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownListener : AkGameObj
{
    [SerializeField]
    private Transform player;

    public override Vector3 GetPosition()
    {
        return player.GetComponent<AkGameObj>().GetPosition();
    }

}
