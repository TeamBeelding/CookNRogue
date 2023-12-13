using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIntro : MonoBehaviour
{
    CameraController _cameraController;


    private void Start()
    {
        _cameraController = CameraController.instance;
        StartCoroutine(Intro());
    }

    IEnumerator Intro()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        _cameraController.ChangeTarget(transform);
        yield return new WaitForSecondsRealtime(2f);
        _cameraController.ChangeTarget(_cameraController.transform.parent);
    }
}
