using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingEnemy : EnemyController
{
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public override bool IsMoving()
    {
        return false;
    }

    public enum State
    {
        Dashing,
        Casting,
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
            case State.Dashing:
                break;
            case State.Casting:
                break;
            case State.Dying:
                Dying();
                break;
        }
    }
    
    private void Dash()
    {
        
    }
    
    private void Cast()
    {
        
    }
    
    private new void Dying()
    {
        base.Dying();
    }
}
