using System.Collections;
using Enemy.Data;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Minimoyz
{
    public class MinimoyzController : EnemyController
    {
        [SerializeField] private MinimoyzData data;
        [SerializeField] private SlimeData slimeData;
        [SerializeField] private NavMeshAgent agent;

        private Coroutine _castingCoroutine;
        private Coroutine _attackCoroutine;
        private bool _shouldChaseAndAttack;
        private bool _isThrowing;
        
        [SerializeField] private GameObject physicsMinimoyz;
    
        public enum State
        {
            Neutral,
            Throw,
            Chase,
            Cast,
            Attack,
            ChaseAndAttack,
            Dying,
        }
    
        public State state;

        protected override void Awake()
        {
            base.Awake();
        
            agent = GetComponent<NavMeshAgent>();
            agent.speed = data.GetSpeed();
            agent.stoppingDistance = data.GetAttackRange();
            FocusPlayer = data.GetFocusPlayer();
            Healthpoint = data.GetHealth();
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            if (_isThrowing)
                SetState(State.Throw);
            else if (FocusPlayer)
                state = State.Chase;

            physicsMinimoyz.SetActive(false);
            base.Start();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        
            StateManagement();
        }

        protected void FixedUpdate()
        {
            if (state == State.Dying)
                return;
        
            AreaDetection();
        }

        public void SetFocus(bool value = true)
        {
            FocusPlayer = value;
        }

        private void StateManagement()
        {
            switch (state)
            {
                case State.Neutral:
                    break;
                case State.Throw:
                    Throw();
                    break;
                case State.Chase:
                    Chase();
                    break;
                case State.Cast:
                    Cast();
                    break;
                case State.Attack:
                    Attack(Attack, data.GetAttackSpeed());
                    break;
                case State.ChaseAndAttack:
                    ChaseAndAttack();
                    break;
                case State.Dying:
                    Dying();
                    break;
                default:
                    Dying();
                    break;
            }
        }
    
        private void AreaDetection()
        {
            if (state == State.Dying)
                return;

            if (Vector3.Distance(transform.position, Player.transform.position) <= data.GetFocusRange())
            {
                FocusPlayer = true;
            
                if (_shouldChaseAndAttack)
                    state = State.ChaseAndAttack;
                else
                    state = State.Chase;
            }
            else
            {
                state = State.Neutral;
            }

            if (Vector3.Distance(transform.position, Player.transform.position) <= data.GetAttackRange())
            {
                if (_shouldChaseAndAttack)
                    state = State.ChaseAndAttack;
                else
                    state = State.Attack;
            }
            else
            {
                if (FocusPlayer)
                    state = State.Chase;
            }
        }
    
        public State GetState()
        {
            return state;
        }
    
        public void SetState(State value)
        {
            state = value;
        }

        public void SetIsThrowing(bool value)
        {
            _isThrowing = value;
        }

        public override bool IsMoving()
        {
            throw new System.NotImplementedException();
        }

        public void Throw()
        {
            agent.enabled = false;
            
            StartCoroutine(IChangeState());
            
            IEnumerator IChangeState()
            {
                yield return new WaitForSeconds(slimeData.GetThrowingSpeed);
                agent.enabled = true;
                physicsMinimoyz.SetActive(true);
                SetState(State.Chase);
            }
        }

        private void Cast()
        {
            if (state == State.Dying)
                return;
        
            if (_castingCoroutine == null)
            {
                _castingCoroutine = StartCoroutine(CastAttack());
            }
        }

        private IEnumerator CastAttack()
        {
            yield return new WaitForSeconds(0.5f);
        }
    
        private void Attack()
        {
            if (state == State.Dying)
                return;
        
            if (_attackCoroutine == null)
                _attackCoroutine = StartCoroutine(ISettingAttack());
        
            IEnumerator ISettingAttack()
            {
                yield return new WaitForSeconds(0.4f);

                if (Vector3.Distance(transform.position, Player.transform.position) > data.GetAttackRange())
                {
                    CancelCast();
                    _attackCoroutine = null;
                    SetState(State.Chase);
                }

                yield return new WaitForSeconds(0.5f);
            
                if (Vector3.Distance(transform.position, Player.transform.position) > data.GetAttackRange())
                {
                    _shouldChaseAndAttack = true;
                    _attackCoroutine = null;
                    SetState(State.ChaseAndAttack);
                }
                else
                {
                    _attackCoroutine = null;
                    HitPlayer();
                }
            }
        }
    
        private void CancelCast()
        {
            _castingCoroutine = null;
            state = State.Chase;
        }

        private void HitPlayer()
        {
            Player.GetComponent<PlayerController>().TakeDamage(data.GetDamage());
            SetState(State.Cast);
        }

        protected override void Chase()
        {
            if (!physicsMinimoyz.activeSelf)
                physicsMinimoyz.SetActive(true);
            
            if (agent.enabled)
                agent.SetDestination(Player.transform.position);
        }
    
        private void ChaseAndAttack()
        {
            Chase();
        
            if (Vector3.Distance(transform.position, Player.transform.position) <= data.GetAttackRange())
            {
                HitPlayer();
                _shouldChaseAndAttack = false;
            }
        }
    
        public override void TakeDamage(float damage = 1, bool isCritical = false)
        {
            base.TakeDamage(damage, isCritical);
        
            if (Healthpoint <= 0)
            {
                state = State.Dying;
            }
        }
    }
}
