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
    [SerializeField] private bool allAIDieBeforeNextWave = false;

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
                StopAllCoroutines();
                EnemyManager.Instance.EndWave();
                gameObject.SetActive(false);
                break;
        }
    }

    private void VerifyWaveManager()
    {
        foreach (WaveSpawner ws in GetComponentsInChildren<WaveSpawner>())
            waveSpawner.Add(ws);

        if (waveSpawner.Count == 0)
        {
            Debug.LogWarning("Wave spawner doesn't exist");
            SetState(State.EndWave);
        }

        SetState(State.NextWave);
    }

    private void NextWave()
    {
        stateCoroutine = StartCoroutine(IDelayBeforeStartingWave());

        IEnumerator IDelayBeforeStartingWave()
        {
            yield return new WaitForSeconds(delayBeforeStartingWave);

            foreach (WaveSpawner ws in waveSpawner)
                ws.SpawnAIFromWave();

            if (allAIDieBeforeNextWave)
                SetState(State.WaitAllEnemiesDie);
            else
                SetState(State.NextWave);
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
                {
                    foreach (WaveSpawner ws in waveSpawner)
                    {
                        if (!ws.IsWaveIsEnd())
                            continue;

                        else
                            SetState(State.EndWave);
                    }

                    SetState(State.NextWave);
                }
            }
        }
    }

    public void DestroyAllAI()
    {
        foreach (WaveSpawner ws in GetComponentsInChildren<WaveSpawner>())
            ws.DespawnAI();
    }
}
