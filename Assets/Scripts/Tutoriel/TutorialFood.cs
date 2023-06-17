using Sirenix.OdinInspector;
using UnityEngine;

namespace Tutoriel
{
    public class TutorialFood : MonoBehaviour
    {
        [SerializeField] [Required]
        private TutorialManager tutorialManager;

        private void Awake()
        {
            if (tutorialManager == null)
                tutorialManager = GameObject.FindGameObjectWithTag("TutorialManager").GetComponent<TutorialManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                tutorialManager.SetPlayerHasIngredients(true);
        }
    }
}
