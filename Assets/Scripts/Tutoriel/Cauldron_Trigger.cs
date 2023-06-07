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
                tutorialManager.DisplayText();
        }
        
        IEnumerator ValidateIngredientRoutine()
        {
            yield return new WaitForSecondsRealtime(3);
            tutorialManager.ValidateIngredient();
        }
    }
}
