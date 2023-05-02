using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
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

    [SerializeField] internal float m_dashDuration = .2f;
    [SerializeField] internal float m_dashForce = 2f;

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
    LayerMask m_interactionMask;
    [SerializeField]
    AimAssistPreset m_aimAssistPresset;

    [SerializeField]
    TransitionController takeDamageTransition;

    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private PlayerAnimStates PlayerAnimStates;

    static PlayerController _instance;

    PlayerActions _playerActions;

    Rigidbody _rb;
    Transform _relativeTransform;

    Vector2 _moveInputValue;
    public Vector2 MoveInputValue
    {
        get => _moveInputValue;
    }

    Vector2 _aimInputValue;
    public Vector2 AimInputValue
    {
        get => _aimInputValue;
    }


    private bool m_isDashing = false;
    private Vector3 m_dashDirection = Vector2.zero;

    [HideInInspector]
    public bool _isAiming = false;

    bool _isAimingOnMouse = false;
    Vector3 _aimDirection;
    Vector3 _correctedAimDirection;
    float _aimMagnitude;

    PlayerInventoryScript _inventoryScript;
    EnemyManager _enemyManager;
    RoomManager _roomManager;
    PlayerCooking _cookingScript;

    bool _isLocked = false;
    private bool m_isGamePaused = false;

    Collider _curInteractCollider = null;

    private enum playerStates
    {
        Default,
        Cooking,
        UI
    }

    private playerStates _curState = playerStates.Default;

    public static PlayerController Instance
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
        _relativeTransform = m_mainCamera.transform;
        _inventoryScript = PlayerInventoryScript.Instance;
        _enemyManager = EnemyManager.Instance;
        _roomManager = RoomManager.instance;
        _cookingScript = GetComponent<PlayerCooking>();

        _rb = _rb != null ? _rb : GetComponent<Rigidbody>();


        pauseMenu.SetActive(false);

        //Set Input Actions Map
        _playerActions = new PlayerActions();
        _playerActions.Default.Enable();
        _playerActions.Cooking.Enable();
        _playerActions.UI.Enable();

        //Set Default Events
        _playerActions.Default.Shoot.performed += Shoot;
        _playerActions.Default.Move.performed += Move_Performed;
        _playerActions.Default.Move.canceled += Move_Canceled;
        _playerActions.Default.Aim.performed += Aim_Performed;
        _playerActions.Default.Aim.canceled += Aim_Canceled;
        _playerActions.Default.Dash.performed += Dash;
        _playerActions.Default.Cook.performed += Cook_Performed;
        _playerActions.Default.Quit.performed += Quit;

        //Set Cooking Events
        _playerActions.Cooking.Cook.canceled += Cook_Canceled;
        _playerActions.Cooking.SelectIngerdient.performed += SelectIngredient;
        _playerActions.Cooking.MoveInventorySlotLeft.performed += MoveInventorySlotLeft;
        _playerActions.Cooking.MoveInventorySlotRight.performed += MoveInventorySlotRight;
        _playerActions.Cooking.StartCrafting.performed += StartCraftingBullet;

        //Set UI Events
        _playerActions.UI.Pause.performed += OnPauseGame;

        _playerActions.Cooking.Disable();
        _playerActions.UI.Disable();

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
            _aimMagnitude = 1f;
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

        //Dash check
        if (!m_isDashing)
        {
            //Null Input Check
            if (_moveInputValue.magnitude <= 0)
            {
                //Stop if input is null
                _rb.velocity = Vector3.zero;

                PlayerAnimStates.animStates = PlayerAnimStates.playerAnimStates.IDLE;

                if (_isAiming)
                {
                    PlayerAnimStates.animStates = PlayerAnimStates.playerAnimStates.IDLEATTACK;
                }
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

                    PlayerAnimStates.animStates = PlayerAnimStates.playerAnimStates.RUNNING;
                }
                else
                {
                    PlayerAnimStates.animStates = PlayerAnimStates.playerAnimStates.RUNNINGATTACK;
                }
            }
        }
        else
        {
            //Dash

            if (m_dashDirection == Vector3.zero)
            {
                m_dashDirection = m_model.transform.forward;
                StartCoroutine(ICasting());
            }
            else
            {
                Move(m_dashDirection, m_moveSpeed * m_dashForce);
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
                if (_playerActions.Default.Interact.triggered)
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

        if (!m_isDashing && _rb.velocity.magnitude > m_maxMoveSpeed)
        {
            _rb.velocity = new Vector3(direction.x, 0, direction.z) * m_maxMoveSpeed;

        }
    }

    //Dash
    private void Dash(InputAction.CallbackContext context)
    {
        m_isDashing = true;
    }

    //Dash Casting
    private IEnumerator ICasting()
    {
        yield return new WaitForSeconds(m_dashDuration);
        m_isDashing = false;
        m_dashDirection = Vector2.zero;
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
        _aimMagnitude = 0f;

        m_aimArrow.SetActive(false);
    }

    #endregion

    #region Cooking
    void Cook_Performed(InputAction.CallbackContext context)
    {
        StartCookingState();

        //Start cooking
        _cookingScript.StartCooking();
    }

    void Cook_Canceled(InputAction.CallbackContext context)
    {
        StopCookingState();

        //Stop cooking
        _cookingScript.StopCooking();
    }

    void StartCookingState()
    {
        Debug.Log("cook");

        //Input state check
        if(_curState != playerStates.Default)
            return;

        //Set input state
        _playerActions.Default.Disable();
        _playerActions.Cooking.Enable();

        _curState = playerStates.Cooking;
    }

    public void StopCookingState()
    {
        Debug.Log("stop cook");

        //Input state check
        if (_curState != playerStates.Cooking)
            return;

        //Set input state
        _playerActions.Cooking.Disable();
        _playerActions.Default.Enable();

        _curState = playerStates.Default;
    }

    void SelectIngredient(InputAction.CallbackContext context)
    {
        //Active Inventory Check
        if (!_inventoryScript.IsDisplayed)
            return;

        _inventoryScript.SelectIngredient();
    }

    void MoveInventorySlotLeft(InputAction.CallbackContext context)
    {
        //Active Inventory Check
        if (!_inventoryScript.IsDisplayed)
            return;

        _inventoryScript._scroll.SwitchToLeftIngredient();
    }

    void MoveInventorySlotRight(InputAction.CallbackContext context)
    {
        //Active Inventory Check
        if (!_inventoryScript.IsDisplayed)
            return;

        _inventoryScript._scroll.SwitchToRightIngredient();
    }

    void StartCraftingBullet(InputAction.CallbackContext context)
    {
        //Active Inventory Check
        if (_inventoryScript.IsDisplayed)
        {
            _cookingScript.StartCrafting();
        }
        else
        {
            _cookingScript.CheckQTE();
        }
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
            GetComponent<PlayerAttack>().Shoot();
        }
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

        //Feedbacks
        CameraController.instance.ScreenShake();
        takeDamageTransition.LoadTransition();

        //Cooking cancel
        StopCookingState();

        //Death Check
        if (m_currentHealthValue <= 0)
        {
            m_currentHealthValue = m_maxHealthValue;
            RoomManager.instance.RestartLevel();
        }
    }

    void Spawn()
    {
        //transform.position = _roomManager.SpawnPoint.position;
    }

    void Quit(InputAction.CallbackContext context)
    {
        Application.Quit();
    }

    #endregion

    #region UI

    private void OnPauseGame(InputAction.CallbackContext callbackContext)
    {
        PauseGame();
    }

    public void PauseGame()
    {
        if (m_isGamePaused)
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }

        m_isGamePaused = !m_isGamePaused;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (m_aimAssistPresset)
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
    }

    private void Reset()
    {
        //Set Varialbes
        _rb = GetComponent<Rigidbody>();
    }
#endif
}
