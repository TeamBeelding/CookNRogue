using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingEnemy : EnemyController
{
    private Coroutine castingCoroutine;
    private bool isCharging = false;
    
    // Start is called before the first frame update
    private void Start()
    {
        if (_focusPlayer)
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
    
    public void SetState(State value)
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
        StartCasting();
    }
    
    private new void Dying()
    {
        base.Dying();
    }
    
    public override void TakeDamage(float damage = 1)
    {
        base.TakeDamage(damage);
        StartCasting();
    }
    
    private void StartCasting()
    {
        if (castingCoroutine != null)
            StopCoroutine(castingCoroutine);
        
        castingCoroutine = StartCoroutine(ICasting());
    }
    
    private Vector3 GetPlayerDirection()
    {
        if (isCharging)
            return Vector3.zero;
        
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0;

        return direction;
    }
    
    private void ChargingToPlayer()
    {
        transform.position += GetPlayerDirection() * (5 * Time.deltaTime);
    }
    
    private IEnumerator ICasting()
    {
        yield return new WaitForSeconds(2f);
        state = State.Dashing;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Enemy"))
            state = State.Casting;
        
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(1);
            StartCasting();
        }
    }
}
