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

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (data)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, data.GetMaxRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, data.GetRadius);
        }
    }

#endif
}