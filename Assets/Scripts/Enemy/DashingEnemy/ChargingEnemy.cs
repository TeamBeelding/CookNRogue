using System.Collections;
using UnityEngine;

public class ChargingEnemy : EnemyController
{
    private Coroutine _castingCoroutine;
    private Coroutine _waitingCoroutine;
    private RaycastHit _hit;

    private bool _isCharging = false;
    private bool _canShowingRedLine = false;
    
    private Vector3 _direction;
    private Rigidbody _rigidbody;

    [SerializeField] private GameObject _redLine;
    [SerializeField] private EnemyDashingData _data;
    
    private Material _redLineMaterial;
    private bool _isRedLineFullVisible = false;

    [SerializeField]
    private GameObject visual;
    
    public enum State
    {
        Casting,
        Waiting,
        Dashing,
        Dying
    }

    public State state;
        
    private void Awake()
    {
        base.Awake();
        Healthpoint = _data.GetHealth();
    }
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
        _rigidbody = GetComponent<Rigidbody>();
        state = State.Casting;
        _redLineMaterial = _redLine.GetComponent<Renderer>().material;
    }

    private void FixedUpdate()
    {
        IStateManagement();
    }

    public override bool IsMoving()
    {
        return false;
    }

    public State GetState()
    {
        return state;
    }
    
    private void SetState(State value)
    {
        state = value;
    }
    
    private void IStateManagement()
    {
        switch (state)
        {
            case State.Casting:
                Casting();
                break;
            case State.Waiting:
                WaitingAnotherDash();
                break;
            case State.Dashing:
                Dashing();
                break;
            case State.Dying:
                Dying();
                break;
            default:
                Dying();
                break;
        }
    }

    /// <summary>
    /// Dashing State :
    /// Stop casting and dash to player
    /// </summary>
    private void Dashing()
    {
        _isCharging = true;
        StopCoroutine(_castingCoroutine);
        
        ChargingToPlayer();
    }
    
    private void StopMoving()
    {
        _direction = Vector3.zero;

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        StopCoroutine(ICanShowingRedLine());
        _canShowingRedLine = false;
    }

    /// <summary>
    /// Casting State :
    /// Preparing to dash and Look at player
    /// </summary>
    private void Casting()
    {
        _isCharging = false;
        _castingCoroutine = StartCoroutine(ICasting());
        visual.transform.LookAt(new Vector3(Player.transform.position.x, visual.transform.position.y, Player.transform.position.z));
    }
    
    /// <summary>
    /// Wait some time before casting again
    /// </summary>
    private void WaitingAnotherDash()
    {
        StopMoving();

        _isCharging = false;
        _isRedLineFullVisible = false;
        _canShowingRedLine = false;

        if (_castingCoroutine != null)
            StopCoroutine(_castingCoroutine);
        
        _waitingCoroutine = StartCoroutine(IWaiting());
    }
    
    /// <summary>
    /// Dying Test
    /// </summary>
    protected override void Dying()
    {
        base.Dying();

        StopCasting();
    }
    
    /// <summary>
    /// Enemy Take Damage
    /// </summary>
    /// <param name="damage"></param>
    public override void TakeDamage(float damage = 1, bool isCritical = false)
    {
        base.TakeDamage(damage, isCritical);

        StopCasting();
        SetState(State.Waiting);
    }

    /// <summary>
    /// Stop casting coroutine
    /// </summary>
    private void StopCasting()
    {
        if (_castingCoroutine != null)
            StopCoroutine(_castingCoroutine);
    }
    
    /// <summary>
    /// Return the direction to player
    /// </summary>
    /// <returns></returns>
    private Vector3 GetPlayerDirection()
    {
        _isCharging = true;
        Vector3 direction = (Player.transform.position - transform.position).normalized;
        direction.y = 0;
        
        return direction;
    }
    
    private void ChargingToPlayer()
    {
        transform.position += _direction * (_data.GetSpeed() * Time.deltaTime);
    }

    private IEnumerator ICasting()
    {
        yield return StartCoroutine(ICanShowingRedLine());
        
        ShowLightRedLine();
        
        yield return new WaitForSeconds(_data.GetTimeBeforeLerpRedLine());
        
        ShowFullyRedLine();
        
        yield return new WaitForSeconds(_data.GetRemainingForDash());

        if (!_isCharging)
            _direction = GetPlayerDirection();
        
        SetState(State.Dashing);
    }
    
    private IEnumerator ICanShowingRedLine()
    {
        if (_canShowingRedLine)
            yield break;
        
        HideRedLine();
        yield return new WaitForSeconds(_data.GetTimeBeforeShowingRedLine());
    }
    
    /// <summary>
    /// Time before the enemy can dash again
    /// </summary>
    private IEnumerator IWaiting()
    {
        yield return new WaitForSeconds(_data.GetTimeWaitingDash());
        
        SetState(State.Casting);
    }

    /// <summary>
    /// Make Red line more visible
    /// </summary>
    private void ShowFullyRedLine()
    {
        _isRedLineFullVisible = true;
        _redLineMaterial.SetFloat("_Alpha", 0.3f);
    }

    /// <summary>
    /// Reset line renderer color and hide it
    /// </summary>
    private void ShowLightRedLine()
    {
        if (_isRedLineFullVisible) return;
        
        _canShowingRedLine = true;
        _redLineMaterial.SetFloat("_Alpha", 0.85f);
    }
    
    /// <summary>
    /// Put the red line to full alpha (invisible)
    /// </summary>
    private void HideRedLine()
    {
        _redLineMaterial.SetFloat("_Alpha", 1f);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Obstruction"))
        {
            StopCasting();
            SetState(State.Waiting);
        }
        
        if (other.gameObject.CompareTag("Player"))
        {
            StopCasting();
            SetState(State.Waiting);
            other.gameObject.GetComponent<PlayerController>().TakeDamage(_data.GetDamage());
        }
    }
    
    /// <summary>
    /// Stop all coroutines when the object is destroyed
    /// </summary>
    ~ChargingEnemy()
    {
        StopAllCoroutines();
    }
}
