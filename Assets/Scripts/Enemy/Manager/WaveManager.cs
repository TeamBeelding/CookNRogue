using Enemy;
using System;
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

    int count = 0;
    List<WaveSpawner> waveList = new List<WaveSpawner>();

    private Coroutine stateCoroutine;

    static int test = 0;

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
                PoolManager.Instance.DestroyAI();

                //Ammo Pause
                PlayerController.Instance.AttackScript.PauseAmmoTimer = true;

                StopAllCoroutines();
                break;
        }
    }

    private void VerifyWaveManager()
    {
        foreach (WaveSpawner ws in GetComponentsInChildren<WaveSpawner>())
            waveSpawner.Add(ws);

        if (waveSpawner.Count == 0)
            SetState(State.EndWave);

        if (gameObject.activeInHierarchy)
            stateCoroutine = StartCoroutine(IDelayBeforeStartingWave());

        IEnumerator IDelayBeforeStartingWave()
        {
            yield return new WaitForSeconds(delayBeforeStartingWave);

            //Ammo pause
            PlayerController.Instance.AttackScript.PauseAmmoTimer = false;

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

            yield return new WaitForSeconds(1f);

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
        int count = 0;
        stateCoroutine = StartCoroutine(IWaitingAllAIDie());

        IEnumerator IWaitingAllAIDie()
        {
            while (state == State.WaitAllEnemiesDie)
            {
                if (EnemyManager.Instance.GetNumOfEnemies() > 0)
                    yield return new WaitForSeconds(1);
                else
                {
                    //foreach (WaveSpawner ws in waveSpawner)
                    //{
                    //    if (!ws.IsWaveIsEnd())
                    //        continue;
                    //    else
                    //    {
                    //        ws.gameObject.SetActive(false);
                    //        //waveSpawnerCount--;
                    //    }
                    //}

                    foreach (WaveSpawner ws in GetComponentsInChildren<WaveSpawner>())
                    {
                        if (!ws.IsWaveIsEnd())
                            count++;
                    }

                    if (count > 0)
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
        count = 0;
        waveList = new List<WaveSpawner>();

        foreach (WaveSpawner ws in GetComponentsInChildren<WaveSpawner>())
        {
            count++;

            if (ws.IsLastWave())
                waveList.Add(ws);
        }

        if (waveList.Count == count)
        {
            if (EnemyManager.Instance.GetNumOfEnemies() == 1)
            {
                EnemyManager.Instance.LastAIDying();
                print("Last AI Dying -- Function called");
            }
            else
            {
                Debug.Log($"<color=red>AI remaining : {EnemyManager.Instance.GetNumOfEnemies() -1}</color>");
            }
        }
        else
        {
            Debug.Log($"<color=blue>Wave count != count {waveList.Count} - {count}</color>");
        }
    }
}
