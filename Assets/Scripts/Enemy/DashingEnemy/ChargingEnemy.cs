using System.Collections;
using UnityEngine;

public class ChargingEnemy : EnemyController
{
    private Coroutine castingCoroutine;
    private Coroutine waitingCoroutine;
    private RaycastHit _hit;

    private bool isCharging = false;
    private bool canShowingRedLine = false;
    
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

    /// <summary>
    /// Dashing State :
    /// Stop casting and dash to player
    /// </summary>
    private void Dashing()
    {
        isCharging = true;
        StopCoroutine(castingCoroutine);
        
        ChargingToPlayer();
    }
    
    private void StopMoving()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        StopCoroutine(ICanShowingRedLine());
        canShowingRedLine = false;
        
        direction = Vector3.zero;
    }

    /// <summary>
    /// Casting State :
    /// Preparing to dash and Look at player
    /// </summary>
    private void Casting()
    {
        isCharging = false;
        castingCoroutine = StartCoroutine(ICasting());
        transform.LookAt(player.transform);
    }
    
    /// <summary>
    /// Wait some time before casting again
    /// </summary>
    private void WaitingAnotherDash()
    {
        isCharging = false;
        _isRedLineFullVisible = false;
        canShowingRedLine = false;
        
        StopMoving();
        
        if (castingCoroutine != null)
            StopCoroutine(castingCoroutine);
        
        waitingCoroutine = StartCoroutine(IWaiting());
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
        base.TakeDamage();

        StopCasting();
        SetState(State.Casting);
        
        if (healthpoint <= 0)
        {
            state = State.Dying;
        }
    }

    /// <summary>
    /// Stop casting coroutine
    /// </summary>
    private void StopCasting()
    {
        if (castingCoroutine != null)
            StopCoroutine(castingCoroutine);
    }
    
    
    /// <summary>
    /// Return the direction to player
    /// </summary>
    /// <returns></returns>
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
        // yield return new WaitForSeconds(_data.GetTimeBeforeShowingRedLine());
        //
        // ShowLightRedLine();

        yield return StartCoroutine(ICanShowingRedLine());
        
        ShowLightRedLine();
        
        yield return new WaitForSeconds(_data.GetTimeBeforeLerpRedLine());
        
        ShowFullyRedLine();
        
        yield return new WaitForSeconds(_data.GetRemainingForDash());

        if (!isCharging)
            direction = GetPlayerDirection();
        
        SetState(State.Dashing);
    }
    
    private IEnumerator ICanShowingRedLine()
    {
        if (canShowingRedLine)
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
        
        canShowingRedLine = true;
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
