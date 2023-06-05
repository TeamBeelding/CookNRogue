using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class Cauldron_Trigger : MonoBehaviour
{
    [SerializeField] [Required("TutorialManager is required")]
    private TutorialManager tutorialManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered Cauldron trigger");
            
            if (tutorialManager.step == 0)
                tutorialManager.ApproachCauldron();
            
            if (tutorialManager.step == 1)
                StartCoroutine(ValidateIngredientRoutine());
        } 
    }
    
    IEnumerator ValidateIngredientRoutine()
    {
        yield return new WaitForSecondsRealtime(3);
        tutorialManager.ValidateIngredient();

    }
}
