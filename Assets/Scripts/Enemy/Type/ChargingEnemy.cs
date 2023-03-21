using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingEnemy : EnemyController
{
    private Coroutine castingCoroutine;
    private Coroutine waitingCoroutine;
    
    private bool isCharging = false;
    
    private Vector3 direction;
    private Rigidbody rigidbody;

    [SerializeField] private GameObject _redLine;
    [SerializeField] private Color redColor;
    [SerializeField] private Color redColorAtEnd;
    
    private void Awake()
    {
        base.Awake();
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
        ChargingToPlayer();
    }
    
    private void Casting()
    {
        isCharging = false;
        rigidbody.velocity = Vector3.zero;
        castingCoroutine = StartCoroutine(ICasting());
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
        transform.position += direction * (data.GetSpeed() * Time.deltaTime);
    }
    
    private IEnumerator ICasting()
    {
        yield return new WaitForSeconds(0.5f);
        
        ShowRedLine();
        
        yield return new WaitForSeconds(2f);
        
        if (!isCharging)
            direction = GetPlayerDirection();
        
        SetState(State.Dashing);
    }
    
    private IEnumerator IWaiting()
    {
        yield return new WaitForSeconds(3f);
        
        SetState(State.Casting);
    }

    private void ShowRedLine()
    {
        // Todo : Enable red line game object
    }
    
    private void HideRedLine()
    {
        // Todo : Reset red line 
        // Todo: Disable red line game object
    }
    
    private void LerpRedLine()
    {
        // Todo : Interpolate red line
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
            other.gameObject.GetComponent<PlayerController>().TakeDamage(1);
            SetState(State.Waiting);
        }
    }
}
