using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;
using Enemy;
using Tutoriel;
using UnityEngine.SceneManagement;
using static UnityEngine.ParticleSystem;

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
    [SerializeField] float m_dashCooldown = 2f;

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
    private GameObject deathMenu;
    [SerializeField]
    private GameObject victoryMenu;

    [SerializeField]
    private GameObject m_debugModeUI;

    [SerializeField]
    private PlayerAnimStates playerAnimStatesScript;

    static PlayerController _instance;

    PlayerActions _playerActions;
    PlayerHealth _playerHealth;

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


    private bool _isDashing = false;
    public bool IsDashing
    {
        get => _isDashing;
    }
    bool _dashOnCooldown = false;

    [SerializeField] ParticleSystem DashingParticles;

    private Vector3 m_dashDirection = Vector2.zero;

    [HideInInspector]
    public bool _isAiming = false;

    bool _isAimingOnMouse = false;
    Vector3 _aimDirection;
    Vector3 _correctedAimDirection;
    float _aimMagnitude;

    PlayerCookingInventory _inventoryScript;
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


    [SerializeField]
    private AK.Wwise.Event _Stop_SFX_Cook;
    [SerializeField]
    private AK.Wwise.Event _Play_MC_Dash;


    public float PlayerAimMagnitude
    {
        get => _aimMagnitude;
    }
    #endregion

    #region Tutorial

    [Header("Tutorial")]
<<<<<<< HEAD

    [SerializeField] private bool _isOnTutorial = false;
    [SerializeField] private TutorialManager _tutorialManager;
=======
    [SerializeField] private bool isOnTutorial = false;
    [SerializeField] private TutorialManager tutorialManager;
