using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDoor : MonoBehaviour
{
    public GameObject door;
    public MeshRenderer Mesh;
    private Material[] SkinnedMaterials;

    [SerializeField]
    private float dissolveRate = 0.0125f;

    [SerializeField]
    private float refreshRate = 0.025f;

    private void Start()
    {
        Mesh = door.GetComponent<MeshRenderer>();

        if (Mesh != null)
        {
            SkinnedMaterials = Mesh.materials;
        }

        EnemyManager.Instance.OnAllEnnemiesKilled += StartOpenDoor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !door.activeInHierarchy)
        {
            RoomManager.instance.LoadRandomLevel();
        }
    }
    private void StartOpenDoor()
    {
        if (door != null)
        {
            StartCoroutine(OpenDoor());
        }
    }

    IEnumerator OpenDoor()
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
        door.SetActive(false);
    }
}
