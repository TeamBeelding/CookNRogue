using System.Collections;
using UnityEngine;

public class MissileBoss : MonoBehaviour
{
	[SerializeField] private float radiusExplosion;
	[SerializeField] private float explosionSpeed;
	[SerializeField] private float damage;

    private void OnEnable()
    {
		Explode();
    }

    private void Explode()
	{
		StartCoroutine(IWaitForExplode());
		
		IEnumerator IWaitForExplode()
		{
			yield return new WaitForSeconds(explosionSpeed);

			if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) <= radiusExplosion)
			{
				PlayerController.Instance.TakeDamage(damage);
				gameObject.SetActive(false);
			}
		}
	}
}