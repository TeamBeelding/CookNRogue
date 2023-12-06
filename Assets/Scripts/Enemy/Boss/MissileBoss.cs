using System.Collections;
using UnityEngine;

public class MissileBoss : MonoBehaviour
{

	private float _damage;
	public float damage { get { return _damage; } }

	[SerializeField] Color _missileColor;
	[SerializeField] MissileDecal _decal;
	[SerializeField] MissileBehaviour _missileBehaviour;
    public void Init(int damage, Vector3 target, Vector3 BossPosition)
    {
		_damage = damage;
		transform.position = target;
        _missileBehaviour.gameObject.SetActive(true);
        _decal.gameObject.SetActive(true);

        _decal.Init(_missileColor);
		_missileBehaviour.Init(_decal.gameObject,this, target, BossPosition);
    }



	/*
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
	*/
}