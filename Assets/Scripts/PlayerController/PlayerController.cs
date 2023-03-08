using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("Character Properties")]
    [SerializeField]
    float m_currentHealthValue = 10f;
    [SerializeField]
    internal float m_maxHealthValue = 10f;
    [SerializeField]
    internal float m_rotationSpeed = 3f;
    [SerializeField]
    internal float m_moveSpeed = 5f;
    [SerializeField]
    internal float m_maxMoveSpeed = 5f;
    [SerializeField]
    internal float m_moveDrag = 1f;

    [SerializeField]
    internal float m_interactionRange = 0.5f;

    [Header("Init")]
    [SerializeField]
    Camera m_mainCamera;
    [SerializeField]
    GameObject m_model;

    [SerializeField]
    GameObject m_aimArrow;
    [SerializeField]
    float aimArrowGrowth = 0.01f;
    [SerializeField]
    float aimArrowDuration = 1;

    [SerializeField]
    PlayerKnockback m_knockback;
    [SerializeField]
    LayerMask m_interactionMask;
    [SerializeField]
    AimAssistPreset m_aimAssistPresset;

    [SerializeField]
    TransitionController takeDamageTransition;

    public PlayerController Instance
    {
        get => _instance;
    }

    public Vector3 PlayerAimDirection
    {
        get
        {
            if (_isAiming)
            {
                return _correctedAimDirection;
            }
            else
            {
                return _aimDirection;
            }
        }
    }

    public float PlayerAimMagnitude
    {
        get => _aimMagnitude;
    }

    PlayerController _instance;

    PlayerActions _playerActions;

    Rigidbody _rb;
    Transform _relativeTransform;

    Vector2 _moveInputValue;
    Vector2 _aimInputValue;

    public float AimInputValue
    {
        get => _aimInputValue.magnitude; 
    }

    [HideInInspector]
    public bool _isAiming = false;
    bool _isAimingOnMouse = false;
    Vector3 _aimDirection;
    Vector3 _correctedAimDirection;
    float _aimMagnitude;

    InventoryScript _scriptInventory;
    EnemyManager _enemyManager;
    RoomManager _roomManager;
    bool _isLocked = false;

    Collider _curInteractCollider = null;
    #endregion

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
        }

        _instance = this;
    }

    void Start()
    {
        //Set Varialbes
        _rb = GetComponent<Rigidbody>();
        _relativeTransform = m_mainCamera.transform;
        _scriptInventory = InventoryScript.instance;
        _enemyManager = EnemyManager.Instance;
        _roomManager = RoomManager.instance;

        //Set Input Actions Map
        _playerActions = new PlayerActions();
        _playerActions.Default_PlayerActions.Enable();
        //Set Events
        _playerActions.Default_PlayerActions.Shoot.performed += Shoot;
        _playerActions.Default_PlayerActions.Move.performed += Move_Performed;
        _playerActions.Default_PlayerActions.Move.canceled += Move_Canceled;
        _playerActions.Default_PlayerActions.Aim.performed += Aim_Performed;
        _playerActions.Default_PlayerActions.Aim.canceled += Aim_Canceled;
        _playerActions.Default_PlayerActions.Craft.performed += Craft;

        _roomManager.OnRoomStart += Spawn;

        m_currentHealthValue = m_maxHealthValue;

        m_aimArrow.SetActive(false);
    }

    private void Update()
    {
        //Block player actions
        if(_isLocked)
        {
            return;
        }

        //Inputs relative to camera
        Vector3 relativeForward = _relativeTransform.forward + _relativeTransform.up;
        Vector3 relativeRight = _relativeTransform.right;
        relativeForward.y = 0;
        relativeRight.y = 0;
        relativeForward.Normalize();
        relativeRight.Normalize();
        

        #region Aim
        //Mouse Inputs Check
        if (_isAimingOnMouse)
        {
            _aimInputValue = Input.mousePosition - m_mainCamera.WorldToScreenPoint(transform.position);
            _aimMagnitude = _aimInputValue.magnitude / 10;
        }

        //Null Input Check
        if (_aimMagnitude > 0)
        {
            //Rotate Player Model
            Vector3 aimInputDir = relativeForward * _aimInputValue.y + relativeRight * _aimInputValue.x;
            aimInputDir = aimInputDir.normalized;
            aimInputDir = new Vector3(aimInputDir.x, 0, aimInputDir.z);

            Rotate(aimInputDir);

            //Set Aim Variables
            _aimDirection = aimInputDir;

            //Correct Aim
            EnemyController[] enemiesInLevel = _enemyManager.EnemiesInLevel;
            GameObject[] aimTargets = new GameObject[enemiesInLevel.Length];
            for(int i =  0; i < enemiesInLevel.Length; i++)
            {
                aimTargets[i] = enemiesInLevel[i].gameObject; 
            }
            _correctedAimDirection = AimAssist2D.CorrectAimDirection(_aimDirection, transform.position, aimTargets, m_aimAssistPresset);
        }
        #endregion

        #region Movement
        //Null Input Check
        if (_moveInputValue.magnitude <= 0)
        {
            //Stop if input is null
            _rb.velocity = Vector3.zero;
        }
        else
        {
            //Move Player
            Vector3 moveInputDir = relativeForward * _moveInputValue.y + relativeRight * _moveInputValue.x;
            moveInputDir = moveInputDir.normalized;

            float speed = m_moveSpeed * _moveInputValue.sqrMagnitude;

            Move(moveInputDir, speed);

            //Rotate model if player is not aiming
            if (!_isAiming)
            {
                Rotate(moveInputDir);

                //Set Aiming Variables
                _aimDirection = moveInputDir;
            }
        }


        #endregion

        #region Interact
        Collider[] interactableColliders = new Collider[3];

        //Get interactable objects in range
        int foundObjects = Physics.OverlapSphereNonAlloc(transform.position + m_model.transform.forward * 0.5f, m_interactionRange, interactableColliders, m_interactionMask);
        if (foundObjects > 0)
        {
            //Get closest object
            float shortestDistance = Mathf.Infinity;
            int shortestIndex = 0;
            for (int index = 0; index < interactableColliders.Length; index++)
            {
                if (interactableColliders[index] == null)
                {
                    break;
                }

                float distance = Vector3.Distance(transform.position, interactableColliders[index].transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    shortestIndex = index;
                }
            }

            //Reset current interactable object if it is not the closest
            if (_curInteractCollider != null && _curInteractCollider != interactableColliders[shortestIndex])
            {
                var curInteractable = _curInteractCollider.GetComponent<IInteractable>();
                curInteractable.Interactable(false);
            }

            //Set new current interactable object
            _curInteractCollider = interactableColliders[shortestIndex];        
            if (interactableColliders[shortestIndex].TryGetComponent<IInteractable>(out var interactable))
            {
                interactable.Interactable(true);

                //Interact if button is pressed
                if (_playerActions.Default_PlayerActions.Interact.triggered)
                {
                    interactable.Interact("Default");
                }
            }
        }
        else
        {
            //If not in range, reset current interactable object
            if (_curInteractCollider != null)
            {
                var curInteractable = _curInteractCollider.GetComponent<IInteractable>();
                curInteractable.Interactable(false);
                _curInteractCollider = null;
            }
        }
        #endregion
    }

    #region Movement
    void Move_Performed(InputAction.CallbackContext context)
    {
        _moveInputValue = context.ReadValue<Vector2>();
    }

    void Move_Canceled(InputAction.CallbackContext context)
    {
        _moveInputValue = Vector2.zero;
    }

    void Move(Vector3 direction, float speed)
    {
        _rb.AddForce(100f * speed * Time.deltaTime * direction, ForceMode.Force);
        _rb.drag = m_moveDrag;

        if (_rb.velocity.magnitude > m_maxMoveSpeed)
        {
            _rb.velocity = new Vector3(direction.x, _rb.velocity.y, direction.z) * m_maxMoveSpeed;
        }
    }
    #endregion

    #region Aim
    void Aim_Performed(InputAction.CallbackContext context)
    {
        _isAiming = true;

        m_aimArrow.SetActive(true);

        var inputType = context.control.layout;

        //Check Input Device
        switch(inputType)
        {
            //M&K
            case ("Button"):
                {
                    _isAimingOnMouse = true;
                    return;
                }
            //Gamepad
            case ("Stick"):
                {
                    _aimInputValue = context.ReadValue<Vector2>();
                    _aimMagnitude = _aimInputValue.magnitude;
                    return;
                }
        }
    }

    void Aim_Canceled(InputAction.CallbackContext context)
    {
        //Reset
        _isAiming = false;
        _isAimingOnMouse = false;
        _aimInputValue = Vector2.zero;

        //m_aimArrow.SetActive(false);
    }

    #endregion

    #region Other Actions

    void Shoot(InputAction.CallbackContext context)
    {
        //Block player actions
        if (_isLocked)
        {
            return;
        }

        if (context.performed)
        {
            m_knockback.StartKnockback();
            GetComponent<PlayerAttack>().Shoot();
        }
    }

    void Craft(InputAction.CallbackContext context)
    {
        _scriptInventory.Craft();
    }

    #endregion

    #region Utilities

    public void Lock(bool isLocked)
    {
        _isLocked = isLocked;
    }

    void Rotate(Vector3 direction)
    {
        float newX = Mathf.Lerp(m_model.transform.forward.x, direction.x, Time.deltaTime * m_rotationSpeed);
        float newZ = Mathf.Lerp(m_model.transform.forward.z, direction.z, Time.deltaTime * m_rotationSpeed);
        m_model.transform.forward = new Vector3(newX, 0, newZ);
    }

    public void TakeDamage(float damage) 
    {
        m_currentHealthValue -= Mathf.Abs(damage);
        CameraController.instance.ScreenShake();
        takeDamageTransition.LoadTransition();
        if (m_currentHealthValue <= 0)
        {
            m_currentHealthValue = m_maxHealthValue;
            RoomManager.instance.LoadRandomLevel();
        }
    }

    void Spawn()
    {
        transform.position = _roomManager.SpawnPoint.position;
    }

    #endregion

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 target = transform.position + _aimDirection * m_aimAssistPresset.GetMaxDistance;
        //Draw base Aim
        Gizmos.DrawLine(transform.position, target);

        Gizmos.color = Color.green;
        target = transform.position + _correctedAimDirection * m_aimAssistPresset.GetMaxDistance;
        //Draw base Aim
        Gizmos.DrawLine(transform.position, target);
    }
#endif
}
