using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tutoriel
{
    public class TutorialManager : MonoBehaviour
    {
        public enum TutorialStep
        {
            Move,
            ApproachCauldron,
            ValidateIngredient,
            UnValidateIngredient,
            CookMenuOpen,
            FoodSpawn,
            LowTimeSpeed,
            NormalTimeSpeed,
            Aiming,
            Shooting,
            End
        }
        
        [SerializeField] private TutorialStep _tutorialStep;
        
        [SerializeField]
        private bool isMoving = false;
        [SerializeField]
        private bool isCooking = false;
        [SerializeField]
        private bool isAiming = false;
        [SerializeField]
        private bool isShooting = false;
    
        [SerializeField] 
        private GameObject[] ingredients;
        [SerializeField]
        private GameObject enemy;
    
        public bool GetIsMoving() => isMoving;
        public bool GetIsCooking() => isCooking;
        public bool GetIsAiming() => isAiming;
        public bool GetIsShooting() => isShooting;

        private int _step;

        [SerializeField] private GameObject cauldron;
        [SerializeField] private string[] dialogue;
        private DialogueBox _dialogueBox;
        private string textToDisplay = "";

        private void Start()
        {
            _dialogueBox = GameObject.FindObjectOfType<DialogueBox>();
            
            SetTutorialState(TutorialStep.Move);
        }

        private void Update()
        {
            StateManagement();
        }

        // public int Step => _step;

        public TutorialStep GetTutorialState()
        {
            return _tutorialStep;
        }
        
        private void SetTutorialState(TutorialStep tutorialStep)
        {
            _tutorialStep = tutorialStep;
        }

        private void StateManagement()
        {
            switch (_tutorialStep)
            {
                case TutorialStep.Move:
                    Move();
                    break;
                case TutorialStep.ApproachCauldron:
                    ApproachCauldron();
                    break;
                case TutorialStep.ValidateIngredient:
                    break;
                case TutorialStep.UnValidateIngredient:
                    break;
                case TutorialStep.CookMenuOpen:
                    break;
                case TutorialStep.FoodSpawn:
                    break;
                case TutorialStep.LowTimeSpeed:
                    break;
                case TutorialStep.NormalTimeSpeed:
                    break;
                case TutorialStep.Aiming:
                    break;
                case TutorialStep.Shooting:
                    break;
                case TutorialStep.End:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void DisplayText()
        {
            if (string.IsNullOrEmpty(textToDisplay))
                return;
            
            string[] dialogueToDisplay = { textToDisplay };
            _dialogueBox.DisplayDialogueText(dialogueToDisplay, cauldron.transform);
        }

        private void Move()
        {
            if (GetIsMoving())
                SetTutorialState(TutorialStep.ApproachCauldron);
        }

        private void ApproachCauldron()
        {
            _step = 1;
            textToDisplay = dialogue[0];
            // string[] dialogueToDisplay = { dialogue[0] };
            // _dialogueBox.DisplayDialogueText(dialogueToDisplay, cauldron.transform);
        }

        public void ValidateIngredient()
        {
            _step = 2;
            string[] dialogueToDisplay = { dialogue[2] };
            _dialogueBox.DisplayDialogueText(dialogueToDisplay, cauldron.transform);

            FindObjectOfType<PlayerController>().GetPlayerAction().Default.Cook.performed += this.CookMenuOpen;
        }

        public void UnValidateIngredient()
        {
            _step = 1;
            string[] dialogueToDisplay = { dialogue[1] };
            _dialogueBox.DisplayDialogueText(dialogueToDisplay, cauldron.transform);
        }


        public void CookMenuOpen(InputAction.CallbackContext context)
        {
            FindObjectOfType<PlayerController>().GetPlayerAction().Default.Cook.performed -= this.CookMenuOpen;
            _step = 3;
            string[] dialogueToDisplay = { dialogue[3] };
            Time.timeScale = 0;
            _dialogueBox.DisplayDialogueText(dialogueToDisplay, cauldron.transform);
        }

        public void FoodSpawn()
        {
            foreach (GameObject food in ingredients)
            {
                if (food == null)
                    continue;
            
                food.SetActive(true);
            }
        }

        public void LowTimeSpeed()
        {
            Time.timeScale = 0.5f;
        }
    
        public void NormalTimeSpeed()
        {
            Time.timeScale = 1f;
        }
    
        public void SpawnEnemy()
        {
            if (enemy == null)
                return;
        
            enemy.SetActive(true);
        }
    
        public void ShowInteractButton()
        {
        
        }
    }
}
