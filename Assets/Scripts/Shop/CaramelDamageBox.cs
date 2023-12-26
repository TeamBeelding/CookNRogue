using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaramelDamageBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (!col.transform.parent)
            return;

        if (col.transform.parent.TryGetComponent(out EnemyController controler))
        {
            controler.TakeDamage(PlayerRuntimeData.GetInstance().data.InventoryData.CaramelDamage);
        }
    }
}
