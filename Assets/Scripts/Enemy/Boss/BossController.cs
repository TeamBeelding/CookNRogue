using System.Collections;
using UnityEngine;

public class BossController : EnemyController
{
    private enum State
    {
        EnterRoom,
        Teleport,
        CastMissiles,
        ThrowMissiles,
        CastDash,
        Dash,
        Shockwave,
        Dying
    }

    [SerializeField] private State state;

    [SerializeField] private BossData data;

    [SerializeField] private GameObject visual;
    [SerializeField] private GameObject physics;

    private MissilesController missilesController;
    private ShockwaveController shockwaveController;

    private Coroutine stateCoroutine;
    private bool hitPlayerDuringDash = false;

    protected override void OnEnable()
    {
        Reset();
    }

    private void Reset()
    {
        missilesController = GetComponent<MissilesController>();
        shockwaveController = GetComponent<ShockwaveController>();
    }

    private void SetState(State newValue)
    {
        if (stateCoroutine != null)
            stateCoroutine = null;

        state = newValue;

        StateManagement();
    }

    private void StateManagement()
    {
        switch (state)
        {
            case State.EnterRoom:
                EnterRoom();
                break;
            case State.Teleport:
                Teleport();
                break;
            case State.CastMissiles:
                CastMissiles();
                break;
            case State.ThrowMissiles:
                ThrowMissiles();
                break;
            case State.CastDash:
                CastDash();
                break;
            case State.Dash:
                Dashing();
                break;
            case State.Shockwave:
                Shockwave();
                break;
            case State.Dying:
                Dying();
                break;
        }
    }

    private void EnterRoom()
    {
        stateCoroutine = StartCoroutine(IEnterRoom());

        IEnumerator IEnterRoom()
        {
            while (state == State.EnterRoom)
            {
                yield return new WaitForSeconds(2);
                SetState(State.Teleport);
            }
        }
    }

    private void Teleport()
    {
        stateCoroutine = StartCoroutine(ITeleport());

        IEnumerator ITeleport()
        {
            while (state == State.Teleport)
            {
                yield return null;
                SetState(State.CastMissiles);
            }
        }
    }

    private void CastMissiles()
    {
        stateCoroutine = StartCoroutine(ICastMissiles());

        IEnumerator ICastMissiles()
        {
            while (state == State.CastMissiles)
            {
                yield return new WaitForSeconds(2f);

                SetState(State.ThrowMissiles);
            }
        }
    }

    private void ThrowMissiles()
    {
        // Todo : 
        // Start throwing missiles

        SetState(State.Dash);
    }

    private void CastDash()
    {
        stateCoroutine = StartCoroutine(ICastDashing());

        IEnumerator ICastDashing()
        {
            while (state == State.Dash)
            {
                yield return new WaitForSeconds(2);
                SetState(State.Dash);
            }
        }
    }

    private void Dashing()
    {
        stateCoroutine = StartCoroutine(IDashing());

        IEnumerator IDashing()
        {
            while (state == State.Dash)
            {
                yield return null;

                if (hitPlayerDuringDash)
                {

                }
                else
                    SetState(State.Shockwave);
            }
        }
    }

    private void Shockwave()
    {
        stateCoroutine = StartCoroutine(IShockwave());

        IEnumerator IShockwave()
        {
            while (state == State.Shockwave)
            {
                yield return null;
                SetState(State.Teleport);
            }
        }
    }

    protected override void Dying()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    public override bool IsMoving()
    {
        throw new System.NotImplementedException();
    }
}
