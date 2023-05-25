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

    private Material[] SkinnedMaterials;

    [SerializeField]
    private float dissolveRate = 0.0125f;

    [SerializeField]
    private float refreshRate = 0.025f;

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
            float counter = 0;

            while (SkinnedMaterials[0].GetFloat("_DissolveAmount") < 1)
            {
                counter += dissolveRate;
                for (int i = 0; i < SkinnedMaterials.Length; i++)
                {
                    SkinnedMaterials[0].SetFloat("_DissolveAmount", counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
        //OPEN GNE GNOOOOOR
        m_door.SetActive(false);
    }
}
