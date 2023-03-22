
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ChargingEnemy : EnemyController
{
    private Coroutine castingCoroutine;
    private Coroutine waitingCoroutine;
    private RaycastHit _hit;

    private bool isCharging = false;
    
    private Vector3 direction;
    private Rigidbody rigidbody;

    [SerializeField] private GameObject _redLine;
    //[SerializeField] private Color redColor;
    //[SerializeField] private Color redColorAtEnd;
    
    [SerializeField]
    private EnemyDashingData _data;
        
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
    
    private void Casting()
    {
        isCharging = false;
        rigidbody.velocity = Vector3.zero;
        castingCoroutine = StartCoroutine(ICasting());
        transform.LookAt(player.transform);

    }
    
    private void WaitingAnotherDash()
    {
        isCharging = false;
        rigidbody.velocity = Vector3.zero;
        
        HideRedLine();
        
        if (castingCoroutine != null)
            StopCoroutine(castingCoroutine);
        
        waitingCoroutine = StartCoroutine(IWaiting());
    }
    
    private new void Dying()
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
        yield return new WaitForSeconds(_data.GetTimeBeforeShowingRedLine());
        
        HideRedLine();
        
        yield return new WaitForSeconds(_data.GetTimeBeforeLerpRedLine());
        
        ShowRedLine();
        
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

    private void ShowRedLine()
    {
        _redLine.GetComponent<MeshRenderer>().material.SetFloat("_Alpha", 0.3f);
        
        if (Physics.Raycast(transform.position, Vector3.forward, out _hit, 6)) 
        {
            //_hit.distance;
        }
    }

    /// <summary>
    /// Reset line renderer color and hide it
    /// </summary>
    private void HideRedLine()
    {
        // Todo : Reset red line color
        _redLine.GetComponent<MeshRenderer>().material.SetFloat("_Alpha", 0.85f);
    }
    

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            rigidbody.velocity = Vector3.zero;
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
