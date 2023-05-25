using System.Collections;
using System.Collections.Generic;
using Enemy.Effect_And_Juiciness;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject sphere;
    public GameObject sphere2;
    public GameObject target;
    public float maxHeight = 10;
    public float speed = 2;

    private GameObject _sphere;
    
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Instantiate", 4);
        
        Invoke("Throw", 6);
    }

    private void Instantiate()
    {
        sphere = Instantiate(sphere, transform.position, Quaternion.identity);
    }

    private void Throw()
    {
        sphere.GetComponent<ThrowingEffect>().ThrowMinimoyz(target.transform, maxHeight, speed);
        sphere2.GetComponent<ThrowingEffect>().ThrowMinimoyz(target.transform, maxHeight, speed);
    }
}
