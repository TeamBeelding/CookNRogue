using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent (typeof(MissilesController), typeof(ShockwaveController))]
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
    private Vector3 targetPosition;

    [SerializeField] Transform _teleportParticlesContainer;
    ParticleSystem[] _teleportParticles;
    protected override void OnEnable()
    {
        base.OnEnable();
        Reset();
    }

    protected override void Start()
    {
        base.Start();

        missilesController = GetComponentInChildren<MissilesController>();
        shockwaveController = GetComponentInChildren<ShockwaveController>();
        _teleportParticles = _teleportParticlesContainer.GetComponentsInChildren<ParticleSystem>();

        SetState(State.EnterRoom);
    }


    private void Reset()
    {
        visual?.SetActive(true);
        physics?.SetActive(true);

        missilesController = GetComponent<MissilesController>();
        shockwaveController = GetComponent<ShockwaveController>();
    }

    public BossData GetBossDataRef() => data;

    private void SetState(State newValue)
    {
        if (stateCoroutine != null)
            stateCoroutine = null;

        state = newValue;

        StateManagement();
    }

    public bool IsDashing() => state == State.Dash;

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
        if (!Player)
            Player = PlayerController.Instance.gameObject;

        stateCoroutine = StartCoroutine(IEnterRoom());

        IEnumerator IEnterRoom()
        {
            while (state == State.EnterRoom)
            {
                yield return new WaitForSeconds(data.GetStart);
                SetState(State.Teleport);
            }
        }
    }

    private void Teleport()
    {
        stateCoroutine = StartCoroutine(ITeleport());

        IEnumerator ITeleport()
        {
            if (_teleportParticlesContainer)
                _teleportParticlesContainer.transform.parent = null;

            while (state == State.Teleport)
            {
                foreach (var particle in _teleportParticles)
                {
                    particle.Play();
                    var VOLT = particle.velocityOverLifetime;
                    VOLT.x = new ParticleSystem.MinMaxCurve(0, 0);
                    VOLT.y = new ParticleSystem.MinMaxCurve(0, 0);
                    VOLT.z = new ParticleSystem.MinMaxCurve(0, 0);
                }
                    

                

                _teleportParticlesContainer.transform.position = Player.transform.position;
                Vector3 tpPos = Player.transform.position;

                yield return new WaitForSeconds(data.GetDelayBeforeTeleport);
                transform.position = tpPos;


                foreach (var particle in _teleportParticles)
                {
                    
                    var VOLT = particle.velocityOverLifetime;
                    VOLT.x = new ParticleSystem.MinMaxCurve(1, 5);
                    VOLT.y = new ParticleSystem.MinMaxCurve(1, 5);
                    VOLT.z = new ParticleSystem.MinMaxCurve(1, 5);
                    particle.Stop();
                }


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
                yield return new WaitForSeconds(data.GetCastMissilesDelay);

                SetState(State.ThrowMissiles);
            }
        }
    }

    private void ThrowMissiles()
    {
        missilesController?.LaunchMissiles();

        SetState(State.Dash);
    }

    private void CastDash()
    {
        stateCoroutine = StartCoroutine(ICastDashing());

        IEnumerator ICastDashing()
        {
            while (state == State.Dash)
            {
                yield return new WaitForSeconds(data.GetCastDashDelay);
                SetState(State.Dash);
            }
        }
    }

    private void Dashing()
    {
        stateCoroutine = StartCoroutine(IDashing());

        IEnumerator IDashing()
        {
            Vector3 direction = (GetTargetPosition() - transform.position).normalized;

            while (state == State.Dash)
            {
                transform.position += direction * (data.GetDashSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }

    public Vector3 GetTargetPosition()
    {
        targetPosition = GetLastPlayerPosition();
        targetPosition.y = 0;

        return targetPosition;

        Vector3 GetLastPlayerPosition()
        {
            return Player.gameObject.transform.position;
        }
    }

    private void Shockwave()
    {
        stateCoroutine = StartCoroutine(IShockwave());

        IEnumerator IShockwave()
        {
            while (state == State.Shockwave)
            {
                shockwaveController.StartShockwave();

                yield return null;
                SetState(State.Teleport);
            }
        }
    }

    public void CollideWithPlayer()
    {
        PlayerController.Instance.TakeDamage(data.GetDamageOnHitDash);

        SetState(State.Teleport);
    }

    public void CollideWithObstruction()
    {
        SetState(State.Shockwave);
    }

    protected override void Dying()
    {
        visual?.SetActive(false);
        physics?.SetActive(false);
    }

    public override bool IsMoving()
    {
        throw new System.NotImplementedException();
    }
}
