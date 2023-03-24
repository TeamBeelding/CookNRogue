using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;

    [SerializeField]
    private List<EnemyController> _enemiesInLevel = new List<EnemyController>();

    public int numOfEnemies;

    public event Action OnAllEnnemiesKilled;

    public static EnemyManager Instance
    {
        get { return _instance; }
    }

    public EnemyController[] EnemiesInLevel
    {
        get { return _enemiesInLevel.ToArray(); }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }

        _instance = this;
    }

    public void AddEnemyToLevel(EnemyController enemy)
    {
        _enemiesInLevel.Add(enemy);
        numOfEnemies++;
    }

    public void RemoveEnemyFromLevel(EnemyController enemy)
    {
        //HACK : List.Remove does not remove elements inside list for EnemyController type
        //So we check directly the instance ids to remove
        //(There is probably a better way to do this)
        for (int i = _enemiesInLevel.Count; i-- > 0;)
        {
            if (_enemiesInLevel[i].GetInstanceID() == enemy.GetInstanceID())
            {
                _enemiesInLevel.RemoveAt(i);
                numOfEnemies--;

            }
        }

        if (numOfEnemies <= 0 && OnAllEnnemiesKilled != null)
        {
            OnAllEnnemiesKilled?.Invoke();
        }
    }

    public void DestroyAll()
    {
        if (_enemiesInLevel != null)
        {
            int StartCount = _enemiesInLevel.Count;
            for (int i = StartCount - 1; i >= 0; i--)
            {
                var current = _enemiesInLevel[i];
                RemoveEnemyFromLevel(current);
                Destroy(current.gameObject);
            }
        }
    }
}