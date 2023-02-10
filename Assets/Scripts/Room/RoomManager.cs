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
    private List<GameObject> EnemiesInLevel;

    public bool isHard;

    private int EnemyLeft;

    GameObject CurrentLevel;

    [SerializeField]
    GameObject Player;
    
    [SerializeField]
    Transform SpawnPoint;

    [SerializeField]
    TransitionController Transition;

    private NavMeshSurface _navMeshSurface;

    [SerializeField] 
    private float delaiBeforeCreateNavMesh = 0.2f;

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

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            EnemiesInLevel.Add(enemy);
        }

    }

    private void Update()
    {
        
    }

    public void RemoveEnemyCount()
    {
        EnemyLeft--;
        if (EnemyLeft <= 0)
        {
            GameObject.Find("Porte").SetActive(false);
        }
        Debug.Log(EnemyLeft);
    }

    // Update is called once per frame
    public void LoadRandomLevel()
    {

        //for (int i = 0; i < EnemiesInLevel.Count; i++)
        //{
        //    if (EnemiesInLevel[i] != null)
        //    {
        //        Destroy(EnemiesInLevel[i]);
        //    }
        //}


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

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            EnemiesInLevel.Add(enemy);
        }
    }

    private void LoadLevel(GameObject[] levels) 
    {
        int rand = Random.Range(0, levels.Length);
        CurrentLevel = Instantiate(levels[rand], Vector3.zero, Quaternion.identity);
        LoadMeshData();
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
