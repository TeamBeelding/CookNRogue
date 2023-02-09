using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] EnemiesInLevel;

    public int numOfEnemies;

    [SerializeField] 
    private float timeEnemyDeathCheck;

    [SerializeField]
    private float time;

    void Start()
    {
        time = timeEnemyDeathCheck;
    }

    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            time = timeEnemyDeathCheck;
            Countdown();
        }
    }

    void Countdown()
    {
        //yield return new WaitForSeconds(seconds);
        if (GameObject.FindGameObjectWithTag("Enemy"))
        {
            EnemiesInLevel = GameObject.FindGameObjectsWithTag("Enemy");
        }

        numOfEnemies = EnemiesInLevel.Length;

        if (!EnemiesInLevel[0])
        {
            if (GameObject.Find("Porte")) 
            {
                GameObject.Find("Porte").SetActive(false);
            }
        }

        Debug.Log("Enemy");
    }
}
