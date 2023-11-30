using Enemy.DashingEnemy;
using UnityEngine;

public class CollisionBossEvent : MonoBehaviour
{
    private BossData data;
    private BossController bossController;

    private void Awake()
    {
        bossController = GetComponentInParent<BossController>();
    }

    private void OnEnable()
    {
        Reset();

        bossController = GetComponentInParent<BossController>();
    }

    private void Reset()
    {
        data = GetComponentInParent<BossController>().GetBossDataRef();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bossController.IsDashing())
        {
            if (other.gameObject.CompareTag("Player"))
                bossController.CollideWithPlayer();

            else if (other.gameObject.CompareTag("Obstruction"))
                bossController.CollideWithObstruction();
        }
        else
        {
            if (other.gameObject.CompareTag("Player"))
                PlayerController.Instance.TakeDamage(data.GetDamageOnHitPlayer);
        }
    }
}
