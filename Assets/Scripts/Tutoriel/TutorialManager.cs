using System;
using System.Collections;
using Dialogues;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Tutoriel
{
    public class TutorialManager : MonoBehaviour
    {
        public enum TutorialStep
        {
            Move,
            ApproachCauldron,
            ApproachCloser,
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
        
        [SerializeField] private TutorialStep tutorialStep;
        [SerializeField] private float distanceToCauldron = 2f;
        [SerializeField] private float distanceCloseEnough = 1f;
        private bool _isCloseEnough = false;
        
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

        private Coroutine _coroutineState;
        private IEnumerator _enumeratorState;
    
        public bool GetIsMoving() => isMoving;
        public bool GetIsCooking() => isCooking;
        public bool GetIsAiming() => isAiming;
        public bool GetIsShooting() => isShooting;

        private int _step;

        [SerializeField] private GameObject cauldron;
        [SerializeField] private string[] dialogue;
        private DialogueBox _dialogueBox;
        private string _textToDisplay = "";
        private GameObject _player;

        private void Start()
        {
            _dialogueBox = GameObject.FindObjectOfType<DialogueBox>();
            _player = GameObject.FindGameObjectWithTag("Player");
            
            SetTutorialState(TutorialStep.Move);
        }

        public TutorialStep GetTutorialState() => tutorialStep;
        
        private void SetTutorialState(TutorialStep tutorialStep)
        {
            StopAllCoroutines();
            
            // StopCoroutine(_coroutineState);

            this.tutorialStep = tutorialStep;

            Debug.Log("<color=red>Current tutorial step: " + tutorialStep + "</color>");
            
            StateManagement();
        }

        private void StateManagement()
        {
            switch (tutorialStep)
            {
                case TutorialStep.Move:
                    Move();
                    break;
                case TutorialStep.ApproachCauldron:
                    ApproachCauldron();
                    break;
                case TutorialStep.ApproachCloser:
                    ApproachMoreClose();
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
        
        private void DistanceCheck()
        {
            if (tutorialStep != TutorialStep.ApproachCauldron) return;
            
            if (Vector3.Distance(_player.transform.position, cauldron.transform.position) <= distanceToCauldron)
            {
                if (Vector3.Distance(_player.transform.position, cauldron.transform.position) <= distanceCloseEnough)
                    SetTutorialState(TutorialStep.ApproachCloser);
            }
        }
        
        public void DisplayText()
        {
            if (string.IsNullOrEmpty(_textToDisplay))
                return;
            
            // string[] dialogueToDisplay = { _textToDisplay };
            // _dialogueBox.DisplayDialogueText(dialogueToDisplay, cauldron.transform);
            
            // _dialogueBox.DisplayText(_textToDisplay, cauldron.transform);
        }

        private void Move()
        {
            // if (_coroutineState == null)
            //     _coroutineState = StartCoroutine(IChekingMovement());

            // _coroutineState = StartCoroutine(IChekingMovement());

            StartCoroutine(IChekingMovement());
            
            IEnumerator IChekingMovement()
            {
                while (tutorialStep == TutorialStep.Move)
                {
                    if (isMoving)
                    {
                        Debug.Log($"<color=green>Exit state {GetTutorialState()}</color>");
                        SetTutorialState(TutorialStep.ApproachCauldron);
                    }

                    yield return null;
                }
            }
        }
        
        public void SetIsMoving(bool value)
        {
            isMoving = value;
        }

        private void ApproachCauldron()
        {
            // _step = 1;
            
            // if (_coroutineState == null)
            //     _coroutineState = StartCoroutine(IApproaching());

            // _coroutineState = StartCoroutine(IApproaching());

            StartCoroutine(IApproaching());
            
            IEnumerator IApproaching()
            {
                while (tutorialStep == TutorialStep.ApproachCauldron)
                {
                    Debug.Log("ApproachCauldron");

                    if (Vector3.Distance(_player.transform.position, cauldron.transform.position) <= distanceToCauldron)
                    {
                        if (Vector3.Distance(_player.transform.position, cauldron.transform.position) <= distanceCloseEnough)
                        {
                            Debug.Log($"<color=green>Exit state {GetTutorialState()}</color>");
                            SetTutorialState(TutorialStep.ApproachCloser);
                        }
                        else
                            _textToDisplay = dialogue[0];
                    }
                    
                    _dialogueBox.DisplayText(_textToDisplay, cauldron.transform);
                    yield return null;
                }
            }
        }
        
        private void ApproachMoreClose()
        {
            // si ingredient alors texte 0 sinon texte 1
            
            // if (_coroutineState == null)
            //     _coroutineState = StartCoroutine(IValidateIngredient());
            
            IEnumerator IValidateIngredient()
            {
                while (GetTutorialState() == TutorialStep.ApproachCloser)
                {
                    // si le joueur a des ingredients alors on passe a l'étape de cuisine
                    // sinon on demande au joueur de prendre un ingredient
                }
                
                yield return null;
            }
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(cauldron.transform.position, distanceToCauldron);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(cauldron.transform.position, distanceCloseEnough);
        }
    }
}
