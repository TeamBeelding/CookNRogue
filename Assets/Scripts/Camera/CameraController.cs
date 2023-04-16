using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    //Controler
    [Header("Camera Controller")]
    [Header("==========================================================================================================================================================================================================================")]
    // m_mainCamera Transform is needeed for the smooth movement of the camera.
    [SerializeField]
    private Transform m_mainCamera;
    // The child of main camera is needed for the _shake of the camera thus called _shake gimble
    private Transform _shakeGimble;
    //Player target is initialised as the camera lock, it is the MAIN object (Player) which the camera follows.
    [SerializeField]
    private Transform m_cameraPlayerTarget; 

    [SerializeField]
    private Vector3 m_offsetCoord;
    [SerializeField]
    private Quaternion m_offsetRotation;

    //Obstructions
    [Header("Obstructions")]
    [Header("==========================================================================================================================================================================================================================")]
    // m_opacity is the value which Chooses the amount of transparency in the walls when they are faded
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_opacity = 0.5f; // frome 0 to 1 (float) : default 0.5
    [SerializeField]
    // Tramsform Head, is GameObject holds an important role for the raycast and place holder for the meshrender. So it is never null;
    private Transform m_transformHead;

    private MeshRenderer _placeHolderMesh;
    private MeshRenderer _obstructionMesh;
    private RaycastHit _hit;


    //Shake
    [Header("Camera Shake")]
    [Header("==========================================================================================================================================================================================================================")]
    // How long will the _shake last
    [SerializeField]
    private float m_shakeDuration = 1f;
    // This decides the amount of _shake over time
    [SerializeField]
    private AnimationCurve m_shakeCurve;
    // Whether it is currently shaking or not
    [HideInInspector]
    public bool _shake = false;

    //Shake
    [Header("Camera Zoom")]
    [Header("==========================================================================================================================================================================================================================")]
    // How long will the _shake last
    [SerializeField]
    private float m_zoomDuration = 1f;
    [SerializeField]
    [Range(1f,10f)]
    private float m_zoomFactor = 2f;
    // This decides the amount of _shake over time
    [SerializeField]
    private AnimationCurve m_zoomCurve;
    // Whether it is currently shaking or not
    [HideInInspector]
    public bool _zoom = false;

    //Smooth
    [Header("Camera Smooth")]
    [Header("==========================================================================================================================================================================================================================")]
    // How fast dose the camera follow the player
    [SerializeField]
    private float m_smoothSpeed = 2.5f;
    float _initialZoom;
    // If needed an addtional target can be added, in that case the camera make its way to that the target transform in a smooth way.
    // This can be usfull for bosses, special items in the room ect..
    [SerializeField]
    private Transform m_target;

    [Header("Aim Smoothing")]
    [Header("==========================================================================================================================================================================================================================")]
    [SerializeField]
    private float m_cameraAimDistance;
    private float _currentMagnitude = 0;

    void Start()
    {
        instance = this;

        //m_mainCamera.position += m_offsetCoord;
        m_mainCamera.rotation *= m_offsetRotation;
        m_mainCamera.position += m_offsetCoord;

        // To get the child transform of the camera for the _shake
        _shakeGimble = m_mainCamera.GetChild(0).GetComponent<Transform>();
        // Making sure the obstruction is initied and not null at the start for erros.
        _placeHolderMesh = m_transformHead.gameObject.GetComponent<MeshRenderer>();

        _initialZoom = _shakeGimble.GetChild(0).GetComponent<Camera>().orthographicSize;
        _obstructionMesh = _placeHolderMesh;
    }

    private void LateUpdate()
    {
        // In the update the camera will follow the player, unless the the target object has been added.
        // When target is removed (=null) Camera will start following the player again
        if (m_target == null)
        {
            _currentMagnitude = m_cameraPlayerTarget.gameObject.GetComponent<PlayerController>().PlayerAimMagnitude;
            m_mainCamera.position = Vector3.Lerp(m_mainCamera.position, m_cameraPlayerTarget.position + (m_cameraAimDistance * m_cameraPlayerTarget.gameObject.GetComponent<PlayerController>().PlayerAimDirection) * _currentMagnitude + m_offsetCoord, m_smoothSpeed * Time.deltaTime);
        }
        else 
        {
            m_mainCamera.position = Vector3.Lerp(m_mainCamera.position, m_target.position + m_offsetCoord, m_smoothSpeed * Time.deltaTime);
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        // Main Function for the Transparency of the obstructions / walls.
        CameraTransparent();
        // initiate _shake corountine if _shake is true
        if (_shake)
        {
            _shake = false;
            StartCoroutine(IShake());
        }

        if (_zoom)
        {
            _zoom = false;
            StartCoroutine(IZoom());
        }
    }

    private void CameraTransparent()
    {
        Debug.DrawLine(m_transformHead.position + m_offsetCoord, m_transformHead.position, Color.magenta);
        Debug.DrawLine(m_mainCamera.position, m_cameraPlayerTarget.position, Color.red);

        // Security measure for when and if obstruction = null, for instance when the room / level changes.
        if (!_obstructionMesh) 
        {
            _obstructionMesh = _placeHolderMesh;
        }

        // Shooots a physics linecaste to check object obstructing the player from the camera
        if (Physics.Linecast(m_transformHead.position + m_offsetCoord, m_transformHead.position, out _hit))
        {
            _obstructionMesh.material.SetFloat("_Opacity", 1f);
            // if the object is tagged obstruction it turns transparent
            if (_hit.collider.CompareTag("Obstruction"))
            {
                if (!_hit.collider.GetComponent<MeshRenderer>())
                {
                    _obstructionMesh = _hit.collider.GetComponentInChildren<MeshRenderer>();
                    _obstructionMesh.material.SetFloat("_Opacity", m_opacity);
                }
                else
                {
                    _obstructionMesh = _hit.collider.GetComponent<MeshRenderer>();
                    _obstructionMesh.material.SetFloat("_Opacity", m_opacity);
                }
            }
        }
        else
        {
            //else turn it back to what it was.
            _obstructionMesh.material.SetFloat("_Opacity", 1f);
        }
    }

    public void ScreenShake() 
    {
        StartCoroutine(IShake());
    }

    IEnumerator IShake()
    {
        // set a variable for the elapse
        float elapsedTime = 0f;
        // Getting start position of _shake gimble in local space
        Vector3 startPosition = _shakeGimble.localPosition;
        while (elapsedTime < m_shakeDuration)
        {
            // adding time to counter
            elapsedTime += Time.deltaTime;
            // strength of the curve at specific time. So strength over time (The y axis being strength, and x being time)
            float strength = m_shakeCurve.Evaluate(elapsedTime / m_shakeDuration);
            // changing the local postion of _shake gimble inside the unit circle, so random position in a circle and adding the start position.
            _shakeGimble.localPosition = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }
        // reseting shakegimble to original position.
        _shakeGimble.localPosition = startPosition;
    }


    public void ScreenZoom()
    {
        StartCoroutine(IZoom());
    }

    IEnumerator IZoom()
    {
        // set a variable for the elapse
        float elapsedTime = 0f;
        // Getting start position of _shake gimble in local space
        while (elapsedTime < m_zoomDuration)
        {
            // adding time to counter
            elapsedTime += Time.deltaTime;
            // strength of the curve at specific time. So strength over time (The y axis being strength, and x being time)
            float zoomspeed = m_zoomCurve.Evaluate(elapsedTime / m_zoomDuration);
            Debug.Log(zoomspeed);
            // changing the local postion of _shake gimble inside the unit circle, so random position in a circle and adding the start position.
            _shakeGimble.GetChild(0).GetComponent<Camera>().orthographicSize = _initialZoom * zoomspeed;
            yield return null;
        }
    }
}
