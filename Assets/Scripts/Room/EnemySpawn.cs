using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject[] EasyEnemies;
    [SerializeField]
    private GameObject[] HardEnemies;
    [Header("If you want to specify an enemy put it here:")]
    [SerializeField]
    private GameObject CurrentEnemy;
    
    
    void Start()
    {
        if (CurrentEnemy)
        {
            Instantiate(CurrentEnemy, transform.position + Vector3.up, Quaternion.identity);
        }
        else if (!RoomManager.instance.isHard)
        {
            SpawnEnemy(EasyEnemies);
        }
        else
        {
            SpawnEnemy(HardEnemies);
        }
    }

    private void SpawnEnemy(GameObject[] enemy)
    {
        int rand = Random.Range(0, enemy.Length);
        CurrentEnemy = Instantiate(enemy[rand], transform.position + Vector3.up, Quaternion.identity);
    }
}
