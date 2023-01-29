using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static UnityEditor.Experimental.GraphView.Port;

public class CameraController : MonoBehaviour
{
    //UI

    enum CameraOptions // your custom enumeration
    {
        Default,
        Transparency
    };

    [SerializeField]
    private CameraOptions cameraOptions;

    [SerializeField]
    private Transform TransformCamera;
    //[SerializeField]
    //=private float clipingDistance = 1f;
    [SerializeField]
    private float opacity = 0.5f;

    private RaycastHit hit;
    private Vector3 cameraOffset;

    public MeshRenderer Obstruction;
    public Transform WallCheck;

    // Start is called before the first frame update
    void Start()
    {

        Obstruction = WallCheck.gameObject.GetComponent<MeshRenderer>();
        Obstruction.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraOptions.Equals(CameraOptions.Default))
        {
            //CameraCollision();
        }
        else 
        {
            CameraTransparent();
        }

    }



    //// Not needed just keeping it in case
    //private void CameraCollision() 
    //{
    //    Debug.DrawLine(WallCheck.position, transform.position, Color.green);

    //    if (Physics.Linecast(WallCheck.position, transform.position, out hit))
    //    {
    //        if (!hit.collider.gameObject.tag.Equals("Player") && hit.collider.gameObject.tag.Equals("Wall"))
    //        {
                
    //            TransformCamera.localPosition = Vector3.Lerp(TransformCamera.localPosition, new Vector3(
    //                                                    hit.point.x + clipingDistance - WallCheck.position.x,
    //                                                    hit.point.y - clipingDistance - WallCheck.position.y, 
    //                                                    hit.point.z + clipingDistance - WallCheck.position.z) + Vector3.up * 2, Time.deltaTime * 10); ;

    //            Debug.DrawLine(WallCheck.position, new Vector3(
    //                                                    hit.point.x,
    //                                                    hit.point.y,
    //                                                    hit.point.z), Color.red);

    //        }
    //    }
    //    else
    //    {
    //        TransformCamera.localPosition = Vector3.Lerp(TransformCamera.localPosition, cameraOffset, Time.deltaTime * 10);
    //    }
    //}


    private void CameraTransparent()
    {

        Debug.DrawLine(WallCheck.position, transform.position, Color.green);

        if (Physics.Linecast(WallCheck.position, transform.position, out hit))
        {
            Obstruction = hit.collider.gameObject.GetComponent<MeshRenderer>();
            Obstruction.material.SetFloat("_Opacity", 1f);


            if (!hit.collider.gameObject.tag.Equals("Player") && hit.collider.gameObject.tag.Equals("Wall"))
            {
                Obstruction.material.SetFloat("_Opacity", opacity);
            }

        }
        else
        {
            Obstruction.material.SetFloat("_Opacity", 1f);
        }

        //Debug.Log(Obstruction.material.name + " Color: " + Obstruction.material.GetFloat("_Opacity"));

        // Old wall is not equall to new wall then make old wall opaque again
    }



}
