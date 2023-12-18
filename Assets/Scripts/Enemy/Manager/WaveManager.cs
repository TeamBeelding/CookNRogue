using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private enum State
    {
        VerifyContent,
        NextWave,
        WaitAllEnemiesDie,
        EndWave
    }

    private State state;

    [SerializeField] private List<WaveSpawner> waveSpawner;

    [SerializeField] private float delayBeforeStartingWave = 1;
    [SerializeField] private float delayBetweenEachWave = 2;
    [SerializeField] private bool allAIDieBeforeNextWave = false;

    private int waveSpawnerCount = 0;

    private Coroutine stateCoroutine;

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
            case State.NextWave:
                NextWave();
                break;
            case State.WaitAllEnemiesDie:
                WaitingAllEnemieDie();
                break;
            case State.EndWave:
                DestroyAllAI();
                StopAllCoroutines();
                break;
        }
    }

    private void VerifyWaveManager()
    {
        foreach (WaveSpawner ws in GetComponentsInChildren<WaveSpawner>())
        {
            waveSpawner.Add(ws);
            waveSpawnerCount++;
        }

        if (waveSpawner.Count == 0)
            SetState(State.EndWave);

        if (gameObject.activeInHierarchy)
            stateCoroutine = StartCoroutine(IDelayBeforeStartingWave());

        IEnumerator IDelayBeforeStartingWave()
        {
            yield return new WaitForSeconds(delayBeforeStartingWave);

            SetState(State.NextWave);
        }
    }

    private void NextWave()
    {
        if (gameObject.activeInHierarchy)
            stateCoroutine = StartCoroutine(IDelayBeteweenEachWave());

        IEnumerator IDelayBeteweenEachWave()
        {
            foreach (WaveSpawner ws in waveSpawner)
                ws.SpawnAIFromWave();

            if (allAIDieBeforeNextWave)
                SetState(State.WaitAllEnemiesDie);
            else
            {
                yield return new WaitForSeconds(delayBeforeStartingWave);
                SetState(State.NextWave);
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
                if (EnemyManager.Instance.GetNumOfEnemies() > 0)
                    yield return new WaitForSeconds(1);
                else
                {
                    foreach (WaveSpawner ws in waveSpawner)
                    {
                        if (!ws.IsWaveIsEnd())
                            continue;
                        else
                        {
                            ws.gameObject.SetActive(false);
                            waveSpawnerCount--;
                        }
                    }

                    if (waveSpawnerCount > 0)
                    {
                        yield return new WaitForSeconds(delayBetweenEachWave);
                        SetState(State.NextWave);
                    }
                    else
                        SetState(State.EndWave);
                }
            }
        }
    }

    public void DestroyAllAI()
    {
        foreach (WaveSpawner ws in GetComponentsInChildren<WaveSpawner>())
            ws.DespawnAI();
    }

    public void SlowMotion()
    {
        List<WaveSpawner> waveList = new List<WaveSpawner>();

        foreach (WaveSpawner ws in GetComponentsInChildren<WaveSpawner>())
        {
            if (ws.IsLastWave())
                waveList.Add(ws);
        }

        if (waveList.Count == 0)
            return;
        else if (waveList[0].IsLastWave())
        {
            if (EnemyManager.Instance.GetNumOfEnemies() == 1)
            {
                EnemyManager.Instance.LastAIDying();
                print("Last AI Dying -- Function called");
            }
        }
    }
}
