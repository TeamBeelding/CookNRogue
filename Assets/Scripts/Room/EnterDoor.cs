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

    [SerializeField]
    private float m_doorOpeningDuration = 2f;
    [SerializeField]
    private AnimationCurve m_doorOpeningCurve;
    [SerializeField]
    private float m_refreshRate = 0.025f;

    private Material[] SkinnedMaterials;

    private void Start()
    {
        if (m_door != null)
        {
            if (m_mesh != null)
            {
                SkinnedMaterials = m_mesh.materials;
            }

            if (m_isOpenOnStart)
            {
                m_door.SetActive(false);
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
            for (float f = 0; f < m_doorOpeningDuration; f += m_refreshRate)
            {
                float progress = f / m_doorOpeningDuration;
                float animProgress = Mathf.Lerp(1, 0, m_doorOpeningCurve.Evaluate(progress));
                SkinnedMaterials[0].SetFloat("_GrowValue", animProgress);
                yield return new WaitForSeconds(m_refreshRate);
            }
            SkinnedMaterials[0].SetFloat("_GrowValue", 0);
        }
        //OPEN GNE GNOOOOOR
        m_door.SetActive(false);
    }
}
