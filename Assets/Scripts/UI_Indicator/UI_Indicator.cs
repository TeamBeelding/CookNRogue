using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Indicator : MonoBehaviour
{
    [SerializeField] Image _image;
    Transform _target;
    Camera _camera;
    [SerializeField] float borderThickness = 100;
    [SerializeField] bool activated = false;

    private void Start()
    {
        _target = transform;
        _camera = CameraController.instance.GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        if (!activated)
            return;

        Vector3 TargetPosition = _camera.WorldToScreenPoint(_target.position);
        Vector3 correctPosition = new Vector3(TargetPosition.x, TargetPosition.y, 0);
        
        bool IsOffScreen = correctPosition.x <= borderThickness || correctPosition.x >= Screen.width - borderThickness || correctPosition.y <= borderThickness || correctPosition.y >= Screen.height - borderThickness;
        if (IsOffScreen)
        {
            if (correctPosition.x <= borderThickness) correctPosition.x = borderThickness;
            if (correctPosition.x >= Screen.width - borderThickness) correctPosition.x = Screen.width - borderThickness;
            if (correctPosition.y <= borderThickness) correctPosition.y = borderThickness;
            if (correctPosition.y >= Screen.height - borderThickness) correctPosition.y = Screen.height - borderThickness;
            showIndicator();
        }
        else
            hideIndicator();


        Vector3 dir = correctPosition - new Vector3(Screen.width / 2, Screen.height / 2, 0);
        dir.Normalize();
        
        _image.rectTransform.right = dir;
        _image.rectTransform.position = Vector3.Lerp(_image.rectTransform.position, correctPosition, 0.25f);
    }

    public void showIndicator()
    {
        _image.enabled = true;
    }

    public void hideIndicator()
    {
        _image.enabled = false;
    }

    public void ActivateIndicator()
    {
        activated = true;
        _image.enabled = true;
        Debug.Log("activated!!!!");
    }

    public void DeActivateIndicator()
    {
        activated = false;
        _image.enabled = false;
    }

}
