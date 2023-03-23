using System.Collections;
using UnityEngine;

public class ChargingEnemy : EnemyController
{
    private Coroutine castingCoroutine;
    private Coroutine waitingCoroutine;
    private RaycastHit _hit;

    private bool isCharging = false;
    
    private Vector3 direction;
    private Rigidbody rigidbody;

    [SerializeField] private GameObject _redLine;
    [SerializeField] private EnemyDashingData _data;
    
    private Material _redLineMaterial;
    private bool _isRedLineFullVisible = false;
        
    private void Awake()
    {
        base.Awake();
        healthpoint = _data.GetHealth();
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        
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

    public enum State
    {
        Casting,
        Waiting,
        Dashing,
        Dying
    }

    public State state;

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

    private void Dashing()
    {
        isCharging = true;
        StopCoroutine(castingCoroutine);
        
        ChargingToPlayer();
    }
    
    // function make ia stop moving
    
    private void StopMoving()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        
        direction = Vector3.zero;
    }

    private void Casting()
    {
        isCharging = false;
        castingCoroutine = StartCoroutine(ICasting());
        transform.LookAt(player.transform);

    }
    
    private void WaitingAnotherDash()
    {
        isCharging = false;
        _isRedLineFullVisible = false;
        
        StopMoving();
        ShowLightRedLine();
        
        if (castingCoroutine != null)
            StopCoroutine(castingCoroutine);
        
        waitingCoroutine = StartCoroutine(IWaiting());
    }
    
    protected override void Dying()
    {
        base.Dying();

        StopCasting();
    }
    
    public override void TakeDamage(float damage = 1)
    {
        base.TakeDamage();

        StopCasting();
        SetState(State.Casting);
        
        if (healthpoint <= 0)
        {
            state = State.Dying;
        }
    }

    private void StopCasting()
    {
        if (castingCoroutine != null)
            StopCoroutine(castingCoroutine);
    }
    
    private Vector3 GetPlayerDirection()
    {
        isCharging = true;
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0;
        
        return direction;
    }
    
    private void ChargingToPlayer()
    {
        transform.position += direction * (_data.GetSpeed() * Time.deltaTime);
    }

    private IEnumerator ICasting()
    {
        // HideRedLine();
        
        yield return new WaitForSeconds(_data.GetTimeBeforeShowingRedLine());
        
        ShowLightRedLine();
        
        yield return new WaitForSeconds(_data.GetTimeBeforeLerpRedLine());
        
        ShowFullyRedLine();
        
        yield return new WaitForSeconds(_data.GetRemainingForDash());

        if (!isCharging)
            direction = GetPlayerDirection();
        
        SetState(State.Dashing);
    }
    
    /// <summary>
    /// Time before the enemy can dash again
    /// </summary>
    private IEnumerator IWaiting()
    {
        yield return new WaitForSeconds(_data.GetTimeWaitingDash());
        
        SetState(State.Casting);
    }

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
        
        _redLineMaterial.SetFloat("_Alpha", 0.85f);
    }
    
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
            other.gameObject.GetComponent<PlayerController>().TakeDamage(_data.GetDamage());
            SetState(State.Waiting);
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
