using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public abstract class EnemyController : MonoBehaviour, IState, IEffectable
{
    protected GameObject player;
    
    [HideInInspector]
    public StatusEffectData _effectData;
    private Renderer _rend;
    private MeshRenderer _meshRenderer;
    private CapsuleCollider _collider;
    
    private IEnumerator colorCoroutine;
    
    protected bool _focusPlayer = false;
    private bool _canAttack = true;
    
    protected float healthpoint;
    
    [SerializeField]
    protected GameObject explosion;

    protected virtual void Awake()
    {
        _rend = GetComponentInChildren<Renderer>();
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _collider = GetComponent<CapsuleCollider>();

        player = PlayerController.Instance.gameObject;
        
        AddToEnemyManager();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _rend.material.color = Color.white;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (_effectData != null)
            HandleEffect();
    }

    public abstract bool IsMoving();

    #region AttackState

    protected virtual void Chase()
    {
        
    }
    
    protected virtual void Attack(UnityAction OnAction, float delay = 0.5f)
    {
        if (_canAttack)
        {
            OnAction?.Invoke();
            _canAttack = false;
            StartCoroutine(IAttackTimer(delay));
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

    protected virtual void Dying()
    {
        DestroyEffect();
        Destroy(gameObject);
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

    private IEnumerator IAttackTimer(float delay = 0.5f)
    {
        yield return new WaitForSeconds(delay);
        _canAttack = true;
        _rend.material.color = Color.white;
    }
    
    #endregion

    //Add to enemy manager
    private void AddToEnemyManager()
    {
        EnemyManager.Instance.AddEnemyToLevel(this);
    }
    
    private void OnDestroy()
    {
        StopAllCoroutines();
        EnemyManager.Instance.RemoveEnemyFromLevel(this);
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
            TakeDamage(_effectData._DOTAmount);
        }
            
    }
    #endregion

    #region Effect

    private void DestroyEffect()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        _collider.enabled = false;
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
    
    #if UNITY_EDITOR

    protected virtual void OnDrawGizmosSelected()
    {
        
    }

    #endif

    #endregion
}
