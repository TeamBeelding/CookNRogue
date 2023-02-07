using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;
    [SerializeField]
    private GameObject[] EasyLevels;
    [SerializeField]
    private GameObject[] HardLevels;

    [SerializeField]
    private bool isHard;

    private int EnemyLeft;

    GameObject CurrentLevel;

    [SerializeField]
    GameObject Player;


    [SerializeField]
    Transform SpawnPoint;

    [SerializeField]
    TransitionController Transition;


    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);    // Suppression d'une instance pr�c�dente (s�curit�...s�curit�...)

        instance = this;
    }


    // Start is called before the first frame update
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        LoadRandomLevel();
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
        Transition.LoadTransition();

        if (CurrentLevel != null)
        {
            Destroy(CurrentLevel);
        }

        if (!isHard) 
        {
            int rand = Random.Range(0, EasyLevels.Length);
            CurrentLevel = Instantiate(EasyLevels[rand], Vector3.zero, Quaternion.identity);
            Player.transform.position = SpawnPoint.position;
        }
        else
        {
            int rand = Random.Range(0, HardLevels.Length);
            CurrentLevel = Instantiate(EasyLevels[rand], Vector3.zero, Quaternion.identity);
            Player.transform.position = SpawnPoint.position;
        }

        //EnemyLeft = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
}
