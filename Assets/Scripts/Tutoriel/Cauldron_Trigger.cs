using System.Collections;
using UnityEngine;

public class Cauldron_Trigger : MonoBehaviour
{
    private TutorialManager _tutorialManager;

    private void Awake()
    {
        _tutorialManager = FindObjectOfType<TutorialManager>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (/*other.GetComponent<PlayerInventoryScript>().projectilesData.Count > 0*/ other.CompareTag("Player"))
        {
            if (_tutorialManager.step == 0)
                _tutorialManager.ApproachCauldron();
            
            if (_tutorialManager.step == 1)
            {
                StartCoroutine(ValidateIngredientRoutine());
            }
        } 
    }
    
    IEnumerator ValidateIngredientRoutine()
    {
        yield return new WaitForSecondsRealtime(3);
        _tutorialManager.ValidateIngredient();

    }
}
