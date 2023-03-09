using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour,IEffectable
{
    public enum State
    {
        Chase,
        Neutral,
        Attack,
        Dying,
    }

    public State state;
    
    private GameObject player;
    [SerializeField] 
    private EnemyData data;
    private StatusEffectData _effectData;

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
    [SerializeField] 
    private Transform[] paths;

    private int _destPoint = 0;
    
    private bool _focusPlayer = false;
    private bool _canAttack = true;
    
    private float healthpoint;

    [Serializable]
    private struct ShakingParams
    {
        public float elapsed;
        public float duration;
        public float magnitude;
    }

    [SerializeField]
    private ShakingParams shakingParams;

    private void Awake()
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
        if (state == State.Dying)
            return;
        
        player = GameObject.FindGameObjectWithTag("Player");
        _rend.material.color = Color.white;

        if (_focusPlayer)
            state = State.Chase;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (state == State.Dying)
            return;

        if(_effectData != null)
            HandleEffect();

        StateManagement();
    }

    public bool IsMoving()
    {
        return state == State.Chase;
    }
    
    private void FixedUpdate()
    {
        if (state == State.Dying)
            return;
        
        AreaDetection();
    }

    public State GetState()
    {
        return state;
    }

    #region State
    private void StateManagement()
    {
        // Change the state of the enemy and call the corresponding function or colorize the enemy
        switch (state)
        {
            case State.Chase:
                _agent.SetDestination(player.transform.position);
                stateRenderer.material.color = Color.yellow;
                stateSystem.gameObject.SetActive(true);
                break;
            case State.Attack:
                stateRenderer.material.color = Color.red;
                stateSystem.gameObject.SetActive(true);
                Attack();
                break;
            case State.Neutral:
                stateSystem.gameObject.SetActive(false);
                Pathing();
                break;
            case State.Dying:
                visual.SetActive(false);
                KillEnemy();
                break;
            default:
                KillEnemy();
                break;
        }
    }

    private void AreaDetection()
    {
        // If the enemy is not focused on the player, it will detect the player if he is in the detection range
        if (!_focusPlayer)
        {
            Collider[] col = Physics.OverlapSphere(transform.position, data.GetRangeDetection(), 
                LayerMask.NameToLayer("Player"));

            foreach (Collider c in col)
            {
                if (c.gameObject.CompareTag("Player"))
                {
                    _focusPlayer = true;
                    state = State.Chase;
                }
            }
        }
        else
        {
            // Switch between chase and attack state depending on the distance between the enemy and the player 
            state = Vector3.Distance(transform.position, player.transform.position) <= data.GetAttackRange() ? State.Attack : State.Chase;
        }
    }
    
    #endregion
    
    #region NeutralState
    
    private void Pathing()
    {
        // If path is empty, do nothing
        if (paths.Length == 0)
            return;
        
        // If the enemy is close enough to the current point, go to the next point
        if (Vector3.Distance(transform.position, paths[_destPoint].position) < _agent.stoppingDistance)
            GoToNextPoint();
        
        // Set the destination of the enemy to the current point
        _agent.SetDestination(paths[_destPoint].position);
    }
    
    private void GoToNextPoint()
    {
        // If there is no more point, go back to the first point
        _destPoint = (_destPoint + 1) % paths.Length;
    }
    
    #endregion

    #region AttackState
    
    // ReSharper disable Unity.PerformanceAnalysis
    public void Attack()
    {
        if (_canAttack)
        {
            GameObject shot = Instantiate(bullet, gun.transform.position, Quaternion.identity);
            shot.GetComponent<EnemyBulletController>().SetDirection(player.transform);
            _canAttack = false;
            StartCoroutine(AttackTimer());
        }
    }
    
    #endregion
    
    #region TakeDamage

    public void TakeDamage(float damage = 1)
    {
        if (state == State.Neutral)
            state = State.Chase;

        ReduiceHealth(damage);
        StartCoroutine(ColorationFeedback());
    }

    private void ReduiceHealth(float damage)
    {
        damage = Mathf.Abs(damage);
        healthpoint -= damage;

        if (healthpoint <= 0)
        {
            state = State.Dying;
        }
    }

    public void KnockBack()
    {
        _rigidbody.constraints = RigidbodyConstraints.None;
        
        StopCoroutine(StoppingForce());
        
        Vector3 direction = transform.position - player.transform.position;
        direction.Normalize();
        
        _rigidbody.AddForce(direction * data.GetRecoilForce(), ForceMode.Impulse);
        StartCoroutine(StoppingForce());
    }

    public void ColorFeedback()
    {
        StartCoroutine(ColorationFeedback());
    }

    public void KillEnemy()
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
            ReduiceHealth(_effectData._DOTAmount);
        }
            
    }
    #endregion

    #region Effect

    public void DestroyEffect()
    {
        if (destructSystem)
            destructSystem.gameObject.SetActive(true);
    }

    public void Shake()
    {
        StartCoroutine(ShakeObject());
    }
    
    private IEnumerator ShakeObject()
    {
        float elapsed = shakingParams.elapsed;

        Vector3 originalPos = transform.position;
        
        while (elapsed < shakingParams.duration)
        {
            float x = Random.Range(-1f, 1f) * shakingParams.magnitude;
            float z = Random.Range(-1f, 1f) * shakingParams.magnitude;
            
            transform.position = new Vector3(originalPos.x + x, originalPos.y, originalPos.z + z);
            elapsed += Time.deltaTime;
            
            yield return null;
        }
        
        transform.position = originalPos;
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
