using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingEnemy : EnemyController
{
    private Coroutine castingCoroutine;
    private bool isCharging = false;
    
    private Vector3 direction;
    private Rigidbody rigidbody;
    
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

    // Update is called once per frame
    private void Update()
    {
        
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
        ChargingToPlayer();
    }
    
    private void Casting()
    {
        isCharging = false;
        castingCoroutine = StartCoroutine(ICasting());
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
        yield return new WaitForSeconds(2f);
        
        direction = GetPlayerDirection();
        SetState(State.Dashing);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            rigidbody.velocity = Vector3.zero;
            StopCasting();
            SetState(State.Casting);
        }
        
        if (other.gameObject.CompareTag("Player"))
        {
            rigidbody.velocity = Vector3.zero;
            StopCasting();
            other.gameObject.GetComponent<PlayerController>().TakeDamage(1);
            SetState(State.Casting);
        }
    }
}
