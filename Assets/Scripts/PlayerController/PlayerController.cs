using UnityEngine.InputSystem;
using UnityEngine;
using UnityEditor.Rendering.LookDev;
using System.Collections;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("Character Properties")]
    [SerializeField]
    float m_rotationSpeed = 3f;
    [SerializeField]
    float m_moveSpeed = 5f;
    [SerializeField]
    float m_maxMoveSpeed = 5f;
    [SerializeField]
    float m_moveDrag = 1f;

    [Header("Init")]
    [SerializeField]
    Camera m_mainCamera;
    [SerializeField]
    GameObject m_model;

    public Vector3 PlayerAimDirection
    {
        get => _aimDirection;
    }

    PlayerActions _playerActions;

    Rigidbody _rb;
    Transform _relativeTransform;

    Vector2 moveInputValue;
    Vector2 aimInputValue;

    bool _isAiming = false;
    bool _isAimingOnMouse = false;
    Vector3 _aimDirection;
    #endregion

    void Start()
    {
        //Set Varialbes
        _rb = GetComponent<Rigidbody>();
        _relativeTransform = m_mainCamera.transform;

        //Set Input Actions Map
        _playerActions = new PlayerActions();
        _playerActions.Default_PlayerActions.Enable();
        //Set Events
        _playerActions.Default_PlayerActions.Shoot.performed += Shoot;
        _playerActions.Default_PlayerActions.Move.performed += Move_Performed;
        _playerActions.Default_PlayerActions.Move.canceled += Move_Canceled;
        _playerActions.Default_PlayerActions.Aim.performed += Aim_Performed;
        _playerActions.Default_PlayerActions.Aim.canceled+= Aim_Canceled;
    }

    private void Update()
    {
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
        if (context.performed)
        {
            GetComponent<PlayerAttack>().Shoot();
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
