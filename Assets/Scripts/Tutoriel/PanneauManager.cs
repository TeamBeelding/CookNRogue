using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PanneauManager : MonoBehaviour
{
    [SerializeField] private GameObject image;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (image)
                DisplayImage(true);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (image)
                DisplayImage(false);
        }
    }

    private void DisplayImage(bool value)
    {
        if (image)
            image.SetActive(value);
    }
}
