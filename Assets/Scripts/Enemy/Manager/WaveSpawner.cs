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

        if (waveContainer[currentWaveIndex - 1].IAType != PoolType.None)
            ai = PoolManager.Instance.InstantiateFromPool(waveContainer[currentWaveIndex - 1].IAType, transform.position, Quaternion.identity);
    }

    public void DespawnAI()
    {
        if (ai != null)
            PoolManager.Instance.DesinstantiateFromPool(ai);
    }

    public bool IsWaveIsEnd() => currentWaveIndex >= waveCount;
}