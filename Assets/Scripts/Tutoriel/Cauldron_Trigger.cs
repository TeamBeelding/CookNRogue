using Sirenix.OdinInspector.Editor.Validation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron_Trigger : MonoBehaviour
{
 
    TutorialManager tutorialManager;

    private void Awake()
    {
        tutorialManager = FindObjectOfType<TutorialManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (/*other.GetComponent<PlayerInventoryScript>().projectilesData.Count > 0*/ other.CompareTag("Player"))
        {
            if (tutorialManager.step == 0)
                tutorialManager.ApproachCauldron();
            
            if (tutorialManager.step == 1)
            {
                StartCoroutine(ValidateIngredientRoutine());
            }
        } 
    }
    IEnumerator ValidateIngredientRoutine()
    {
        yield return new WaitForSecondsRealtime(3);
        tutorialManager.ValidateIngredient();

    }
}
