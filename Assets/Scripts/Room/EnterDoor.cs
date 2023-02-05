using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDoor : MonoBehaviour
{
    public GameObject door;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !door.activeInHierarchy)
        {
            RoomManager.instance.LoadRandomLevel();
        }
    }
}
