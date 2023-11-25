using System.Collections;
using UnityEngine;

[RequireComponent (typeof(BossController))]
public class MissilesController : MonoBehaviour
{
    private BossController boss;
    private BossData data;

    private void Awake()
    {
        boss = GetComponentInParent<BossController>();
    }

    private void OnEnable()
    {
        Reset();
    }

    private void Reset()
    {
        boss = GetComponentInParent<BossController>();
        data = GetComponent<BossController>().GetBossDataRef();
    }

    public void LaunchMissiles()
    {
        StartCoroutine(IDelayForEachMissile());

        IEnumerator IDelayForEachMissile()
        {
            for (int i = 0; i < data.GetMissilesCount; i++)
            {
                //Debug.Log($"Launch at {boss.GetTargetPosition()}");
                MissileBoss missile = MissileManager.instance.GetAvailableMissile();
                missile.transform.position = boss.GetTargetPosition();
                missile.Init(data.damage);

                yield return new WaitForSeconds(data.GetDelayForEachMissiles);
            }
        }
    }
}