using System.Collections;
using UnityEngine;

[RequireComponent (typeof(BossController))]
public class MissilesController : MonoBehaviour
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

    public void LaunchMissiles()
    {
        StartCoroutine(IDelayForEachMissile());

        IEnumerator IDelayForEachMissile()
        {
            for (int i = 0; i < data.GetMissilesCount; i++)
            {
                Vector3 lastPlayerPosition = PlayerController.Instance.transform.position;

                Debug.Log($"Launch at {lastPlayerPosition}");
                yield return new WaitForSeconds(data.GetDelayForEachMissiles);
            }
        }
    }
}