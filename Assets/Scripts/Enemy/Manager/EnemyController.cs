using System.Collections;
using System.Collections.Generic;
using Enemy;
using TMPro;
using Tutoriel;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public abstract class EnemyController : MonoBehaviour
{
    protected GameObject Player;
    
    [HideInInspector]
    public List<StatusEffectHandler> _effectHandlers;
    private Renderer _rend;
    private MeshRenderer _meshRenderer;
    [SerializeField] protected CapsuleCollider _collider;
    
    private IEnumerator _colorCoroutine;
    
    protected bool FocusPlayer = false;
    private bool _canAttack = true;
    protected bool _canAttackAnim = true;

    protected float Healthpoint;

    [SerializeField]
    protected GameObject _damageUI;

    [SerializeField]
    protected GameObject explosion;

    [SerializeField] ParticleSystem _spawnFX;

    [SerializeField] 
    private TutorialManager tutorial;

    protected WaveManager waveManager;

    protected bool hasAskForSlow = false;

    [Header("Sound")]
    [SerializeField]
    private AK.Wwise.Event _Play_SFX_Ennemy_Spawn;

    protected virtual void Awake()
    {
        _rend = GetComponentInChildren<Renderer>();
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _collider = GetComponent<CapsuleCollider>();
        
        if (_collider == null)
            _collider = GetComponentInChildren<CapsuleCollider>();
    }

    protected virtual void Start()
    {
        //_rend.material.color = Color.white;

        if (_spawnFX)
            _spawnFX.Play();

        if (_Play_SFX_Ennemy_Spawn!=null)
            _Play_SFX_Ennemy_Spawn.Post(gameObject);
    }

    protected virtual void OnEnable()
    {
        Player = PlayerController.Instance.gameObject;
        waveManager = GameObject.FindGameObjectWithTag("WaveManager").GetComponent<WaveManager>();

        hasAskForSlow = false;

        AddToEnemyManager();
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
    
    // ReSharper disable Unity.PerformanceAnalysis
    protected virtual void Attack(UnityAction OnAction, float delay = 0.5f)
    {
        if (Player.GetComponent<PlayerController>().GetIsOnTutorial())
            return;
        
        if (_canAttack)
        {
            OnAction?.Invoke();
            _canAttack = false;
            _canAttackAnim = false;
            StartCoroutine(IAttackTimer(delay));
        }
        
        IEnumerator IAttackTimer(float delay = 0.5f)
        {
            yield return new WaitForSeconds(delay * 0.95f);
            _canAttackAnim = true;

            yield return new WaitForSeconds(delay * 0.05f);
            _canAttack = true;
        }
    }
    
    #endregion

    #region TakeDamage

    public virtual void TakeDamage(float damage = 1, bool isCritical = false)
    {
        damage = Mathf.Abs(damage);
        Healthpoint -= damage;

        //DAMAGE UI
        if (_damageUI)
        {
            GameObject UIDAMAGE = Instantiate(_damageUI, transform.position + (Vector3.up * 3) + GetCameraDirection() * 0.5f, Quaternion.identity);
            UIDAMAGE.GetComponentInChildren<TextMeshProUGUI>().text = damage.ToString();
            Destroy(UIDAMAGE, 1);
        }

        if (Healthpoint > 0)
        {
            TakeDamageEffect();
        }
    }
    
    protected virtual void TakeDamageEffect()
    {
        
    }

    protected virtual void Dying()
    {
        if (!hasAskForSlow)
            waveManager.SlowMotion();

        DestroyEffect();
        PoolManager.Instance.DesinstantiateFromPool(gameObject);
    }

    #endregion

    private void AddToEnemyManager()
    {
        EnemyManager.Instance.AddEnemyToLevel(this);
    }
    
    protected virtual void OnDisable()
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
        if (explosion == null)
            return;
        
        Instantiate(explosion, transform.position, Quaternion.identity);
        
        if (_collider)
            _collider.enabled = false;
    }

    // Lerp color of the enemy
    private void LerpColor(Renderer r, Color color, float t)
    {
        _colorCoroutine = ILerpColorCoroutine(r, color, t);
        StopCoroutine(_colorCoroutine);
        StartCoroutine(_colorCoroutine);
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
    
    /// <summary>
    /// Stop all coroutines when the object is destroyed
    /// </summary>
    ~EnemyController()
    {
        StopAllCoroutines();
    }

    Vector3 GetCameraDirection()
    {
        Vector3 dir = Camera.main.transform.position - transform.position;
        return dir;
    }
}
