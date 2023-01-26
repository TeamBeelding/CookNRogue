using UnityEngine.InputSystem;
using UnityEngine;
using UnityEditor.Rendering.LookDev;

public class PlayerController : MonoBehaviour
{
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

    PlayerActions _playerActions;

    Rigidbody _rb;
    Transform _relativeTransform;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _relativeTransform = m_mainCamera.transform;

        _playerActions = new PlayerActions();
        _playerActions.Default_PlayerActions.Enable();
    }

    private void Update()
    {
        #region Movement
        //Movement Inputs
        Vector2 inputValue = _playerActions.Default_PlayerActions.Move.ReadValue<Vector2>();

        if (inputValue.magnitude <= 0)
        {
            //Stop if input is null
            _rb.velocity = Vector3.zero;
        }
        else
        {
            //Inputs relative to camera
            Vector3 relativeForward = _relativeTransform.forward + _relativeTransform.up;
            Vector3 relativeRight = _relativeTransform.right;
            relativeForward.y = 0;
            relativeRight.y = 0;
            relativeForward.Normalize();
            relativeRight.Normalize();

            //Move Player
            Vector3 InputDir = relativeForward * inputValue.y + relativeRight * inputValue.x;
            InputDir = InputDir.normalized;

            //Debug.Log(inputValue);

            float speed = m_moveSpeed * inputValue.sqrMagnitude;

            Rotate(InputDir);
            Move(InputDir, speed);
        }


        #endregion
    }

    void Rotate(Vector3 direction)
    {
        m_model.transform.forward = Vector3.Slerp(m_model.transform.forward, direction, Time.deltaTime * m_rotationSpeed);
    }

    void Move(Vector3 direction, float speed)
    {
        _rb.AddForce(direction * speed * Time.deltaTime * 100f, ForceMode.Force);
        _rb.drag = m_moveDrag;

        if (_rb.velocity.magnitude > m_maxMoveSpeed)
        {
            _rb.velocity = new Vector3(direction.x, _rb.velocity.y, direction.z) * m_maxMoveSpeed;
        }
    }
}
