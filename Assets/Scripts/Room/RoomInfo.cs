using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo : MonoBehaviour
{
    [SerializeField]
    private GameObject _spawnPoint;
    public GameObject SpawnPoint 
    { 
        get { return _spawnPoint; }
        set { _spawnPoint = value; }
    }
}
