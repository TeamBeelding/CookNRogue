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

    [SerializeField] private List<WaveContainer> list;

    private int waveCount = 1;
    private int currentWaveIndex = 0;

    private void Awake()
    {
        waveCount = list.Count;
    }

    public void SpawnAIFromWave()
    {
        if (currentWaveIndex >= waveCount)
            return;

        currentWaveIndex++;

        PoolManager.Instance.InstantiateFromPool(list[currentWaveIndex - 1].IAType, transform.position, Quaternion.identity);
    }

    public bool IsWaveIsEnd() => currentWaveIndex > waveCount;
}
