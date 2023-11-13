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
        public int currency;
    }

    [SerializeField] private List<WaveStruct> wavesContainers;

    [SerializeField] private int waveNumber = 1;
    [SerializeField] private int minEnemiesInWave = 1;
    [SerializeField] private int maxEnemiesInWave = 1;
    [SerializeField] private float delayBetweenWave = 1f;
    [SerializeField] private float delayBetweenSpawn = 1f;

    [SerializeField] private bool needAllEnemiesDiesBeforeNextWave = false;

    private bool isWaveIsEnd = false;

    private Coroutine waveCoroutine;
    private Coroutine enemyCoroutine;

    private List<PoolType> allType;

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
     
        VerifyWaveManager();
    }

    private void Start()
    {
        for (int i = 0; i < waveNumber; i++)
        {
            for(int j = 0; j < minEnemiesInWave; j++)
            {
                enemyCoroutine = StartCoroutine(IWaitForNextEnemieSpawn());
            }

            waveCoroutine = StartCoroutine(IWaitNextWave());
        }

        isWaveIsEnd = true;

        StopCoroutine(waveCoroutine);
        StopCoroutine(enemyCoroutine);
    }

    private void VerifyWaveManager()
    {
        if (waveSpawner != null || wavesContainers.Count == 0)
            Debug.LogWarning("Wave spawner ou wave container doesn't exist");

        foreach (WaveStruct wave in wavesContainers)
        {
            for (int i = 0; i < wave.currency; i++)
                allType.Add(wave.type);
        }        
        
        foreach (WaveStruct wave in wavesContainers)
        {
            if (wave.currency == 0)
                wavesContainers.Remove(wave);
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
        yield return new WaitForSeconds(delayBetweenWave);
    }

    private IEnumerator IWaitForNextEnemieSpawn()
    {
        PoolManager.Instance.InstantiateFromPool(GetRandomEnemy(), GetRandomSpawner(), Quaternion.identity);
        yield return new WaitForSeconds(delayBetweenSpawn);
    }
}
