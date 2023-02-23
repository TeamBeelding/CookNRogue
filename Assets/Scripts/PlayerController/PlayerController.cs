using UnityEngine.InputSystem;
using UnityEngine;

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
    LayerMask m_interactionMask;
    [SerializeField]
    TransitionController takeDamageTransition;

    public Vector3 PlayerAimDirection
    {
        get => _aimDirection;
    }

    PlayerActions _playerActions;

    Rigidbody _rb;
    Transform _relativeTransform;

    Vector2 moveInputValue;
    Vector2 aimInputValue;

    [HideInInspector]
    public bool _isAiming = false;
    bool _isAimingOnMouse = false;
    Vector3 _aimDirection;

    InventoryScript _scriptInventory;

    bool _isLocked = false;

    Collider _curInteractCollider = null;
    #endregion

    void Start()
    {
        //Set Varialbes
        _rb = GetComponent<Rigidbody>();
        _relativeTransform = m_mainCamera.transform;
        _scriptInventory = InventoryScript.instance;

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

        m_currentHealthValue = m_maxHealthValue;
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
            aimInputValue = Input.mousePosition - m_mainCamera.WorldToScreenPoint(transform.position);
        }

        //Null Input Check
        if (aimInputValue.magnitude > 0)
        {
            //Rotate Player Model
            Vector3 aimInputDir = relativeForward * aimInputValue.y + relativeRight * aimInputValue.x;
            aimInputDir = aimInputDir.normalized;
            aimInputDir = new Vector3(aimInputDir.x, 0, aimInputDir.z);

            Rotate(aimInputDir);

            //Set Aim Variables
            _aimDirection = aimInputDir;
        }
        #endregion

        #region Movement
        //Null Input Check
        if (moveInputValue.magnitude <= 0)
        {
            //Stop if input is null
            _rb.velocity = Vector3.zero;
        }
        else
        {
            //Move Player
            Vector3 moveInputDir = relativeForward * moveInputValue.y + relativeRight * moveInputValue.x;
            moveInputDir = moveInputDir.normalized;

            float speed = m_moveSpeed * moveInputValue.sqrMagnitude;

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

    void Rotate(Vector3 direction)
    {
        float newX = Mathf.Lerp(m_model.transform.forward.x, direction.x, Time.deltaTime * m_rotationSpeed);
        float newZ = Mathf.Lerp(m_model.transform.forward.z, direction.z, Time.deltaTime * m_rotationSpeed);
        m_model.transform.forward = new Vector3 (newX, 0, newZ);
    }

    #region Movement
    void Move_Performed(InputAction.CallbackContext context)
    {
        moveInputValue = context.ReadValue<Vector2>();
    }

    void Move_Canceled(InputAction.CallbackContext context)
    {
        moveInputValue = Vector2.zero;
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
                    aimInputValue = context.ReadValue<Vector2>();
                    return;
                }
        }
    }

    void Aim_Canceled(InputAction.CallbackContext context)
    {
        //Reset
        _isAiming = false;
        _isAimingOnMouse = false;
        aimInputValue = Vector2.zero;
    }
    #endregion 

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

    void Craft(InputAction.CallbackContext context)
    {
        _scriptInventory.Craft();
    }

    public void Lock(bool isLocked)
    {
        _isLocked = isLocked;
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

    private void OnDrawGizmos()
    {
        if (_isAiming)
        {
            //Draw Aim Assist
        }

        Vector3 target = transform.position + _aimDirection * 2f;
        Gizmos.DrawLine(transform.position, target);
    }


}
