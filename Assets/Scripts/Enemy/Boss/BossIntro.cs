using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIntro : MonoBehaviour
{
    CameraController _cameraController;
    BossController _bossController;
    PlayerController _playerController;

    [SerializeField] Animator _barAnimator;
    
    [SerializeField] Animator _healthbarAnimator;
    public Animator healthbarAnimator { get { return _healthbarAnimator; } }

    [SerializeField] float _introDelay = 1f;
    [SerializeField] float _introDuration = 2f;

    private void Start()
    {
        _playerController = PlayerController.Instance;
        _cameraController = CameraController.instance;
        _bossController = GetComponent<BossController>();
        StartCoroutine(Intro());
    }

    IEnumerator Intro()
    {
        PlayerAttack pattack = PlayerController.Instance.transform.GetComponent<PlayerAttack>();
        pattack.PauseAmmoTimer = true;
        _playerController.enabled = false;
        _bossController.enabled = false;
        yield return new WaitForSecondsRealtime(_introDelay);

        _barAnimator.Play("Bar_Intro");
        _cameraController.ChangeTarget(transform);
        yield return new WaitForSecondsRealtime(_introDuration);

        _cameraController.ChangeTarget(_cameraController.transform.parent);
        _playerController.enabled = true;
        _bossController.enabled = true;
        _barAnimator.Play("Bar_Exit");

        _healthbarAnimator.Play("Boss_HealthBar_Intro");
        pattack.PauseAmmoTimer = false;
    }
}
