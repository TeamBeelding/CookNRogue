using System.Collections;
using UnityEngine;

[RequireComponent (typeof(BossController))]
public class MissilesController : MonoBehaviour
{
    private BossController boss;
    private BossData data;

    [Header("DISPLACEMENT")]
    [SerializeField] bool _randomDisplacement;
    [SerializeField] float _displacementAmount;

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
                Vector3 target = boss.GetTargetPosition();

                if (_randomDisplacement)
                {
                    float displacementX = Random.Range(-_displacementAmount, _displacementAmount);
                    float displacementZ = Random.Range(-_displacementAmount, _displacementAmount);
                    target += new Vector3(displacementX, 0, displacementZ);
                }

                missile.Init(data.damage, target, transform.position);

                yield return new WaitForSeconds(data.GetDelayForEachMissiles);
            }
        }
    }
}