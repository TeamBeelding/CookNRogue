using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugElvis : MonoBehaviour
{
    [SerializeField] private GameObject slime;
    [SerializeField] private Transform position;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
            Instantiate(slime, position.position, Quaternion.identity);
    }
}
