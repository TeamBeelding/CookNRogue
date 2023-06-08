using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace Tutoriel
{
    public class CauldronTrigger : MonoBehaviour
    {
        [SerializeField] [Required("TutorialManager is required")]
        private TutorialManager tutorialManager;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player is close to the cauldron");
                tutorialManager.DisplayText();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                
            }
        }

        IEnumerator ValidateIngredientRoutine()
        {
            yield return new WaitForSecondsRealtime(3);
            tutorialManager.ValidateIngredient();
        }
    }
}
