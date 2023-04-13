using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class EnemyController : MonoBehaviour
{
    protected GameObject player;
    
    [HideInInspector]
    public List<StatusEffectHandler> _effectHandlers;
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
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        player = PlayerController.Instance.gameObject;
        AddToEnemyManager();
        
        _rend.material.color = Color.white;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        HandleAllEffects();
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

    public virtual void TakeDamage(float damage = 1, bool isCritical = false)
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

    public void ApplyEffect(StatusEffectHandler handler)
    {
        _effectHandlers.Add(handler);
    }

    public void RemoveEffect(StatusEffectHandler handler)
    {
        _effectHandlers.Remove(handler);
    }

    private void HandleAllEffects()
    {
        foreach (StatusEffectHandler handler in _effectHandlers)
        {
            handler.HandleEffect();
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
