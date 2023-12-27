using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{

    [System.Serializable]
    public struct WaveContainer
    {
        public int waveIndex;
        public PoolType IAType;
    }

    [SerializeField] private List<WaveContainer> waveContainer;
    [SerializeField] GameObject fx;
    [SerializeField] private float delay = 0.25f;
    [SerializeField] private AK.Wwise.Event _Play_SFX_Enemy_Spawn_Magic;

    private GameObject ai;

    private int waveCount = 1;
    private int currentWaveIndex = 0;

    private void Awake()
    {
        waveCount = waveContainer.Count;
    }

    public void SpawnAIFromWave()
    {
        if (currentWaveIndex >= waveCount)
            return;

        currentWaveIndex++;

        StartCoroutine(IDelay());

        IEnumerator IDelay()
        {
            if (waveContainer[currentWaveIndex - 1].IAType != PoolType.None)
            {
                GameObject dirtFX = Instantiate(fx, transform.position, Quaternion.identity);
                _Play_SFX_Enemy_Spawn_Magic.Post(dirtFX);

                dirtFX.transform.parent = transform;
                dirtFX.SetActive(true);

                dirtFX.GetComponentInChildren<ParticleSystem>().Play();

                yield return new WaitForSeconds(delay);

                ai = PoolManager.Instance.InstantiateFromPool(waveContainer[currentWaveIndex - 1].IAType, transform.position, Quaternion.identity);
            }
        }
    }

    public void DespawnAI()
    {
        fx?.SetActive(false);

        if (ai != null)
            PoolManager.Instance.DesinstantiateFromPool(ai);
    }

    public bool IsWaveIsEnd() => currentWaveIndex >= waveCount;

    public bool IsLastWave() => currentWaveIndex == waveCount;
}
