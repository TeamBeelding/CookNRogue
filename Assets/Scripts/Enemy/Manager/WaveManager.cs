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

    [Header("Sound")]
    [SerializeField]
    private AK.Wwise.State _InFightOn;
    [SerializeField]
    private AK.Wwise.State _NoMusic;

    int count = 0;
    List<WaveSpawner> waveList = new List<WaveSpawner>();

    private Coroutine stateCoroutine;

    //private bool isAllWaveDone = false;
    private bool isSlowWasCalled = false;

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

                if (!isSlowWasCalled)
                    EnemyManager.Instance.LastAIDying();

                //Ammo Pause
                PlayerController.Instance.AttackScript.PauseAmmoTimer = true;
                //Camera
                CameraController.instance.ScreenZoom(true);
                //Audios
                _NoMusic.SetValue();

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
            //Camera
            CameraController.instance.ScreenZoom(false);
            //Audio
            _InFightOn.SetValue();

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
                isSlowWasCalled = true;

                EnemyManager.Instance.LastAIDying();
                print("<color=green>Last AI Dying -- Function called</color>");

                SetState(State.EndWave);
            }
        }
    }
}
