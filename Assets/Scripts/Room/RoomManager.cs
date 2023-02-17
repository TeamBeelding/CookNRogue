using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// using UnityEngine.AI;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;
    [SerializeField]
    private GameObject[] EasyLevels;
    [SerializeField]
    private GameObject[] HardLevels;

    [SerializeField]
    private EnemyManager EnemyManagerScript;  

    public bool isHard;

    GameObject CurrentLevel;

    [SerializeField]
    GameObject Player;
    
    public Transform SpawnPoint;

    [SerializeField]
    TransitionController Transition;

    private NavMeshSurface _navMeshSurface;

    [SerializeField] 
    private float delaiBeforeCreateNavMesh = 0.2f;

    public event Action OnRoomStart;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);    // Suppression d'une instance pr�c�dente (s�curit�...s�curit�...)

        instance = this;
    }


    // Start is called before the first frame update
    private void Start()
    {        
        _navMeshSurface = GetComponent<NavMeshSurface>();

        Player = GameObject.FindGameObjectWithTag("Player");
        LoadRandomLevel();
    }

    public void LoadRandomLevel()
    {

        EnemyManagerScript.DestroyAll();

        Transition.LoadTransition();

        if (CurrentLevel != null)
        {
            Destroy(CurrentLevel);
        }

        if (!isHard)
        {
            LoadLevel(EasyLevels);
        }
        else
        {
            LoadLevel(HardLevels);
        }
    }

    private void LoadLevel(GameObject[] levels) 
    {
        int rand = UnityEngine.Random.Range(0, levels.Length);
        CurrentLevel = Instantiate(levels[rand], Vector3.zero, Quaternion.identity);
        LoadMeshData();

        OnRoomStart();
        Player.transform.position = SpawnPoint.position;
    }

    private void LoadMeshData()
    {
        NavMeshData navMeshData = CurrentLevel.GetComponent<NavMeshRoom>()._data;
        _navMeshSurface.UpdateNavMesh(navMeshData);
        
        // Do we need to Build everytime or only once?
        _navMeshSurface.BuildNavMesh();
    }

}
