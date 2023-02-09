using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] EnemiesInLevel;
    [SerializeField]
    private int numOfEnemies;
    void Start()
    {
        EnemiesInLevel = GameObject.FindGameObjectsWithTag("Enemy");
        //numOfEnemies
    }

    void Update()
    {

    }

    private void RemoveEnemyCount() 
    
    {
    
    }
}
