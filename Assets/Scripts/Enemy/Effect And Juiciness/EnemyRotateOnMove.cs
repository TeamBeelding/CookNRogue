using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotateOnMove : MonoBehaviour
{
    private EnemyController enemy;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateOnMove()
    {
        Vector3 localRotation = transform.localRotation.eulerAngles;
        
        if (enemy.IsMoving())
        {
            
        }
        else
        {
            transform.localRotation = Quaternion.Euler(localRotation);
        }
    }
}
