using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Android;

public class DissolvingController : MonoBehaviour
{
    // Start is called before the first frame update

    public MeshRenderer Mesh;
    private Material[] SkinnedMaterials;

    [SerializeField]
    private float dissolveRate = 0.0125f;

    [SerializeField]
    private float refreshRate = 0.025f;

    public bool dissolve = false;

    void Start()
    {
        Mesh = gameObject.GetComponent<MeshRenderer>();

        if (Mesh != null) 
        { 
            SkinnedMaterials = Mesh.materials;
        }
    }

    private void StartDissolve() 
    {
        if (gameObject != null)
        {
            StartCoroutine(Dissolve());
        }
    }

    IEnumerator Dissolve() 
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
                yield return new WaitForSeconds (refreshRate);
            }
        }
    }
}
