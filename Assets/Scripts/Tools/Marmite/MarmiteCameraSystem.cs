using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class MarmiteCameraSystem
{
    [InfoBox("Can't use camera settings when not in game.", InfoMessageType.Warning, "IsInEditor")]
    [DisableInEditorMode]
    [Button(ButtonSizes.Large)]
    private void ShakeCamera()
    {
        var cameraControllers = GameObject.FindObjectsOfType<CameraController>();
        if (cameraControllers == null)
        {
            Debug.LogAssertion("Camera Controller not found in scene");
            return;
        }
        foreach (var cameraController in cameraControllers)
        {
            cameraController.ScreenShake();
        }
    }
    [DisableInEditorMode]
    [Button(ButtonSizes.Large)]
    private void ZoomCamera()
    {
        var cameraControllers = GameObject.FindObjectsOfType<CameraController>();
        if (cameraControllers == null)
        {
            Debug.LogAssertion("Camera Controller not found in scene");
            return;
        }
        foreach (var cameraController in cameraControllers)
        {
            cameraController.ScreenZoom();
        }
    }

    private bool IsInEditor()
    {
        return !Application.isPlaying;
    }
}
