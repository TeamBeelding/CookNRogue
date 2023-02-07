using JetBrains.Annotations;
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
    //Controler
    [Header("Camera Controller")]
    [Header("==========================================================================================================================================================================================================================")]
    [SerializeField]
    private Transform MainCamera;
    [SerializeField]
    private Transform ShakeGimble;
    [SerializeField]
    private Transform CameraGimble;


    //Obstructions
    [Header("Obstructions")]
    [Header("==========================================================================================================================================================================================================================")]
    [SerializeField]
    private float opacity = 0.5f;
    [SerializeField]
    private Transform TransformHead;
    [SerializeField]
    private MeshRenderer ObstructionMesh;
    private RaycastHit Hit;

    //Shake
    [Header("Camera Shake")]
    [Header("==========================================================================================================================================================================================================================")]
    [SerializeField]
    private float ShakeDuration = 1f;
    [SerializeField]
    private AnimationCurve ShakeCurve;
    [SerializeField]
    private bool shake = false;

    //Smooth
    [Header("Camera Smooth")]
    [SerializeField]
    private float smoothSpeed = 0.005f;
    [SerializeField]
    private Transform Target;

    // Start is called before the first frame update
    void Start()
    {
        ShakeGimble = MainCamera.GetChild(0).GetComponent<Transform>();
    }


    private void LateUpdate()
    {
        if (Target == null)
        {
            MainCamera.position = Vector3.Lerp(MainCamera.position, CameraGimble.position, smoothSpeed * Time.deltaTime);
        }
        else 
        {
            MainCamera.position = Vector3.Lerp(MainCamera.position, Target.position, smoothSpeed * Time.deltaTime);
        }
    }


    // Update is called once per frame
    void Update()
    {



        CameraTransparent();

        if (shake)
        {
            shake = false;
            StartCoroutine(Shake());
        }
    }

    private void CameraTransparent()
    {
        Debug.DrawLine(CameraGimble.position, TransformHead.position, Color.green);

        if (Physics.Linecast(CameraGimble.position, TransformHead.position, out Hit))
        {
            ObstructionMesh.material.SetFloat("_Opacity", 1f);
            if (Hit.collider.CompareTag("Obstruction"))
            {
                Debug.Log(Hit.transform.name);
                ObstructionMesh = Hit.collider.GetComponent<MeshRenderer>();
                ObstructionMesh.material.SetFloat("_Opacity", opacity);
            }
        }
        else
        {
            ObstructionMesh.material.SetFloat("_Opacity", 1f);
        }
    }

    IEnumerator Shake()
    {
        float elapsedTime = 0f;
        Vector3 startPosition = ShakeGimble.localPosition;
        while (elapsedTime < ShakeDuration)
        {
            elapsedTime += Time.deltaTime;
            float strength = ShakeCurve.Evaluate(elapsedTime / ShakeDuration);
            ShakeGimble.localPosition = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }
        ShakeGimble.localPosition = startPosition;
    }
}

