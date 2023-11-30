using System.Collections;
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
    private Coroutine rotationCoroutine;
    private Vector3 targetPosition;

    [SerializeField] ParticleSystem _teleportParticles;

    #region Debug

    private float radiusTeleportDebug = 0;
    private Vector3 targetTeleportDebug;

    #endregion

    protected override void Start()
    {
        base.Start();

        missilesController = GetComponentInChildren<MissilesController>();
        shockwaveController = GetComponentInChildren<ShockwaveController>();

        Healthpoint = data.GetHealth;

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
        radiusTeleportDebug = 0;

        if (stateCoroutine != null)
            stateCoroutine = null;

        if (rotationCoroutine != null)
            rotationCoroutine = null;

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
        rotationCoroutine = StartCoroutine(IRotateToPlayer());

        Vector3 teleportTarget = GetTargetPosition();

        IEnumerator ITeleport()
        {
            if (_teleportParticles)
                _teleportParticles.transform.parent = null;

            while (state == State.Teleport)
            {

#if UNITY_EDITOR
                radiusTeleportDebug = 2;
#endif

                if (_teleportParticles)
                    _teleportParticles.Play();

                var VOLT = _teleportParticles.velocityOverLifetime;
                VOLT.y = new ParticleSystem.MinMaxCurve(1, 0);

                _teleportParticles.transform.position = Player.transform.position;
                Vector3 tpPos = Player.transform.position;

                teleportTarget = GetTargetPosition();
                targetTeleportDebug = teleportTarget;

                yield return new WaitForSeconds(data.GetDelayBeforeTeleport);
                transform.position = tpPos;

                if (_teleportParticles)
                {
                    VOLT.y = new ParticleSystem.MinMaxCurve(1, 20);
                    _teleportParticles.Stop();
                }

                yield return new WaitForSeconds(data.DelayBeforeTakingLastPlayerPosition);

                transform.position = teleportTarget;

                SetState(State.CastMissiles);
            }
        }
    }

    private IEnumerator IRotateToPlayer()
    {
        while (state == State.CastMissiles)
        {
            visual.transform.LookAt(new Vector3(Player.transform.position.x, visual.transform.position.y, Player.transform.position.z));
            yield return null;
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
        rotationCoroutine = StartCoroutine(IRotateToPlayer());

        IEnumerator ICastDashing()
        {
            while (state == State.Dash)
            {
                transform.LookAt(new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z));

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
        rotationCoroutine = StartCoroutine(IRotateToPlayer());

        IEnumerator IShockwave()
        {
            while (state == State.Shockwave)
            {
                shockwaveController.StartShockwave();

                SetState(State.Teleport);
                yield return null;
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

    public override void TakeDamage(float damage = 1, bool isCritical = false)
    {
        base.TakeDamage(damage, isCritical);

        if (Healthpoint <= 0)
            SetState(State.Dying);
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

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetTeleportDebug, radiusTeleportDebug);
    }

#endif
}
