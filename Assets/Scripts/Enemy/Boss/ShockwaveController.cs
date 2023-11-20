using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BossController))]
public class ShockwaveController : MonoBehaviour
{
    private BossData data;

    private void OnEnable()
    {
        Reset();
    }

    private void Reset()
    {
        data = GetComponent<BossController>().GetBossDataRef();
    }

    public void StartShockwave()
    {
        StartCoroutine(IDelayForEachShockwave());

        IEnumerator IDelayForEachShockwave()
        {
            for (int i = 0; i < data.GetShockwaveCount; i++)
            {
                yield return new WaitForSeconds(data.GetDelayForEachShockWave);
            }
        }
    }
}