>>>>>>> parent of c4498263 (Revert "Merge branch 'develop' into LD_Demo")

    #endregion

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }

        _instance = this;
    }

    void Start()
    {
<<<<<<< HEAD
        if (_isOnTutorial)
            _tutorialManager = GameObject.FindGameObjectWithTag("TutorialManager").GetComponent<TutorialManager>();
=======
        if (isOnTutorial)
            tutorialManager = GameObject.FindGameObjectWithTag("TutorialManager").GetComponent<TutorialManager>();
>>>>>>> parent of c4498263 (Revert "Merge branch 'develop' into LD_Demo")

        _relativeTransform = m_mainCamera.transform;
        _inventoryScript = PlayerCookingInventory.Instance;
        _enemyManager = EnemyManager.Instance;
        _roomManager = RoomManager.instance;
        _cookingScript = GetComponent<PlayerCooking>();
        _playerHealth = GetComponent<PlayerHealth>();
        _rb = _rb != null ? _rb : GetComponent<Rigidbody>();


        pauseMenu.SetActive(false);
        deathMenu.SetActive(false);

        m_debugModeUI.SetActive(false);

        #region Set Inputs
        //Set Input Actions Map
        _playerActions = new PlayerActions();
        _playerActions.Default.Enable();
        _playerActions.Cooking.Enable();
        _playerActions.UI.Enable();
        _playerActions.Debug.Enable();

        //Set Default Events
        _playerActions.Default.Shoot.performed += Shoot_Performed;
        _playerActions.Default.Shoot.canceled += Shoot_Canceled;
        _playerActions.Default.Move.performed += Move_Performed;
        _playerActions.Default.Move.canceled += Move_Canceled;
        _playerActions.Default.Aim.performed += Aim_Performed;
        _playerActions.Default.Aim.canceled += Aim_Canceled;
        _playerActions.Default.Dash.performed += Dash;
        _playerActions.Default.Cook.started += Cook_Performed;
        _playerActions.Default.Pause.performed += OnPauseGame;
        _playerActions.Default.EnterDebug.started += EnterDebug;

        //Set Cooking Events
        _playerActions.Cooking.Cook.canceled += Cook_Canceled;
        _playerActions.Cooking.SelectIngredient.performed += SelectIngredient;
        _playerActions.Cooking.StartCrafting.performed += StartCraftingBullet;
        _playerActions.Cooking.IngredientSelector.performed += OnIngredientSelectorInput;
        _playerActions.Cooking.IngredientSelector.canceled += OnIngredientSelectorInputStop;
        _playerActions.Cooking.ChangeWheel.performed += OnChangeUIWheel;


        //Set UI Events
        _playerActions.UI.Pause.performed += OnPauseGame;

        //Set Debug Events
        _playerActions.Debug.EnterDebug.started += QuitDebug;
        _playerActions.Debug.KillAllEnemies.started += KillAllEnemies;
        _playerActions.Debug.ReloadLevel.started += ReloadLevel;
        _playerActions.Debug.GoToNextLevel.started += GotToNextLevel;
        _playerActions.Debug.GoToPreviousLevel.started += GotToPreviousLevel;
        _playerActions.Debug.SkipTuto.started += SkipTuto;

        _playerActions.Cooking.Disable();
        _playerActions.UI.Disable();
        _playerActions.Debug.Disable();
        #endregion

        if (_roomManager)
            _roomManager.OnRoomStart += Spawn;

        m_currentHealthValue = m_maxHealthValue;

        m_aimArrow.SetActive(false);
    }

    private void Update()
    {
        //Block player actions
        if (_isLocked)
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

        //Dash Check
        if (!_isDashing)
        {
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
                for (int i = 0; i < enemiesInLevel.Length; i++)
                {
                    aimTargets[i] = enemiesInLevel[i].gameObject;
                }
                _correctedAimDirection = AimAssist2D.CorrectAimDirection(_aimDirection, transform.position, aimTargets, m_aimAssistPresset);
            }
        }

        #endregion

        #region Movement

        //Dash check
        if (!_isDashing)
        {
            //Null Input Check
            if (_moveInputValue.magnitude <= 0)
            {
                //Stop if input is null
                _rb.velocity = Vector3.zero;

                playerAnimStatesScript.animStates = PlayerAnimStates.playerAnimStates.IDLE;

                if (_isAiming)
                {
                    playerAnimStatesScript.animStates = PlayerAnimStates.playerAnimStates.IDLEATTACK;
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

                    playerAnimStatesScript.animStates = PlayerAnimStates.playerAnimStates.RUNNING;
                }
                else
                {
                    playerAnimStatesScript.animStates = PlayerAnimStates.playerAnimStates.RUNNINGATTACK;
                }
            }
        }
        else
        {
            //Dash

            if (m_dashDirection == Vector3.zero)
            {
                //Null Input Check
                if (_moveInputValue.magnitude <= 0)
                {
                    m_dashDirection = m_model.transform.forward;
                }
                else
                {
                    Vector3 moveInputDir = relativeForward * _moveInputValue.y + relativeRight * _moveInputValue.x;
                    moveInputDir = moveInputDir.normalized;

                    m_dashDirection = moveInputDir;
                }

                StartCoroutine(ICasting());
            }
            else
            {
                Rotate(m_dashDirection);
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

    public bool GetIsOnTutorial()
    {
        return _isOnTutorial;
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

        if (!_isDashing && _rb.velocity.magnitude > m_maxMoveSpeed)
        {
            _rb.velocity = new Vector3(direction.x, 0, direction.z) * m_maxMoveSpeed;
        }

        CheckingIfPlayerIsMovingForTutorial();
    }

    private void CheckingIfPlayerIsMovingForTutorial()
    {
        if (!_tutorialManager)
            return;

<<<<<<< HEAD
        if (_isOnTutorial)
            _tutorialManager.SetIsMoving(true);
=======
        if (isOnTutorial)
            tutorialManager.SetIsMoving(true);
>>>>>>> parent of c4498263 (Revert "Merge branch 'develop' into LD_Demo")
    }

    public void CheckingIfCookingIsDone()
    {
        if (!_tutorialManager)
            return;

<<<<<<< HEAD
        if (_isOnTutorial)
            _tutorialManager.SetIsCookingDone(true);
=======
        if (isOnTutorial)
            tutorialManager.SetIsCookingDone(true);
>>>>>>> parent of c4498263 (Revert "Merge branch 'develop' into LD_Demo")
    }

    //Dash
    private void Dash(InputAction.CallbackContext context)
    {
        if (_dashOnCooldown)
        {
            return;
        }

        _isDashing = true;
        _Play_MC_Dash.Post(gameObject);

        StartCoroutine(IDashCooldown());
    }

    //Dash Casting
    private IEnumerator ICasting()
    {
        //Aim Check;
        if (_isAiming)
        {
            m_aimArrow.SetActive(false);
        }

        DashingParticles.Play();
        yield return new WaitForSeconds(m_dashDuration);
        _isDashing = false;
        m_dashDirection = Vector2.zero;
        DashingParticles.Stop();

        //Aim Check;
        if (_isAiming)
        {
            m_aimArrow.SetActive(true);
        }
    }

    private IEnumerator IDashCooldown()
    {
        _dashOnCooldown = true;
        yield return new WaitForSeconds(m_dashCooldown);
        _dashOnCooldown = false;
    }

    #endregion

    #region Aim
    void Aim_Performed(InputAction.CallbackContext context)
    {
        if (_isDashing)
        {
            return;
        }

        _isAiming = true;

        m_aimArrow.SetActive(true);

        var inputType = context.control.layout;

        //Check Input Device
        switch (inputType)
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
        playerAnimStatesScript._animator.SetBool("cooking", true);
        playerAnimStatesScript.Marmite(false, true);

        //Input state check
        if (_curState != playerStates.Default)
            return;

        //Set input state
        _playerActions.Default.Disable();
        _playerActions.Cooking.Enable();

        _curState = playerStates.Cooking;
    }

    public void StopCookingState()
    {
        playerAnimStatesScript._animator.SetBool("cooking", false);
        playerAnimStatesScript.Marmite(false, false);
        _Stop_SFX_Cook.Post(gameObject);
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
        if (!_inventoryScript.IsDisplayed())
            return;

        _inventoryScript.SelectIngredient();
    }

    void OnIngredientSelectorInput(InputAction.CallbackContext context)
    {
        //Active Inventory Check
        if (!_inventoryScript.IsDisplayed())
            return;

        _inventoryScript.OnSelectorInput(context.ReadValue<Vector2>().normalized);
    }

    void OnIngredientSelectorInputStop(InputAction.CallbackContext context)
    {
        //Active Inventory Check
        if (!_inventoryScript.IsDisplayed())
            return;

        _inventoryScript.OnSelectorInputStop();
    }

    void OnChangeUIWheel(InputAction.CallbackContext context)
    {
        //Active Inventory Check
        if (!_inventoryScript.IsDisplayed())
            return;

        _inventoryScript.SwitchWheel();
    }

    void StartCraftingBullet(InputAction.CallbackContext context)
    {
        //Active Inventory Check
        if (_inventoryScript.IsDisplayed())
        {
            _cookingScript.StartCrafting();
        }
        else
        {
            _cookingScript.CheckQTE();
        }
    }

    public void QTEAppear()
    {
        if (_isOnTutorial)
            _tutorialManager.SetIsQTE(true);
    }

    #endregion

    #region Other Actions

    void Shoot_Performed(InputAction.CallbackContext context)
    {
        //Block player actions
        if (_isLocked || IsDashing)
        {
            return;
        }

        if (context.performed)
        {
            GetComponent<PlayerAttack>().SetIsShooting(true);
        }
    }

    void Shoot_Canceled(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            GetComponent<PlayerAttack>().SetIsShooting(false);
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
        //Feedbacks
        CameraController.instance.ScreenShake();
        takeDamageTransition.LoadTransition();

        //Cooking cancel
        StopCookingState();
        
        if (_tutorialManager)
            return;
            
        if (!_playerHealth.TakeDamage(1))
        {
            PauseGame();
            _playerActions.UI.Enable();
            _playerActions.Default.Disable();
            _playerActions.Cooking.Disable();
            pauseMenu.SetActive(false);
            victoryMenu.SetActive(false);
            deathMenu.SetActive(true);
        }

    }
    
    void Spawn()
    {
        //transform.position = _roomManager.SpawnPoint.position;
    }

    #endregion

    #region Debug

    void EnterDebug(InputAction.CallbackContext context)
    {
        _playerActions.Default.Disable();
        _playerActions.Debug.Enable();

        Time.timeScale = 0;

        m_debugModeUI.SetActive(true);
    }

    void QuitDebug(InputAction.CallbackContext context)
    {
        _playerActions.Debug.Disable();
        _playerActions.Default.Enable();

        Time.timeScale = 1;

        m_debugModeUI.SetActive(false);
    }

    void QuitDebug()
    {
        _playerActions.Debug.Disable();
        _playerActions.Default.Enable();

        Time.timeScale = 1;

        m_debugModeUI.SetActive(false);
    }

    void KillAllEnemies(InputAction.CallbackContext context)
    {
        _enemyManager.DestroyAll();
        QuitDebug();
    }

    void GotToNextLevel(InputAction.CallbackContext context)
    {
        _roomManager.LoadNextLevel();
        QuitDebug();
    }

    void GotToPreviousLevel(InputAction.CallbackContext context)
    {
        _roomManager.LoadPreviousLevel();
        QuitDebug();
    }

    void ReloadLevel(InputAction.CallbackContext context)
    {
        _roomManager.RestartRoom();
        QuitDebug();
    }

    void SkipTuto(InputAction.CallbackContext context)
    {
        if(_isOnTutorial && SceneManager.sceneCountInBuildSettings > 2)
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            Debug.LogWarning("Not in tutorial !");
        }

        QuitDebug();
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
            _playerActions.UI.Disable();
            _playerActions.Default.Enable();
        }
        else
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            _playerActions.Default.Disable();
            _playerActions.UI.Enable();
        }

        m_isGamePaused = !m_isGamePaused;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    
    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void EndGame()
    {
        _playerActions.UI.Enable();
        _playerActions.Default.Disable();
        _playerActions.Cooking.Disable();
        pauseMenu.SetActive(false);
        deathMenu.SetActive(false);
        victoryMenu.SetActive(true);
        PauseGame();
    }

    public void RestartLevel()
    {
        _playerActions.UI.Disable();
        _playerActions.Default.Enable();
        _playerActions.Cooking.Disable();
        pauseMenu.SetActive(false);
        deathMenu.SetActive(false);
        victoryMenu.SetActive(false);
        PauseGame();
        RoomManager.instance.RestartLevel();
    }

    #endregion

    public PlayerActions GetPlayerAction()
    {
        return _playerActions;
    }

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
