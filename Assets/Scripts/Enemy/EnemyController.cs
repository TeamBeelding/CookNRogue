using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public abstract class EnemyController : MonoBehaviour, IState, IEffectable
{
    protected GameObject player;
    
    [HideInInspector]
    public StatusEffectData _effectData;

    [SerializeField] 
    protected EnemyData data;
    private Renderer _rend;
    private MeshRenderer _meshRenderer;
    private NavMeshAgent _agent;
    private CapsuleCollider _collider;
    
    [SerializeField]
    private ParticleSystem m_destructSystem;
    [SerializeField]
    private ParticleSystem m_stateSystem;
    private Renderer stateRenderer;
    
    private IEnumerator colorCoroutine;
    
    [SerializeField]
    private GameObject m_gun;
    [SerializeField]
    private GameObject m_visual;
    [SerializeField]
    private GameObject m_bullet;

    protected bool _focusPlayer = false;
    private bool _canAttack = true;
    
    protected float healthpoint;

    protected void Awake()
    {
        _rend = GetComponentInChildren<Renderer>();
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _collider = GetComponent<CapsuleCollider>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = data.GetSpeed();
        _agent.stoppingDistance = data.GetAttackRange();
        _focusPlayer = data.GetFocusPlayer();
        healthpoint = data.GetHealth();

        stateRenderer = m_stateSystem.GetComponent<Renderer>();

        player = GameObject.FindGameObjectWithTag("Player");
        
        AddToEnemyManager();
    }

    // Start is called before the first frame update
    protected void Start()
    {
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
        _agent.SetDestination(player.transform.position);
        stateRenderer.material.color = Color.yellow;
        m_stateSystem.gameObject.SetActive(true);
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    protected void Attack()
    {
        stateRenderer.material.color = Color.red;
        m_stateSystem.gameObject.SetActive(true);
        
        if (_canAttack)
        {
            GameObject shot = Instantiate(m_bullet, m_gun.transform.position, Quaternion.identity);
            shot.GetComponent<EnemyBulletController>().SetDirection(player.transform);
            _canAttack = false;
            StartCoroutine(IAttackTimer());
        }
    }
    
    #endregion
    
    #region TakeDamage

    public virtual void TakeDamage(float damage = 1)
    {
        damage = Mathf.Abs(damage);
        healthpoint -= damage;
        
        StartCoroutine(IColorationFeedback());
    }

    protected void Dying()
    {
        EnemyManager.Instance.RemoveEnemyFromLevel(this);
        
        StopAllCoroutines();
        
        _collider.enabled = false;
        m_stateSystem.gameObject.SetActive(false);
        m_visual.SetActive(false);
        DestroyEffect();
        Destroy(gameObject, 1f);
    }
    
    // Color the enemy red for a short time to indicate that he has been hit
    private IEnumerator IColorationFeedback()
    {
        for (int i = 0; i < 5; i++)
        {
            _meshRenderer.enabled = false;
            yield return new WaitForSeconds(0.2f);
            _meshRenderer.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator IAttackTimer()
    {
        LerpColor(_rend, Color.red, data.GetAttackSpeed());
        yield return new WaitForSeconds(data.GetAttackSpeed());
        _canAttack = true;
        _rend.material.color = Color.white;
    }
    
    #endregion

    //Add to enemy manager
    private void AddToEnemyManager()
    {
        EnemyManager.Instance.AddEnemyToLevel(this);
    }

    #region StatusEffect

    private float _currentEffectTime = 0;
    private float _NextTickTime = 0;
    private ParticleSystem _part;

    public void ApplyEffect(StatusEffectData data)
    {
        
        _effectData = data;
        if(_effectData._effectpart != null)
            _part = Instantiate(_effectData._effectpart,transform);
    }

    public void RemoveEffect()
    {
        _currentEffectTime = 0;
        _NextTickTime = 0;
        _effectData = null;
        _agent.speed = data.GetSpeed();
        _agent.stoppingDistance = data.GetAttackRange();
        _focusPlayer = data.GetFocusPlayer();
        if (_part != null)
            Destroy(_part);
            
    }

    public void HandleEffect()
    {
        _currentEffectTime += Time.deltaTime;

        if (_currentEffectTime > _effectData._lifetime)
            RemoveEffect();

        if (_effectData == null)
            return;

        if(_effectData._DOTAmount != 0 && _currentEffectTime > _NextTickTime)
        {
            _NextTickTime += _currentEffectTime;
            // ReduiceHealth(_effectData._DOTAmount);
        }
            
    }
    #endregion

    #region Effect

    private void DestroyEffect()
    {
        if (m_destructSystem)
            m_destructSystem.gameObject.SetActive(true);
    }

    // Lerp color of the enemy
    private void LerpColor(Renderer r, Color color, float t)
    {
        colorCoroutine = ILerpColorCoroutine(r, color, t);
        StopCoroutine(colorCoroutine);
        StartCoroutine(colorCoroutine);
    }
    
    private IEnumerator ILerpColorCoroutine(Renderer r, Color color, float t)
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
