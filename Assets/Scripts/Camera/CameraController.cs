using System.Collections;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    //Controler
    [Header("Camera Controller")]
    [Header("==========================================================================================================================================================================================================================")]
    // MainCamera Transform is needeed for the smooth movement of the camera.
    [SerializeField]
    private Transform MainCamera;
    // The child of main camera is needed for the shake of the camera thus called shake gimble
    [SerializeField]
    private Transform ShakeGimble;
    //Player target is initialised as the camera lock, it is the MAIN object (Player) which the camera follows.
    [SerializeField]
    private Transform CameraPlayerTarget;

    //Obstructions
    [Header("Obstructions")]
    [Header("==========================================================================================================================================================================================================================")]
    // opacity is the value which Chooses the amount of transparency in the walls when they are faded
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float opacity = 0.5f; // frome 0 to 1 (float) : default 0.5
    [SerializeField]
    // Tramsform Head, is GameObject holds an important role for the raycast and place holder for the meshrender. So it is never null;
    private Transform TransformHead;
    private MeshRenderer PlaceHolderMesh;
    private MeshRenderer ObstructionMesh;
    private RaycastHit Hit;


    //Shake
    [Header("Camera Shake")]
    [Header("==========================================================================================================================================================================================================================")]
    // How long will the shake last
    [SerializeField]
    private float ShakeDuration = 1f;
    // This decides the amount of shake over time
    [SerializeField]
    private AnimationCurve ShakeCurve;
    // Whether it is currently shaking or not
    public bool shake = false;

    //Shake
    [Header("Camera Zoom")]
    [Header("==========================================================================================================================================================================================================================")]
    // How long will the shake last
    [SerializeField]
    private float zoomDuration = 1f;
    [SerializeField]
    [Range(1f,10f)]
    private float zoomFactor = 2f;
    // This decides the amount of shake over time
    [SerializeField]
    private AnimationCurve ZoomCurve;
    // Whether it is currently shaking or not
    public bool zoom = false;

    //Smooth
    [Header("Camera Smooth")]
    // How fast dose the camera follow the player
    [SerializeField]
    private float smoothSpeed = 2.5f;
    float initialZoom;
    // If needed an addtional target can be added, in that case the camera make its way to that the target transform in a smooth way.
    // This can be usfull for bosses, special items in the room ect..
    [SerializeField]
    private Transform Target;

    void Start()
    {
        instance = this;
        // To get the child transform of the camera for the shake
        ShakeGimble = MainCamera.GetChild(0).GetComponent<Transform>();
        // Making sure the obstruction is initied and not null at the start for erros.
        PlaceHolderMesh = TransformHead.gameObject.GetComponent<MeshRenderer>();
        initialZoom = ShakeGimble.GetChild(0).GetComponent<Camera>().orthographicSize;
        ObstructionMesh = PlaceHolderMesh;
    }


    private void LateUpdate()
    {
        // In the update the camera will follow the player, unless the the target object has been added.
        // When target is removed (=null) Camera will start following the player again
        if (Target == null)
        {
            MainCamera.position = Vector3.Lerp(MainCamera.position, CameraPlayerTarget.position, smoothSpeed * Time.deltaTime);
        }
        else 
        {
            MainCamera.position = Vector3.Lerp(MainCamera.position, Target.position, smoothSpeed * Time.deltaTime);
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        // Main Function for the Transparency of the obstructions / walls.
        CameraTransparent();

        // initiate shake corountine if shake is true
        if (shake)
        {
            shake = false;
            StartCoroutine(Shake());
        }

        if (zoom)
        {
            zoom = false;
            StartCoroutine(Zoom());
        }
    }

    private void CameraTransparent()
    {
        // Draws a line from player to object.
        Debug.DrawLine(CameraPlayerTarget.position, TransformHead.position, Color.green);

        // Security measure for when and if obstruction = null, for instance when the room / level changes.
        if (!ObstructionMesh) 
        {
            ObstructionMesh = PlaceHolderMesh;
        }

        // Shooots a physics linecaste to check object obstructing the player from the camera
        if (Physics.Linecast(CameraPlayerTarget.position, TransformHead.position, out Hit))
        {
            ObstructionMesh.material.SetFloat("_Opacity", 1f);
            // if the object is tagged obstruction it turns transparent
            if (Hit.collider.CompareTag("Obstruction"))
            {
                ObstructionMesh = Hit.collider.GetComponent<MeshRenderer>();
                ObstructionMesh.material.SetFloat("_Opacity", opacity);
            }
        }
        else
        {
            //else turn it back to what it was.
            ObstructionMesh.material.SetFloat("_Opacity", 1f);
        }
    }

    public void ScreenShake() 
    {
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        // set a variable for the elapse
        float elapsedTime = 0f;
        // Getting start position of shake gimble in local space
        Vector3 startPosition = ShakeGimble.localPosition;
        while (elapsedTime < ShakeDuration)
        {
            // adding time to counter
            elapsedTime += Time.deltaTime;
            // strength of the curve at specific time. So strength over time (The y axis being strength, and x being time)
            float strength = ZoomCurve.Evaluate(elapsedTime / ShakeDuration);
            // changing the local postion of shake gimble inside the unit circle, so random position in a circle and adding the start position.
            ShakeGimble.localPosition = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }
        // reseting shakegimble to original position.
        ShakeGimble.localPosition = startPosition;
    }


    public void ScreenZoom()
    {
        StartCoroutine(Zoom());
    }

    IEnumerator Zoom()
    {
        // set a variable for the elapse
        float elapsedTime = 0f;
        //float normalZoom = ShakeGimble.GetChild(0).GetComponent<Camera>().orthographicSize;
        // Getting start position of shake gimble in local space
        //Vector3 startPosition = ShakeGimble.localPosition;
        while (elapsedTime < ShakeDuration)
        {
            // adding time to counter
            elapsedTime += Time.deltaTime;
            // strength of the curve at specific time. So strength over time (The y axis being strength, and x being time)
            float zoomspeed = ZoomCurve.Evaluate(elapsedTime / ShakeDuration);
            Debug.Log(zoomspeed);
            // changing the local postion of shake gimble inside the unit circle, so random position in a circle and adding the start position.

            ShakeGimble.GetChild(0).GetComponent<Camera>().orthographicSize = initialZoom * zoomspeed;
            yield return null;
        }
        // reseting shakegimble to original position.
        //ShakeGimble.localPosition = startPosition;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CameraController))]
public class CameraControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        //CameraController camera = (CameraController)target;

        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("TOOLS: ", "");
        GuiLine(1);

        if (GUILayout.Button("Shake"))
        {
            CameraController.instance.shake = true;
        }

        if (GUILayout.Button("Zoom"))
        {
            CameraController.instance.zoom = true;
        }
    }

    void GuiLine(int i_height = 1)
    {
        Rect rect = EditorGUILayout.GetControlRect(false, i_height);
        rect.height = i_height;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        EditorGUILayout.Separator();
    }
}
#endif
