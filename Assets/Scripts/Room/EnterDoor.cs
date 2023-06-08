using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDoor : MonoBehaviour
{
    [SerializeField]
    private GameObject m_door;
    [SerializeField]
    private MeshRenderer m_mesh;
    [SerializeField]
    private bool m_isOpenOnStart = false;

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
    private Material m_portalMaterial;
    [SerializeField]
    private Material m_rayMaterial;
    [SerializeField]
    private AnimationCurve m_rayCurve;

    private Material[] SkinnedMaterials;

    [SerializeField] GameObject _godRays;

    private void Start()
    {
        if (m_door != null)
        {
            _godRays.SetActive(true);

            if (m_mesh != null)
            {
                SkinnedMaterials = m_mesh.materials;
                SetDoor(0f);
                SetPortal(0f);
            }

            if (m_isOpenOnStart)
            {
                m_door.SetActive(false);
                SetDoor(1f);
                SetPortal(1f);
            }
            else
            {
                EnemyManager.Instance.OnAllEnnemiesKilled += StartOpenDoor;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !m_door.activeInHierarchy)
        {
            RoomManager.instance.LoadNextLevel();
        }
    }
    private void StartOpenDoor()
    {
        if (m_door != null)
        {
            StartCoroutine(IOpenDoor());
        }
    }

    IEnumerator IOpenDoor()
    {
        if (SkinnedMaterials.Length > 0)
        {
            float duration = m_portalAnimDuration + m_portalAnimOffset < m_doorOpeningDuration ? m_doorOpeningDuration : m_portalAnimDuration + m_portalAnimOffset;
            Debug.Log(duration);

            for (float f = 0f; f < duration; f += m_refreshRate)
            {
                SetDoor(f < m_doorOpeningDuration ? f : m_doorOpeningDuration);
                SetPortal(f <= m_portalAnimOffset ? 0f : f - m_portalAnimOffset);
                yield return new WaitForSeconds(m_refreshRate);
            }
            SetDoor(1f);
            SetPortal(1f);
        }

        //OPEN GNE GNOOOOOR

        m_door.SetActive(false);
    }

    void SetPortal(float value)
    {
        float progress = value / m_portalAnimDuration;
        m_portalMaterial.SetFloat("_EmissionIntensity", Mathf.Lerp(0.1f, 0.5f, m_portalIntensityCurve.Evaluate(progress)));
        m_portalMaterial.SetFloat("_Transition", m_portalAnimCurve.Evaluate(progress));
        Color tempColor = m_rayMaterial.color;
        tempColor.a = Mathf.Lerp(0f, 0.3f, m_rayCurve.Evaluate(progress));
        m_rayMaterial.color = tempColor;
    }

    void SetDoor (float value)
    {
        float progress = m_doorOpeningCurve.Evaluate(value / m_doorOpeningDuration);
        float animProgress = Mathf.Lerp(1f, 0f, progress);
        SkinnedMaterials[0].SetFloat("_GrowValue", animProgress);
    }
}
