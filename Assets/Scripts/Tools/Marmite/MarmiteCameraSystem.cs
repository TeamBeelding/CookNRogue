#if UNITY_EDITOR

using Sirenix.OdinInspector;
using UnityEngine;

public class MarmiteCameraSystem
{
    bool _zoom = false;

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
        _zoom = !_zoom;
        var cameraControllers = GameObject.FindObjectsOfType<CameraController>();
        if (cameraControllers == null)
        {
            Debug.LogAssertion("Camera Controller not found in scene");
            return;
        }
        foreach (var cameraController in cameraControllers)
        {
            cameraController.ScreenZoom(_zoom);
        }
    }

    private bool IsInEditor()
    {
        return !Application.isPlaying;
    }
}

#endif