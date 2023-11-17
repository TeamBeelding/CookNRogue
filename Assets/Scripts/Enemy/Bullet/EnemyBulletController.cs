using System.Collections;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    [SerializeField] private EnemyBulletData _data;

    private bool isDirectionSet = false;
    private Vector3 _direction;
    private float _damage;

    private void Awake()
    {
        _damage = _data.GetDamage();
    }

    private void OnEnable()
    {
        //_damage = _data.GetDamage();
        StartCoroutine(IDesinstantiate());
    }

    private void OnDisable()
    {
        //StopCoroutine(IDesinstantiate());
    }

    // Update is called once per frame
    private void Update()
    {
        if (isDirectionSet)
            Move();
    }

    public void SetDirection(Transform dir)
    {
        _direction = new Vector3(dir.position.x - transform.position.x, 0, dir.position.z - transform.position.z);
        _direction.Normalize();
        isDirectionSet = true;
    }
    
    public void SetDirection(Vector3 dir)
    {
        _direction = new Vector3(dir.x - transform.position.x, 0, dir.z - transform.position.z);
        _direction.Normalize();
        isDirectionSet = true;
    }
    
    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    private void Move()
    {
        transform.position += _direction * (_data.GetSpeed() * Time.deltaTime);
    }

    private IEnumerator IDesinstantiate()
    {
        yield return new WaitForSeconds(_data.GetLifeTime());
        PoolManager.Instance.DesinstantiateFromPool(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>()?.TakeDamage(_damage);
            PoolManager.Instance.DesinstantiateFromPool(gameObject);
        }
        else if (!other.transform.parent.CompareTag("Enemy"))
            PoolManager.Instance.DesinstantiateFromPool(gameObject);
    }
}