using System.Collections;
using UnityEngine;

namespace Enemy.DashingEnemy
{
    public class ChargingEnemy : EnemyController
    {
        [Header("Sound")]
        [SerializeField]
        private AK.Wwise.Event _Play_SFX_Cabbage_Footsteps;
        [SerializeField]
        private AK.Wwise.Event _Play_SFX_Cabbage_Charge_LP;
        [SerializeField]
        private AK.Wwise.Event _Play_SFX_Cabbage_Charge_Impact;
        [SerializeField]
        private AK.Wwise.Event _Play_SFX_Cabbage_Hit;
        [SerializeField]
        private AK.Wwise.Event _Play_SFX_Cabbage_Death;
        [SerializeField]
        private AK.Wwise.Event _Play_Weapon_Hit;

        private Coroutine _coroutineState;
        private Coroutine _rotateCoroutine;
        private RaycastHit _hit;

        private bool _isCharging = false;
        private bool _canShowingRedLine = false;
        //private bool _changeStateToWaiting = false;
    
        private Vector3 _direction;

        [SerializeField] private GameObject _redLine;
        [SerializeField] private EnemyDashingData _data;
    
        private Material _redLineMaterial;
        //private bool _isRedLineFullVisible = false;

        [SerializeField]
        private GameObject visual;

        private Animator animator;
        [SerializeField] private GameObject physics;

        public enum State
        {
            Casting,
            Waiting,
            Dashing,
            Dying
        }

        public State state;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            Healthpoint = _data.GetHealth();
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            animator = GetComponentInChildren<Animator>();
            SetState(State.Casting);
            _redLineMaterial = _redLine.GetComponent<Renderer>().material;
        }

        public override bool IsMoving()
        {
            return false;
        }

        private State GetState()
        {
            return state;
        }
    
        private void SetState(State value)
        {
            if (_coroutineState != null)
                StopCoroutine(_coroutineState);
            
            if (_rotateCoroutine != null)
                StopCoroutine(_rotateCoroutine);
        
            state = value;
            
            StateManagement();
        }
    
        private void StateManagement()
        {
            switch (state)
            {
                case State.Casting:
                    animator.SetBool("isAttack", false);
                    Casting();
                    break;
                case State.Waiting:
                    animator.SetBool("isAttack", false);
                    WaitingAnotherDash();
                    break;
                case State.Dashing:
                    animator.SetBool("isAttack", true);
                    _Play_SFX_Cabbage_Charge_LP.Post(gameObject);
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

        /// <summary>
        /// Dashing State :
        /// Stop casting and dash to player
        /// </summary>
        private void Dashing()
        {
            ShowFullyRedLine();
            
            RaycastHit hit;
            Vector3 direction = (Player.transform.position - transform.position).normalized;
            
            if (Physics.Raycast(transform.position, direction, out hit, Vector3.Distance(transform.position, Player.transform.position)))
            {
                if (!hit.collider.CompareTag("Player"))
                {
                    SetState(State.Casting);
                    
                    return;
                }
                
                _isCharging = true;
                _coroutineState = StartCoroutine(ChargingToPlayer());
            }

            IEnumerator ChargingToPlayer()
            {
                while (_isCharging)
                {
                    if (_direction != Vector3.zero)
                    {
                        transform.position += _direction * (_data.GetSpeed() * Time.deltaTime);
                    }
                    else
                    {
                        _isCharging = false;
                    }
                
                    yield return null;
                }
            }
        }
    
        private void StopMoving()
        {
            _direction = Vector3.zero;
            
            HideRedLine();
        }

        /// <summary>
        /// Casting State :
        /// Preparing to dash and Look at player
        /// </summary>
        private void Casting()
        {
            HideRedLine();
            
            //_changeStateToWaiting = false;
            _isCharging = false;
            
            _coroutineState = StartCoroutine(ICasting());
            _rotateCoroutine = StartCoroutine(RotateToPlayer());

            IEnumerator RotateToPlayer()
            {
                while (GetState() == State.Casting)
                {
                    visual.transform.LookAt(new Vector3(Player.transform.position.x, visual.transform.position.y, Player.transform.position.z));
                    yield return null;
                }
            }
        
            IEnumerator ICasting()
            {
                yield return StartCoroutine(ICanShowingRedLine());
        
                // ShowLightRedLine();
        
                yield return new WaitForSeconds(_data.GetTimeBeforeLerpRedLine());
        
                // ShowFullyRedLine();
        
                yield return new WaitForSeconds(_data.GetRemainingForDash());

                if (!_isCharging)
                    _direction = GetPlayerDirection();
            
                SetState(State.Dashing);
            }
        }
    
        /// <summary>
        /// Wait some time before casting again
        /// </summary>
        private void WaitingAnotherDash()
        {
            _isCharging = false;
            //_isRedLineFullVisible = false;
            _canShowingRedLine = false;

            _coroutineState = StartCoroutine(IWaiting());
            StopCoroutine(ICanShowingRedLine());
        
            IEnumerator IWaiting()
            {
                yield return new WaitForSeconds(_data.GetTimeWaitingDash());
        
                SetState(State.Casting);
            }
        }
    
        /// <summary>
        /// Dying Test
        /// </summary>
        protected override void Dying()
        {
            HideRedLine();
            physics.SetActive(false);
            
            StopCoroutine(_rotateCoroutine);
            StopCoroutine(_coroutineState);
            
            _Play_SFX_Cabbage_Death.Post(gameObject);

            animator.SetBool("isDead", true);

            StartCoroutine(IDeathAnim());

            IEnumerator IDeathAnim()
            {
                yield return new WaitForSeconds(2f);
                base.Dying();
            }
        }

        /// <summary>
        /// Return the direction to player
        /// </summary>
        /// <returns></returns>
        private Vector3 GetPlayerDirection()
        {
            _isCharging = true;
            Vector3 direction = (Player.transform.position - transform.position).normalized;
            direction.y = 0;
        
            return direction;
        }

        private IEnumerator ICanShowingRedLine()
        {
            if (_canShowingRedLine)
                yield break;
        
            HideRedLine();
            yield return new WaitForSeconds(_data.GetTimeBeforeShowingRedLine());
        }

        /// <summary>
        /// Make Red line more visible
        /// </summary>
        private void ShowFullyRedLine()
        {
            _redLineMaterial.SetFloat("_Alpha", 0.3f);
        }
        
        private void ShowLightRedLine()
        {
            _canShowingRedLine = true;
            _redLineMaterial.SetFloat("_Alpha", 0.85f);
        }

        private void HideRedLine()
        {
            if (_redLineMaterial)
                _redLineMaterial.SetFloat("_Alpha", 1f);
        }
    
        public void CollideWithPlayer()
        {
            _Play_SFX_Cabbage_Charge_Impact.Post(gameObject);
            StopMoving();
            Player.GetComponent<PlayerController>().TakeDamage(_data.GetDamage());

            SetState(State.Waiting);
        }
    
        public void CollideWithObstruction()
        {
            _Play_SFX_Cabbage_Charge_Impact.Post(gameObject);
            StopMoving();
            SetState(State.Waiting);
        }

        public override void TakeDamage(float damage = 1, bool isCritical = false)
        {
            _Play_SFX_Cabbage_Hit.Post(gameObject);
            base.TakeDamage(damage, isCritical);
        }
    }
}