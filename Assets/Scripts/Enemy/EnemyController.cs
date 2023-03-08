using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public abstract class EnemyController : MonoBehaviour, IState
{
    protected PlayerController Player;
    [SerializeField] 
    protected EnemyData data;

    private Renderer _rend;
    private Rigidbody _rigidbody;
    private MeshRenderer _meshRenderer;
    private NavMeshAgent _agent;
    private CapsuleCollider _collider;
    
    [SerializeField]
    private ParticleSystem destructSystem;
    [SerializeField]
    private ParticleSystem stateSystem;
    private Renderer stateRenderer;
    
    private IEnumerator colorCoroutine;
    
    [SerializeField]
    private GameObject gun;
    [SerializeField]
    private GameObject visual;
    [SerializeField]
    private GameObject bullet;
    // [SerializeField] 
    // private Transform[] paths;

    // private int _destPoint = 0;
    
    protected bool _focusPlayer = false;
    private bool _canAttack = true;
    
    private float healthpoint;

    // [Serializable]
    // private struct ShakingParams
    // {
    //     public float elapsed;
    //     public float duration;
    //     public float magnitude;
    // }

    // [SerializeField]
    // private ShakingParams shakingParams;

    protected void Awake()
    {
        _rend = GetComponentInChildren<Renderer>();
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _collider = GetComponent<CapsuleCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = data.GetSpeed();
        _agent.stoppingDistance = data.GetAttackRange();
        _focusPlayer = data.GetFocusPlayer();
        healthpoint = data.GetHealth();

        stateRenderer = stateSystem.GetComponent<Renderer>();

        AddToEnemyManager();
    }

    // Start is called before the first frame update
    protected void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        
        _rend.material.color = Color.white;
    }

    // Update is called once per frame
    protected void Update()
    {
        // if (state == State.Dying)
        //     return;
        
        // IStateManagement();
    }

    public abstract bool IsMoving();

    #region AttackState

    protected void Chase()
    {
        _agent.SetDestination(Player.transform.position);
        stateRenderer.material.color = Color.yellow;
        stateSystem.gameObject.SetActive(true);
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    protected void Attack()
    {
        stateRenderer.material.color = Color.red;
        stateSystem.gameObject.SetActive(true);
        
        if (_canAttack)
        {
            GameObject shot = Instantiate(bullet, gun.transform.position, Quaternion.identity);
            shot.GetComponent<EnemyBulletController>().SetDirection(Player.transform);
            _canAttack = false;
            StartCoroutine(AttackTimer());
        }
    }
    
    #endregion
    
    #region TakeDamage

    public void TakeDamage(float damage = 1)
    {
        ReduiceHealth(damage);
        StartCoroutine(ColorationFeedback());
    }

    private void ReduiceHealth(float damage)
    {
        damage = Mathf.Abs(damage);
        healthpoint -= damage;

        // if (healthpoint <= 0)
        // {
        //     state = State.Dying;
        // }
    }

    public void KnockBack()
    {
        _rigidbody.constraints = RigidbodyConstraints.None;
        
        StopCoroutine(StoppingForce());
        
        Vector3 direction = transform.position - Player.transform.position;
        direction.Normalize();
        
        _rigidbody.AddForce(direction * data.GetRecoilForce(), ForceMode.Impulse);
        StartCoroutine(StoppingForce());
    }

    public void ColorFeedback()
    {
        StartCoroutine(ColorationFeedback());
    }

    protected void Dying()
    {
        EnemyManager.Instance.RemoveEnemyFromLevel(this);
        
        StopAllCoroutines();
        
        _collider.enabled = false;
        stateSystem.gameObject.SetActive(false);
        visual.SetActive(false);
        DestroyEffect();
        Destroy(gameObject, 1f);
    }
    
    // Color the enemy red for a short time to indicate that he has been hit
    private IEnumerator ColorationFeedback()
    {
        for (int i = 0; i < 5; i++)
        {
            _meshRenderer.enabled = false;
            yield return new WaitForSeconds(0.2f);
            _meshRenderer.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator AttackTimer()
    {
        LerpColor(_rend, Color.red, data.GetAttackSpeed());
        yield return new WaitForSeconds(data.GetAttackSpeed());
        _canAttack = true;
        _rend.material.color = Color.white;
    }
    
    #endregion

    // Stop the enemy from moving after receiving a recoil force
    private IEnumerator StoppingForce()
    {
        yield return new WaitForSeconds(.5f);
        _rigidbody.velocity = Vector3.zero;
    }

    //Add to enemy manager
    private void AddToEnemyManager()
    {
        EnemyManager.Instance.AddEnemyToLevel(this);
    }

    #region Effect

    public void DestroyEffect()
    {
        if (destructSystem)
            destructSystem.gameObject.SetActive(true);
    }

    // Lerp color of the enemy
    private void LerpColor(Renderer r, Color color, float t)
    {
        colorCoroutine = LerpColorCoroutine(r, color, t);
        StopCoroutine(colorCoroutine);
        StartCoroutine(colorCoroutine);
    }
    
    private IEnumerator LerpColorCoroutine(Renderer r, Color color, float t)
    {
        Color current = r.material.color;
        float elapsed = 0.0f;
        
        while (elapsed < t)
        {
            r.material.color = Color.Lerp(current, color, elapsed / t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        r.material.color = color;
    }
    
    #endregion

    #region Guizmos
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, data.GetRangeDetection());
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.GetAttackRange());
    }
    
    #endregion
}
