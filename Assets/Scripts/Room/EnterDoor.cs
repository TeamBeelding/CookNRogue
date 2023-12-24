using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Eflatun.SceneReference;
using Enemy;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using SceneReference = Eflatun.SceneReference.SceneReference;

public class EnterDoor : MonoBehaviour
{
    /* Instancié au chargement d'une nouvelle scène */
    private static DoorManager _doorManager;

    [Header("Room linker")]

    [SerializeField]
    [Probability("_scenesToLoad")]
    private float[] _sceneProbas;

    [SerializeField]
    private SceneReference[] _scenesToLoad;

    public Transform spawnPoint;

    [Space]
    [Header("Door settings")]
    [SerializeField]
    private GameObject m_door;
    [SerializeField]
    private MeshRenderer m_mesh;
    [SerializeField]
    private bool m_isOpenOnStart = false;
    [SerializeField]
    private bool m_isEntrance = false;

    [Space]

    [SerializeField]
    private float m_doorOpeningDuration = 2f;
    [SerializeField]
    private AnimationCurve m_doorOpeningCurve;
    [SerializeField]
    private float m_refreshRate = 0.025f;

    [Space]

    [SerializeField]
    private float m_portalAnimDuration;
    [SerializeField]
    private float m_portalAnimOffset;
    [SerializeField]
    private AnimationCurve m_portalAnimCurve;
    [SerializeField]
    private AnimationCurve m_portalIntensityCurve;
    [SerializeField]
    private MeshRenderer m_portalRenderer;
    [SerializeField]
    private ParticleSystemRenderer m_raysRenderer;
    [SerializeField]
    private AnimationCurve m_rayCurve;

    private Material[] SkinnedMaterials;
    private Material _portalMaterial;
    private Material _raysMaterial;

    [SerializeField] GameObject _godRays;

    bool _doorIsOpened = false;
    bool _raysAreActive = false;

    [SerializeField] private AK.Wwise.Event _Play_SFX_Door_Open;


    private void Awake()
    {
        if (_doorManager == null)
        {
            _doorManager = new GameObject("DoorManager").AddComponent<DoorManager>();
        }

        foreach (SceneReference sceneReference in _scenesToLoad)
        {
            if (sceneReference.State == SceneReferenceState.Unsafe)
            {
                Debug.LogError("Door has no valid level linked " + gameObject.name);
            }
        }

        if (m_door != null)
        {
            _godRays.SetActive(false);

            if (m_mesh != null)
            {
                SkinnedMaterials = m_mesh.materials;
                _portalMaterial = m_portalRenderer.material;
                _raysMaterial = m_raysRenderer.material;
                SetDoor(0f);
                SetPortal(0f);
            }

            if (m_isOpenOnStart)
            {
                SetDoor(m_doorOpeningDuration);
                SetPortal(m_portalAnimDuration);
            }
            else if (m_isEntrance)
            {
                if (m_door != null)
                {
                    m_door.SetActive(true);
                    m_door.GetComponent<Collider>().enabled = true;
                    StartCoroutine(ICloseDoor());
                }
            }
            else
            {
                EnemyManager.Instance.OnAllEnnemiesKilled += StartOpenDoor;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !m_door.GetComponent<Collider>().enabled)
        {
            if (_scenesToLoad.Length == 0)
            {
                Debug.LogError("No scene linked to this door");
                return;
            }

            float proba = Random.Range(0.0f, 1.0f);
            int chosenIndex = _scenesToLoad.Length - 1;
            for (int i = 0; i < _sceneProbas.Length; i++)
            {
                if (proba < _sceneProbas[i])
                {
                    chosenIndex = i;
                    break;
                }
            }


            var sceneToLoad = _scenesToLoad[chosenIndex];

            if (sceneToLoad.State == SceneReferenceState.Unsafe)
            {
                Debug.LogError("Scene reference is invalid");
                return;
            }

            PoolManager.Instance.DestroyAI();

            //SceneManager.LoadScene(sceneToLoad.BuildIndex);
            StartCoroutine(TransitionToNextScene(sceneToLoad.BuildIndex));
        }
    }

    private IEnumerator TransitionToNextScene(int sceneIndex)
    {
        RoomTransition.instance.TriggerTransitionAnimation();
        yield return new WaitForSeconds(RoomTransition.instance.GetAnimationDuration());
        SceneManager.LoadScene(sceneIndex);
    }
    private void StartOpenDoor()
    {
        if (m_door != null)
        {
            _Play_SFX_Door_Open.Post(gameObject);
            StartCoroutine(IOpenDoor());
        }
    }

    IEnumerator IOpenDoor()
    {
        if (SkinnedMaterials.Length > 0)
        {
            float duration = m_portalAnimDuration + m_portalAnimOffset < m_doorOpeningDuration ? m_doorOpeningDuration : m_portalAnimDuration + m_portalAnimOffset;

            for (float f = 0f; f < duration; f += m_refreshRate)
            {
                SetDoor(f < m_doorOpeningDuration ? f : m_doorOpeningDuration);
                SetPortal(f <= m_portalAnimOffset ? 0f : f - m_portalAnimOffset);
                yield return new WaitForSeconds(m_refreshRate);
            }
            SetDoor(m_doorOpeningDuration);
            SetPortal(m_portalAnimDuration);
        }
    }

    IEnumerator ICloseDoor()
    {
        if (SkinnedMaterials.Length > 0)
        {
            float duration = m_portalAnimDuration + m_portalAnimOffset < m_doorOpeningDuration ? m_doorOpeningDuration : m_portalAnimDuration + m_portalAnimOffset;

            for (float f = duration; f > 0; f -= m_refreshRate)
            {
                SetDoor(f < m_doorOpeningDuration ? f : m_doorOpeningDuration);
                SetPortal(f <= m_portalAnimOffset ? 0f : f - m_portalAnimOffset);
                yield return new WaitForSeconds(m_refreshRate);
            }
            SetDoor(0);
            SetPortal(0);
        }
    }

    void SetPortal(float value)
    {
        if(!m_isEntrance && value > 0)
        {
            m_door.GetComponent<Collider>().enabled = false;
        }

        float progress = value / m_portalAnimDuration;
        _portalMaterial.SetFloat("_EmissionIntensity", Mathf.Lerp(0.1f, 0.5f, m_portalIntensityCurve.Evaluate(progress)));
        _portalMaterial.SetFloat("_Transition", m_portalAnimCurve.Evaluate(progress));

        //Activate rays
        if (!_raysAreActive && m_rayCurve.Evaluate(progress) > 0)
        {
            _godRays.SetActive(true);
            _raysAreActive = true;
        }

        Color tempColor = _raysMaterial.color;
        tempColor.a = Mathf.Lerp(0f, 0.3f, m_rayCurve.Evaluate(progress));
        _raysMaterial.color = tempColor;
    }

    void SetDoor (float value)
    {
        if (!m_isEntrance && _doorIsOpened)
            return;

        float progress = m_doorOpeningCurve.Evaluate(value / m_doorOpeningDuration);
        float animProgress = Mathf.Lerp(1f, 0f, progress);
        SkinnedMaterials[0].SetFloat("_GrowValue", animProgress);

        //Open Door
        if(!m_isEntrance && value >= m_doorOpeningDuration)
        {
            m_door.SetActive(false);
            _doorIsOpened = true;
        }
    }
}
