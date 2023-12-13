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

    //[SerializeField] private Animator _animator;
    [SerializeField] private GameObject physics;
    [SerializeField] private GameObject visual;
    [SerializeField] private GameObject effect;

    private Coroutine stateCoroutine;

    protected override void OnEnable()
    {
        base.OnEnable();

        _agent = GetComponent<NavMeshAgent>();

        _agent.speed = _data.Speed;
        Healthpoint = _data.Health;

        //_animator = GetComponentInChildren<Animator>();
        _agent.stoppingDistance = _data.DistanceToPlayerForExplosion;

        visual?.SetActive(true);
        physics?.SetActive(true);
        effect?.SetActive(false);

        SetState(_data.FocusPlayerOnCD ? State.Chase : State.Neutral);
    }

    private void SetState(State newState)
    {
        if (stateCoroutine != null)
            stateCoroutine = null;
        
        state = newState;

        StateManagement();
    }

    private void StateManagement()
    {
        switch (state)
        {
            case State.Neutral:
                Neutral();
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
                Dying();
                break;
        }
    }

    public override bool IsMoving()
    {
        throw new System.NotImplementedException();
    }

    private void Neutral()
    {
        stateCoroutine = StartCoroutine(ICheckingPlayerPos());

        IEnumerator ICheckingPlayerPos()
        {
            while (state == State.Neutral)
            {
                if (Vector3.Distance(transform.position, Player.transform.position) < _data.FocusRange)
                    SetState(State.Chase);

                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    protected override void Chase()
    {
        _agent.stoppingDistance = _data.DistanceToPlayerForExplosion;
        stateCoroutine = StartCoroutine(IChase());

        IEnumerator IChase()
        {
            while (state == State.Chase)
            {
                if (Vector3.Distance(transform.position, Player.transform.position) > _agent.stoppingDistance)
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
        effect?.SetActive(true);

        visual?.SetActive(false);
        physics?.SetActive(false);

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
        //_animator?.SetBool("isDead", true);

        _agent.speed = 0;

        visual?.SetActive(false);
        physics?.SetActive(false);

        effect?.SetActive(true);

        stateCoroutine = StartCoroutine(IDeathAnim());

        IEnumerator IDeathAnim()
        {
            yield return new WaitForSeconds(0.2f);
            base.Dying();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _data.FocusRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _data.ExplosionRange);
    }
#endif
}
