using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Kamilkaze : EnemyController
{
    [SerializeField] private KamilkazeData _data;
    [SerializeField] private NavMeshAgent _agent;

    public enum State
    {
        Neutral,
        Chase,
        Casting,
        Explose,
        Dying
    }

    public State state;

    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject physics;

    private Coroutine stateCoroutine;

    protected override void OnEnable()
    {
        base.OnEnable();

        Healthpoint = _data.Health;
        //_animator = GetComponentInChildren<Animator>();

        SetState(_data.FocusPlayerOnCD ? State.Chase : State.Neutral);
    }

    private void SetState(State newState)
    {
        stateCoroutine = null;
        state = newState;

        StateManagement();
    }

    private void StateManagement()
    {
        switch (state)
        {
            case State.Neutral:
                break;
            case State.Chase:
                Chase();
                break;
            case State.Casting:
                Casting();
                break;
            case State.Explose:
                Explode();
                break;
            case State.Dying:
                Dying();
                break;
            default:
                SetState(State.Dying); 
                break;
        }
    }

    public override bool IsMoving()
    {
        throw new System.NotImplementedException();
    }

    protected override void Chase()
    {
        _agent.stoppingDistance = _data.DistanceToPlayerForExplosion;
        stateCoroutine = StartCoroutine(IChase());

        IEnumerator IChase()
        {
            while (state == State.Chase)
            {
                if (Vector3.Distance(transform.position, Player.transform.position) < _agent.stoppingDistance)
                    _agent.SetDestination(Player.transform.position);
                else
                    SetState(State.Casting);

                yield return null;
            }
        }
    }

    private void Casting()
    {
        stateCoroutine = StartCoroutine(ICasting());

        IEnumerator ICasting()
        {
            yield return new WaitForSeconds(_data.DelaiForExplose);
            SetState(State.Explose);
        }
    }

    private void Explode()
    {
        if (Vector3.Distance(transform.position, Player.transform.position) <= _data.ExplosionRange)
            Player.GetComponent<PlayerController>().TakeDamage(_data.Damage);

        SetState(State.Dying);
    }

    public override void TakeDamage(float damage = 1, bool isCritical = false)
    {
        base.TakeDamage(damage, isCritical);

        if (Healthpoint <= 0)
            SetState(State.Dying);
    }

    protected override void Dying()
    {
        physics?.SetActive(false);
        StopAllCoroutines();

        _animator?.SetBool("isDead", true);

        StartCoroutine(IDeathAnim());

        IEnumerator IDeathAnim()
        {
            yield return new WaitForSeconds(2f);
            base.Dying();
        }
    }
}
