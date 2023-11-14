using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<Transform> waveSpawner;

    [System.Serializable]
    public struct WaveStruct
    {
        public PoolType type;
        public int rating;
    }

    [SerializeField] private List<WaveStruct> wavesContainers;

    [SerializeField] private int waveNumber = 1;
    [SerializeField] private int enemiesInWave = 1;
    //[SerializeField] private int maxEnemiesInWave = 1;
    [SerializeField] private float delayBetweenWave = 1f;
    [SerializeField] private float delayBetweenSpawn = 1f;

    [SerializeField] private bool needAllEnemiesDiesBeforeNextWave = false;

    private bool allWaveEnd = false;
    private bool isWaveIsEnd = false;

    private Coroutine waveCoroutine;
    private Coroutine enemyCoroutine;

    private List<PoolType> allType = new List<PoolType>();

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            StartingWave();
        }
    }

    private void FixedUpdate()
    {
        if (allWaveEnd)
        {
            if (EnemyManager.Instance.EnemiesInLevel.Length == 0)
            {
                isWaveIsEnd = true;
            }
        }
    }

    private void StartingWave()
    {
        VerifyWaveManager();

        allWaveEnd = false;

        for (int i = 0; i < waveNumber; i++)
        {
            for (int j = 0; j < enemiesInWave; j++)
            {
                enemyCoroutine = StartCoroutine(IWaitForNextEnemieSpawn());
            }

            waveCoroutine = StartCoroutine(IWaitNextWave());
        }

        allWaveEnd = true;

        StopCoroutine(waveCoroutine);
        StopCoroutine(enemyCoroutine);

        Debug.Log("End of wave");
    }

    private void VerifyWaveManager()
    {
        if (waveSpawner != null || wavesContainers.Count == 0)
            Debug.LogWarning("Wave spawner ou wave container doesn't exist");

        foreach (WaveStruct wave in wavesContainers)
        {
            for (int i = 0; i < wave.rating; i++)
            {
                var waveType = wave.type;

                Debug.Log(waveType);
                allType.Add(waveType);
            }
        }
    }

    private Vector3 GetRandomSpawner()
    {
        int rand = Random.Range(0, waveSpawner.Count);

        return waveSpawner[rand].transform.position;
    }

    private PoolType GetRandomEnemy()
    {
        var rand = Random.Range(0, allType.Count);

        return wavesContainers[rand].type;
    }

    private IEnumerator IWaitNextWave()
    {
        if (needAllEnemiesDiesBeforeNextWave)
        {
            while (!isWaveIsEnd)
            {
                yield return new WaitForSeconds(1);
            }

            yield return null;
        }   
        else
        {
            yield return new WaitForSeconds(delayBetweenWave);
        }
    }

    private IEnumerator IWaitForNextEnemieSpawn()
    {
        PoolManager.Instance.InstantiateFromPool(GetRandomEnemy(), GetRandomSpawner(), Quaternion.identity);
        yield return new WaitForSeconds(delayBetweenSpawn);
    }
}
