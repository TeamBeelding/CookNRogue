using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private enum State
    {
        VerifyContent,
        SpawnEnemie,
        NextWave,
        WaitAllEnemiesDie,
        EndWave
    }

    private State state;

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
    [SerializeField] private float delayBeforeWave = 1f;
    [SerializeField] private float delayBetweenWave = 1f;
    [SerializeField] private float delayBetweenSpawn = 1f;

    [SerializeField] private bool needAllEnemiesDiesBeforeNextWave = false;

    private Coroutine stateCoroutine;

    private List<PoolType> allType = new List<PoolType>();

    private int indexOfEnemie;
    private int indexOfWave;

    private void Start()
    {
        SetState(State.VerifyContent);
    }

    private void SetState(State newValue)
    {
        if (stateCoroutine != null)
            stateCoroutine = null;

        state = newValue;

        StateManagement();
    }

    private void StateManagement()
    {
        switch (state)
        {
            case State.VerifyContent:
                VerifyWaveManager();
                break;
            case State.SpawnEnemie:
                SpawnEnemies();
                break;
            case State.NextWave:
                NextWave();
                break;
            case State.WaitAllEnemiesDie:
                WaitingAllEnemieDie();
                break;
            case State.EndWave:
                EnemyManager.Instance.EndWave();
                break;
        }
    }

    private void VerifyWaveManager()
    {
        if (waveSpawner == null || wavesContainers.Count == 0)
        {
            Debug.LogWarning("Wave spawner or wave container doesn't exist");
            SetState(State.EndWave);
        }
        else
        {
            foreach (WaveStruct wave in wavesContainers)
            {
                for (int i = 1; i <= wave.rating; i++)
                    allType.Add(wave.type);
            }

            if (allType.Count > 0)
                SetState(State.NextWave);
            else
                SetState(State.EndWave);
        }
    }

    private void SpawnEnemies()
    {
        stateCoroutine = StartCoroutine(ISpawnEnemies());

        IEnumerator ISpawnEnemies()
        {
            while (state == State.SpawnEnemie)
            {
                indexOfEnemie++;

                if (indexOfEnemie > enemiesInWave)
                {
                    if (needAllEnemiesDiesBeforeNextWave)
                        SetState(State.WaitAllEnemiesDie);
                    else
                        SetState(State.NextWave);
                }
                else
                {
                    PoolManager.Instance.InstantiateFromPool(GetRandomEnemy(), GetRandomSpawner(), Quaternion.identity);
                    yield return new WaitForSeconds(delayBetweenSpawn);
                }
            }
        }
    }

    private void NextWave()
    {
        indexOfWave++;
        indexOfEnemie = 0;

        if (indexOfWave > waveNumber)
            SetState(State.EndWave);
        else
            stateCoroutine = StartCoroutine(IWaitingWaveDelay());

        IEnumerator IWaitingWaveDelay()
        {
            while (state == State.NextWave)
            {
                yield return new WaitForSeconds(delayBetweenWave);
                SetState(State.SpawnEnemie);
            }
        }
    }

    private void WaitingAllEnemieDie()
    {
        stateCoroutine = StartCoroutine(IWaitingAllAIDie());

        IEnumerator IWaitingAllAIDie()
        {
            while (state == State.WaitAllEnemiesDie)
            {
                if (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
                    yield return new WaitForSeconds(1);
                else
                    SetState(State.NextWave);
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
        int rand = Random.Range(0, allType.Count - 1);

        return wavesContainers[rand].type;
    }
}
