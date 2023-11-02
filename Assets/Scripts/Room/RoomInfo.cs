using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo : MonoBehaviour
{
    static RoomInfo _instance;
    public static RoomInfo Instance
    {
        get => _instance;
    }

    public event Action OnRoomStart;
    
    [SerializeField]
    private GameObject _spawnPoint;
    public GameObject SpawnPoint 
    { 
        get { return _spawnPoint; }
        set { _spawnPoint = value; }
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        StartCoroutine(RoomStartDelay());
    }

    IEnumerator RoomStartDelay()
    {
        yield return new WaitForEndOfFrame();
        RoomManager.instance.BuildNavMesh();
        PlayerController.Instance.Spawn(_spawnPoint.transform);
        OnRoomStart?.Invoke();
    }
}
