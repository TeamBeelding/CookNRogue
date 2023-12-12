using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Image bossHealthBar;

    [SerializeField] Transform _teleportParticlesContainer;
    ParticleSystem[] _teleportParticles;
    [SerializeField] Transform _dirtParticlesContainer;
    ParticleSystem[] _dirtParticles;

    [SerializeField] Transform _dashParticlesContainer;
    ParticleSystem[] _dashParticles;

    [Header("Sound")]
    [SerializeField]
    private AK.Wwise.Event _Play_SFX_Boss_Leaves;

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
        _dirtParticles = _dirtParticlesContainer.GetComponentsInChildren<ParticleSystem>();
        _dashParticles = _dashParticlesContainer.GetComponentsInChildren<ParticleSystem>();
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
            if (_teleportParticlesContainer)
                _teleportParticlesContainer.transform.parent = null;

            teleportTarget = GetTargetPosition();
            
            foreach (var particle in _teleportParticles)
            {
                particle.Play();
                var VOLT = particle.velocityOverLifetime;
                VOLT.x = new ParticleSystem.MinMaxCurve(0, 0);
                VOLT.y = new ParticleSystem.MinMaxCurve(0, 0);
                VOLT.z = new ParticleSystem.MinMaxCurve(0, 0);
            }

            _teleportParticlesContainer.transform.position = teleportTarget;
            _Play_SFX_Boss_Leaves.Post(gameObject);
            while (state == State.Teleport)
            {
              
                yield return new WaitForSeconds(data.GetDelayBeforeTeleport);

                shockwaveController.ResetRadiusPos();

                yield return new WaitForSeconds(data.DelayBeforeTakingLastPlayerPosition);

                transform.position = teleportTarget;

                SetState(State.CastMissiles);
            }

            foreach (var particle in _teleportParticles)
            {

                var VOLT = particle.velocityOverLifetime;
                VOLT.x = new ParticleSystem.MinMaxCurve(-10, 10);
                VOLT.y = new ParticleSystem.MinMaxCurve(1, 10);
                VOLT.z = new ParticleSystem.MinMaxCurve(-10, 10);
                particle.Stop();
            }

            foreach (var particle in _dirtParticles)
            {
                particle.Play();
            }
        }
    }

    private void CastMissiles()
    {
        stateCoroutine = StartCoroutine(ICastMissiles());
        rotationCoroutine = StartCoroutine(IRotateToPlayer());

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

            foreach(var particle in _dashParticles)
            {
                particle.Play();
            }

            while (state == State.Dash)
            {
                transform.position += direction * (data.GetDashSpeed * Time.deltaTime);
                yield return null;
            }

            foreach (var particle in _dashParticles)
            {
                particle.Stop();
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

                yield return new WaitForSeconds(2);
                EndShockwave();

                yield return null;
            }
        }
    }

    public void EndShockwave()
    {
        SetState(State.Teleport);
    }

    private IEnumerator IRotateToPlayer()
    {
        while (state == State.CastMissiles)
        {
            visual.transform.LookAt(new Vector3(Player.transform.position.x, visual.transform.position.y, Player.transform.position.z));
            yield return null;
        }
    }

    public override void TakeDamage(float damage = 1, bool isCritical = false)
    {
        base.TakeDamage(damage, isCritical);

        UpdateBossHealthBar();

        if (Healthpoint <= 0)
            SetState(State.Dying);
    }

    private void UpdateBossHealthBar()
    {
        if (!bossHealthBar)
            return;

        bossHealthBar.fillAmount = Mathf.Clamp(Healthpoint, 0, data.GetHealth) / data.GetHealth;
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
