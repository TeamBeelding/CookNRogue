using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private Transform TransformCamera;
    private float clipingDistance = 0.1f;

    private RaycastHit hit;
    private Vector3 cameraOffset;

    public MeshRenderer Obstruction;
    public Transform WallCheck;

    // Start is called before the first frame update
    void Start()
    {
        clipingDistance += 1;

        cameraOffset = TransformCamera.localPosition;

        Obstruction = WallCheck.gameObject.GetComponent<MeshRenderer>();
        Obstruction.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    }

    // Update is called once per frame
    void Update()
    {
        //CameraCollision();
        CameraTransparent();


    }

    private void CameraCollision() 
    {
        Debug.DrawLine(WallCheck.position, transform.position * clipingDistance, Color.green);

        if (Physics.Linecast(WallCheck.position, transform.position * clipingDistance, out hit))
        {
            Debug.Log("HELLLLO");
            if (!hit.collider.gameObject.tag.Equals("Player"))
            {
                TransformCamera.localPosition = new Vector3(
                                                        hit.point.x + clipingDistance - WallCheck.position.x,
                                                        hit.point.y + clipingDistance - WallCheck.position.y, 
                                                        hit.point.z + clipingDistance - WallCheck.position.z );
                //TransformCamera.localPosition = transform.position - hit.point.normalized;
            }
        }
        else
        {
            TransformCamera.localPosition = cameraOffset;
        }
    }


    private void CameraTransparent()
    {
        RaycastHit hit;

        Debug.DrawRay(WallCheck.position, transform.position, Color.red, 0.1f);

        if (Physics.Raycast(WallCheck.position, transform.position, out hit))
        {
            //transform = hit.collider.gameObject;
            Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

            if (hit.collider.gameObject.tag == "Wall")
            {
                Obstruction.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

            }

        }
        TransformCamera.localPosition = cameraOffset;
    }

}
