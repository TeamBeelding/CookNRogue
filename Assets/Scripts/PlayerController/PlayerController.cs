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

    [SerializeField]
    PlayerAttack m_playerAttackScript;

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

    private bool _canMove = true;
    public bool CanMove
    {
        get => _canMove;
    }

    [SerializeField] ParticleSystem DashingParticles;
    [SerializeField] ParticleSystem _caramelParticles;

    private Vector3 m_dashDirection = Vector2.zero;

    [HideInInspector]
    public bool _isAiming = false;

    bool _isAimingOnMouse = false;
    Vector3 _aimDirection;
    Vector3 _correctedAimDirection;
    float _aimMagnitude;

    PlayerCookingInventory _inventoryScript;
    EnemyManager _enemyManager;
    PlayerCooking _cookingScript;
    CookBook _cookBookScript;

    bool _isLocked = false;
    private bool m_isGamePaused = false;
    public bool _cookSafety = false;

    bool _isInvicible = false;

    Collider _curInteractCollider = null;

    private enum playerStates
    {
        Default,
        Cooking,
        UI,
        CB
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

    [SerializeField] private bool _isOnTutorial = false;
    [SerializeField] private TutorialManager _tutorialManager;


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
        if (_isOnTutorial)
            _tutorialManager = GameObject.FindGameObjectWithTag("TutorialManager").GetComponent<TutorialManager>();

        _relativeTransform = m_mainCamera.transform;
        _inventoryScript = PlayerCookingInventory.Instance;
        _enemyManager = EnemyManager.Instance;
        _cookBookScript = CookBook.Instance;
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
        _playerActions.CookBook.Enable();
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
        _playerActions.Default.OpenCB.started += OpenCB;

        //Set Cooking Events
        _playerActions.Cooking.Cook.started += Cook_Canceled;
        _playerActions.Cooking.SelectIngredient.performed += SelectIngredient_Performed;
        _playerActions.Cooking.StartCrafting.performed += StartCraftingBullet;
        _playerActions.Cooking.IngredientSelector.performed += OnIngredientSelectorInput;
        _playerActions.Cooking.IngredientSelector.canceled += OnIngredientSelectorInputStop;
        _playerActions.Cooking.ChangeWheel.performed += OnChangeUIWheel;
        _playerActions.Cooking.Move.performed += Move_Performed;
        _playerActions.Cooking.Move.canceled += Move_Canceled;

        //Set UI Events
        _playerActions.UI.Pause.performed += OnPauseGame;
        _playerActions.UI.Return.performed += OnPauseGame;
        _playerActions.UI.SelectIngredient.performed += SelectTotemIngredient;
        _playerActions.UI.IngredientSelector.performed += OnTotemIngredientSelectorInput;
        _playerActions.UI.IngredientSelector.canceled += OnTotemIngredientSelectorInputStop;
        _playerActions.UI.ValidateIngredients.performed += OnValidateIngredients;

        //Set CB Events
        _playerActions.CookBook.Close.performed += CloseCB;
        _playerActions.CookBook.PrevPage.performed += CBPrevPage;
        _playerActions.CookBook.NextPage.performed += CBNextPage;

        //Set Debug Events
        _playerActions.Debug.EnterDebug.started += QuitDebug;
        _playerActions.Debug.KillAllEnemies.started += KillAllEnemies;
        _playerActions.Debug.ReloadLevel.started += ReloadLevel;
        _playerActions.Debug.GoToNextLevel.started += GotToNextLevel;
        _playerActions.Debug.GoToPreviousLevel.started += GotToPreviousLevel;
        _playerActions.Debug.SkipTuto.started += SkipTuto;

        _playerActions.Cooking.Disable();
        _playerActions.UI.Disable();
        _playerActions.CookBook.Disable();
        _playerActions.Debug.Disable();
        #endregion

        m_aimArrow.SetActive(false);
    }

    private void Update()
    {
        //Block player actions
        if (_isLocked)
        {
            return;
        }

        if (_curState == playerStates.Cooking)
        {
            playerAnimStatesScript.Marmite(false, true);
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

            //Deadzone
            if (_aimMagnitude > 0.2f)
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
            //Deadzone
            else if(_moveInputValue.magnitude > 0.2f)
            {
                //Move Player
                Vector3 moveInputDir = relativeForward * _moveInputValue.y + relativeRight * _moveInputValue.x;
                moveInputDir = moveInputDir.normalized;

                float speed = PlayerRuntimeData.GetInstance().data.BaseData.MoveSpeed * _moveInputValue.sqrMagnitude;

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
                //Deadzone
                else if(_moveInputValue.magnitude > 0.2f)
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
                Move(m_dashDirection, PlayerRuntimeData.GetInstance().data.BaseData.MoveSpeed * PlayerRuntimeData.GetInstance().data.BaseData.DashForce);
            }
        }
        #endregion

        #region Interact
        Collider[] interactableColliders = new Collider[3];

        //Get interactable objects in range
        int foundObjects = Physics.OverlapSphereNonAlloc(transform.position + m_model.transform.forward * 0.5f, PlayerRuntimeData.GetInstance().data.BaseData.InteractionRange, interactableColliders, m_interactionMask);
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
        if (!_canMove)
            return;

        _moveInputValue = context.ReadValue<Vector2>();
    }

    void Move_Canceled(InputAction.CallbackContext context)
    {
        _moveInputValue = Vector2.zero;
    }

    void Move(Vector3 direction, float speed)
    {
        //CHECK FOR BUTTERED SHOES BUFF
        if (PlayerRuntimeData.GetInstance().data.InventoryData.ButteredShoes)
            speed *= PlayerRuntimeData.GetInstance().data.InventoryData.ButteredShoesValue;

        _rb.AddForce(100f * speed * Time.deltaTime * direction, ForceMode.Force);
        _rb.drag = PlayerRuntimeData.GetInstance().data.BaseData.MoveDrag;

        if (!_isDashing && _rb.velocity.magnitude > PlayerRuntimeData.GetInstance().data.BaseData.MaxMoveSpeed)
        {
            _rb.velocity = new Vector3(direction.x, 0, direction.z) * PlayerRuntimeData.GetInstance().data.BaseData.MaxMoveSpeed;
        }

        CheckingIfPlayerIsMovingForTutorial();
    }

    private void CheckingIfPlayerIsMovingForTutorial()
    {
        if (!_tutorialManager)
            return;

        if (_isOnTutorial)
            _tutorialManager.SetIsMoving(true);
    }

    public void CheckingIfCookingIsDone()
    {
        if (!_tutorialManager)
            return;

        if (_isOnTutorial)
            _tutorialManager.SetIsCookingDone(true);
    }

    //Dash
    private void Dash(InputAction.CallbackContext context)
    {
        if (!_canMove)
            return;

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

        _isInvicible = true;
        DashingParticles.Play();

        if(PlayerRuntimeData.GetInstance().data.InventoryData.Caramel)
            _caramelParticles.Play();

        yield return new WaitForSeconds(PlayerRuntimeData.GetInstance().data.BaseData.DashDuration);
        _isDashing = false;
        m_dashDirection = Vector2.zero;
        DashingParticles.Stop();
        _caramelParticles.Stop();
        _isInvicible = false;

        //Aim Check;
        if (_isAiming)
        {
            m_aimArrow.SetActive(true);
        }
    }

    private IEnumerator IDashCooldown()
    {
        _dashOnCooldown = true;
        yield return new WaitForSeconds(PlayerRuntimeData.GetInstance().data.BaseData.DashCooldown);
        _dashOnCooldown = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isDashing)
            return;

        if (!PlayerRuntimeData.GetInstance().data.InventoryData.Caramel)
            return;

        if (!collision.transform.parent)
            return;

        Debug.Log("caramel damage");
        EnemyController controler;
        if (collision.transform.parent.TryGetComponent(out controler))
            controler.TakeDamage(PlayerRuntimeData.GetInstance().data.InventoryData.CaramelDamage);


    }
    #endregion

    #region Aim
    void Aim_Performed(InputAction.CallbackContext context)
    {
        if (_isDashing)
        {
            return;
        }

        var inputType = context.control.layout;

        //Check Input Device
        switch (inputType)
        {
            //M&K
            case ("Button"):
                {
                    _isAimingOnMouse = true;
                    _isAiming = true;

                    m_aimArrow.SetActive(true);
                    GetComponent<PlayerAttack>().SetIsShooting(true);
                    return;
                }
            //Gamepad
            case ("Stick"):
                {
                    _aimInputValue = context.ReadValue<Vector2>();
                    _aimMagnitude = _aimInputValue.magnitude;
                    //Deadzone
                    if (_aimMagnitude > 0.2f)
                    {
                        _isAiming = true;

                        m_aimArrow.SetActive(true);
                        GetComponent<PlayerAttack>().SetIsShooting(true);
                    }
                    else
                    {
                        _isAiming = false;

                        m_aimArrow.SetActive(false);
                        GetComponent<PlayerAttack>().SetIsShooting(false);
                    }
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

        GetComponent<PlayerAttack>().SetIsShooting(false);
    }

    #endregion

    #region Cooking
    void Cook_Performed(InputAction.CallbackContext context)
    {
        if (_cookSafety)
        {
            return;
        }

        if (_isAiming)
        {
            _aimInputValue = Vector2.zero;
            _aimMagnitude = 0f;

            m_aimArrow.SetActive(false);

            GetComponent<PlayerAttack>().SetIsShooting(false);
        }

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
        //Input state check
        if (_curState != playerStates.Default)
            return;

        playerAnimStatesScript._animator.SetBool("cooking", true);
        // playerAnimStatesScript.Marmite(false, true);

        //Set input state
        _playerActions.Default.Disable();
        _playerActions.Cooking.Enable();

        _curState = playerStates.Cooking;
    }

    public void StopCookingState()
    {
        //Input state check
        if (_curState != playerStates.Cooking)
            return;

        playerAnimStatesScript._animator.SetBool("cooking", false);
        // playerAnimStatesScript.Marmite(false, false);
        
        _Stop_SFX_Cook.Post(gameObject);

        //Set input state
        _playerActions.Cooking.Disable();
        _playerActions.Default.Enable();

        _curState = playerStates.Default;
    }

    void SelectIngredient_Performed(InputAction.CallbackContext context)
    {
        //Active Inventory Check
        if (_inventoryScript.IsDisplayed())
        {
            _inventoryScript.SelectIngredient();
        }
        else
        {
            //QTE
            _cookingScript.CheckQTE();
        }
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
    }

    public void QTEAppear()
    {
        if (_isOnTutorial)
            _tutorialManager.SetIsQTE(true);
    }

    public void QTEFailed()
    {
        if (_isOnTutorial)
            _tutorialManager.SetIsQTEFailed(true);
    }

    #endregion

    #region Other Actions

    void Shoot_Performed(InputAction.CallbackContext context)
    {
        return;
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
        return;
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

    public void AbleToMove(bool ableToMove)
    {
        _canMove = ableToMove;
    }

    void Rotate(Vector3 direction)
    {
        float newX = Mathf.Lerp(m_model.transform.forward.x, direction.x, Time.deltaTime * PlayerRuntimeData.GetInstance().data.BaseData.RotationSpeed);
        float newZ = Mathf.Lerp(m_model.transform.forward.z, direction.z, Time.deltaTime * PlayerRuntimeData.GetInstance().data.BaseData.RotationSpeed);
        m_model.transform.forward = new Vector3(newX, 0, newZ);
    }

    public void TakeDamage(float damage) 
    {
        if (_isInvicible)
            return;

        //Feedbacks
        CameraController.instance.ScreenShake();
        takeDamageTransition.LoadTransition();
        
        if (_tutorialManager)
            return;
            
        if (!_playerHealth.TakeDamage(1))
        {
            //Cooking cancel
            StopCookingState();

            Time.timeScale = 0f;
            _playerActions.Default.Disable();
            _playerActions.Cooking.Disable();
            _playerActions.UI.Enable();
            m_playerAttackScript.OnDeathReset();
            _cookingScript.Clear();
            pauseMenu.SetActive(false);
            victoryMenu.SetActive(false);
            deathMenu.SetActive(true);
        }
        else
        {
            StartCoroutine(InvicibleTimer());
        }
    }

    private IEnumerator InvicibleTimer()
    {
        _isInvicible = true;
        yield return new WaitForSeconds(PlayerRuntimeData.GetInstance().data.BaseData.InvicibilityDuration);
        _isInvicible = false;
    }
    
    public void Spawn(Transform spawnPoint)
    {
        transform.position = spawnPoint.position;
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
        QuitDebug();
    }

    void GotToPreviousLevel(InputAction.CallbackContext context)
    {
        QuitDebug();
    }

    void ReloadLevel(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        QuitDebug();
    }

    void SkipTuto(InputAction.CallbackContext context)
    {
        if(_isOnTutorial && SceneManager.sceneCountInBuildSettings > 2)
        {
            //AkSoundEngine.StopAll();
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

    void OnTotemIngredientSelectorInput(InputAction.CallbackContext context)
    {

        if (!TotemSelectorScript.instance)
            return;

        TotemSelectorScript.instance.OnSelectorInput(context.ReadValue<Vector2>().normalized);
    }

    void OnTotemIngredientSelectorInputStop(InputAction.CallbackContext context)
    {
        if (!TotemSelectorScript.instance)
            return;

        TotemSelectorScript.instance.OnSelectorInputStop();
    }

    void SelectTotemIngredient(InputAction.CallbackContext context)
    {
        TotemSelectorScript.instance.SelectIngredient();
    }

    void OnValidateIngredients(InputAction.CallbackContext context)
    {
        TotemSelectorScript.instance.ApplyTotemHeal();
        TotemMenuManager._instance.SwitchUI();
    }

    private void OnPauseGame(InputAction.CallbackContext callbackContext)
    {
        PauseGame();
    }

    public void PauseGame()
    {
        if (deathMenu.activeSelf || victoryMenu.activeSelf)
        {
            return;
        }

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
        Time.timeScale = 1;
        AkSoundEngine.StopAll();
        SceneManager.LoadScene(0);
    }
    
    
    public void RestartGame()
    {
        Time.timeScale = 1;
        AkSoundEngine.StopAll();
        RestartLevelFix.Instance.RestartLevel();
        SceneManager.LoadScene(0);
    }

    public void EndGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(false);
        deathMenu.SetActive(false);
        victoryMenu.SetActive(true);
        _playerActions.UI.Enable();
        _playerActions.Default.Disable();
        _playerActions.Cooking.Disable();
        AkSoundEngine.StopAll();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        deathMenu.SetActive(false);
        victoryMenu.SetActive(false);
        _playerActions.UI.Disable();
        _playerActions.Cooking.Disable();
        _playerActions.Default.Enable();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SkipTuto()
    {
        if (_isOnTutorial && SceneManager.sceneCountInBuildSettings > 2)
        {
            SceneManager.LoadScene(2);
            Time.timeScale = 1f;
        }
        else
        {
            Debug.LogWarning("Not in tutorial !");
        }
    }

    #endregion

    #region CB

    void OpenCB(InputAction.CallbackContext context)
    {
        //Input state check
        if (_curState != playerStates.Default)
            return;

        if (_isAiming)
        {
            _aimInputValue = Vector2.zero;
            _aimMagnitude = 0f;

            m_aimArrow.SetActive(false);

            GetComponent<PlayerAttack>().SetIsShooting(false);
        }

        //Set input state
        _playerActions.Default.Disable();
        _playerActions.CookBook.Enable();

        _curState = playerStates.CB;

        _cookBookScript.Show(true);
    }

    void CloseCB(InputAction.CallbackContext context)
    {
        //Input state check
        if (_curState != playerStates.CB)
            return;

        //Set input state
        _playerActions.CookBook.Disable();
        _playerActions.Default.Enable();

        _curState = playerStates.Default;

        _cookBookScript.Show(false);
    }

    void CBPrevPage(InputAction.CallbackContext context)
    {
        _cookBookScript.PrevPage();
    }

    void CBNextPage(InputAction.CallbackContext context)
    {
        _cookBookScript.NextPage();
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
