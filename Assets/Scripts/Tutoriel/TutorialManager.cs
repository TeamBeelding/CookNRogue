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
            CookMenuOpen,
            FightingPhase,
            End
        }
        
        [SerializeField] private TutorialStep tutorialStep;
        [SerializeField] private float distanceToCauldron = 2f;
        [SerializeField] private float distanceCloseEnough = 1f;
        private bool _isCloseEnough = false;
        
        [SerializeField]
        private bool isMoving = false;
        [SerializeField]
        private bool isCookingDone = false;
        
        [SerializeField] 
        private GameObject[] ingredients;
        [SerializeField]
        private GameObject enemy;

        private Coroutine _coroutineState;
        private IEnumerator _enumeratorState;
    
        public bool GetIsMoving() => isMoving;
        public bool GetIsCookingDone() => isCookingDone;

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
        
        // ReSharper disable Unity.PerformanceAnalysis
        private void SetTutorialState(TutorialStep tutorialStep)
        {
            StopAllCoroutines();
            
            this.tutorialStep = tutorialStep;
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
                case TutorialStep.CookMenuOpen:
                    Cooking();
                    break;
                case TutorialStep.FightingPhase:
                    Fighting();
                    break;
                case TutorialStep.End:
                    EndTutorial();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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
            StartCoroutine(IChekingMovement());
            
            IEnumerator IChekingMovement()
            {
                while (tutorialStep == TutorialStep.Move)
                {
                    if (GetIsMoving())
                        SetTutorialState(TutorialStep.ApproachCauldron);

                    yield return null;
                }
            }
        }
        
        public void SetIsMoving(bool value) => isMoving = value;
        
        public void SetIsCookingDone(bool value) => isCookingDone = value;

        private void ApproachCauldron()
        {
            StartCoroutine(IApproaching());
            
            IEnumerator IApproaching()
            {
                while (tutorialStep == TutorialStep.ApproachCauldron)
                {
                    if (Vector3.Distance(_player.transform.position, cauldron.transform.position) <= distanceToCauldron)
                    {
                        if (Vector3.Distance(_player.transform.position, cauldron.transform.position) <= distanceCloseEnough)
                            SetTutorialState(TutorialStep.ApproachCloser);
                        else
                        {
                            _textToDisplay = dialogue[0];
                            _dialogueBox.DisplayText(_textToDisplay, cauldron.transform);
                        }
                    }
                    
                    yield return null;
                }
            }
        }
        
        private void ApproachMoreClose()
        {
            StartCoroutine(IValidateIngredient());
            
            IEnumerator IValidateIngredient()
            {
                while (tutorialStep == TutorialStep.ApproachCloser)
                {
                    // si nombre ingrédient > 0 alors 
                    // sinon afficher texte "il n'y a pas d'ingrédient"

                    if ( 1 > 0)
                    {
                        _textToDisplay = dialogue[1];
                        _dialogueBox.DisplayText(_textToDisplay, cauldron.transform);
                        
                        yield return new WaitForSeconds(0.5f);
                        
                        // surbrillance chaudron
                        SetTutorialState(TutorialStep.CookMenuOpen);
                    }
                    else
                    {
                        // surbrillance ingrédient
                        
                        _textToDisplay = dialogue[4];
                        _dialogueBox.DisplayText(_textToDisplay, cauldron.transform);
                    }
                    
                    yield return null;
                }
            }
        }

        private void Cooking()
        {
            _textToDisplay = "Maintenez X pour ouvrir le menu de cuisine";
            _dialogueBox.DisplayText(_textToDisplay, cauldron.transform);
            
            StartCoroutine(ICooking());

            IEnumerator ICooking()
            {
                while (tutorialStep == TutorialStep.CookMenuOpen)
                {
                    if (GetIsCookingDone())
                    {
                        Debug.Log("Player is cooking");
                        // Changing state
                        SetTutorialState(TutorialStep.FightingPhase);
                    }
                    
                    yield return null;
                }
            }
        }
        
        private void Fighting()
        {
            SpawnEnemy();

            StartCoroutine(IFighting());
            
            IEnumerator IFighting()
            {
                while (tutorialStep == TutorialStep.FightingPhase)
                {
                    if (!enemy)
                        SetTutorialState(TutorialStep.End);
                    
                    yield return null;
                }
            }
        }
        
        private void EndTutorial()
        {
            FoodSpawn();
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

        private void SpawnEnemy()
        {
            if (enemy == null)
                return;
        
            enemy.SetActive(true);
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
