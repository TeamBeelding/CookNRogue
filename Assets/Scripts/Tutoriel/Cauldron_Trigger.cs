using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace Tutoriel
{
    public class Cauldron_Trigger : MonoBehaviour
    {
        [SerializeField] [Required("TutorialManager is required")]
        private TutorialManager tutorialManager;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                tutorialManager.DisplayText();
                
                // if (tutorialManager.Step == 0)
                //     tutorialManager.ApproachCauldron();
                //
                // if (tutorialManager.Step == 1)
                //     StartCoroutine(ValidateIngredientRoutine());
            } 
        }

        IEnumerator ValidateIngredientRoutine()
        {
            yield return new WaitForSecondsRealtime(3);
            tutorialManager.ValidateIngredient();
        }
    }
}
