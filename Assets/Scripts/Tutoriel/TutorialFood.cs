using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Tutoriel;
using UnityEngine;

public class TutorialFood : MonoBehaviour
{
    [SerializeField] [Required]
    private TutorialManager _tutorialManager;

    private void Awake()
    {
        if (_tutorialManager == null)
            _tutorialManager = GameObject.FindGameObjectWithTag("TutorialManager").GetComponent<TutorialManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _tutorialManager.SetPlayerHasIngredients(true);
    }
}
