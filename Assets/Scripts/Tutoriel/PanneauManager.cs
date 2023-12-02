using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PanneauManager : MonoBehaviour
{
    [SerializeField] private GameObject image;
    [SerializeField] private float range = 5f;

    private void Update()
    {
        if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) <= range)
        {
            DisplayImage(true);
        }
        else
        {
            DisplayImage(false);
        }
    }

    private void DisplayImage(bool value)
    {
        if (image)
            image.SetActive(value);
    }
}
