using System;
using System.Collections;
using System.Collections.Generic;
using Dialogues;
using Sirenix.Utilities;
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
        private bool isQTE = false;
        [SerializeField]
        private bool isCookingDone = false;
        [SerializeField]
        private bool playerHasIngredients = false;
        
        [SerializeField] 
        private List<GameObject> ingredients;
        [SerializeField]
        private List<GameObject> enemies = null;

        private Coroutine _coroutineState;
        private IEnumerator _enumeratorState;

        private int _step;

        [SerializeField] private GameObject cauldron;
        [SerializeField] private string[] dialogue;
        private DialogueBox _dialogueBox;
        private string _textToDisplay = "";
        private GameObject _player;
        
        [Header("Text to display")]
        [SerializeField]
        private string textWhenPlayerApproachCauldron = "Approach the cauldron";
        [SerializeField]
        private string textWhenPlayerHasIngredients = "Press E to open the cooking menu";
        [SerializeField]
        private string textWhenPlayerHasntIngredients = "You need ingredients to cook";
        [SerializeField]
        private string textWhenMenuIsOpen = "Press E to close the cooking menu";
        [SerializeField]
        private string textWhenPlayerCooking = "Press E to cook";
        [SerializeField]
        private string textWhenQTE = "Press E to fight";
        [SerializeField]
        private string textWhenFighting = "Press E to fight";
        [SerializeField]
        private string textWhenPlayerHasntAmmo = "You need ammo to fight";
        [SerializeField]
        private string textWhenEnd = "Tutorial is over";

        private void Start()
        {
            _dialogueBox = GameObject.FindObjectOfType<DialogueBox>();
            _player = GameObject.FindGameObjectWithTag("Player");

            SetTutorialState(TutorialStep.Move);
        }

        // private void Update()
        // {
        //     Debug.Log(enemies.IsNullOrEmpty());
        // }

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
                    if (isMoving)
                        SetTutorialState(TutorialStep.ApproachCauldron);

                    yield return null;
                }
            }
        }
        
        public void SetIsMoving(bool value) => isMoving = value;
        
        public void SetIsCookingDone(bool value) => isCookingDone = value;
        
        public void SetPlayerHasIngredients(bool value) => playerHasIngredients = value;

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
                            _textToDisplay = textWhenPlayerApproachCauldron;
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
                    if (playerHasIngredients)
                    {
                        OutlineCauldron(true);
                        
                        _textToDisplay = textWhenPlayerHasIngredients;
                        _dialogueBox.DisplayText(_textToDisplay, cauldron.transform);
                        
                        SetTutorialState(TutorialStep.CookMenuOpen);
                    }
                    else
                    {
                        OutlineIngredient(true);
                        
                        _textToDisplay = textWhenPlayerHasntIngredients;
                        _dialogueBox.DisplayText(_textToDisplay, cauldron.transform);
                    }
                    
                    yield return null;
                }
            }
        }

        private void Cooking()
        {
            _textToDisplay = textWhenMenuIsOpen;
            _dialogueBox.DisplayText(_textToDisplay, cauldron.transform);
            
            StartCoroutine(ICooking());

            IEnumerator ICooking()
            {
                yield return new WaitForSeconds(1);
                
                _textToDisplay = textWhenPlayerCooking;
                _dialogueBox.DisplayText(_textToDisplay, cauldron.transform);

                while (tutorialStep == TutorialStep.CookMenuOpen)
                {
                    if (isQTE)
                    {
                        _textToDisplay = textWhenQTE;
                        _dialogueBox.DisplayText(_textToDisplay, cauldron.transform);
                    }
                    if (isCookingDone)
                    {
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
                _textToDisplay = textWhenFighting;
                _dialogueBox.DisplayText(_textToDisplay, cauldron.transform);

                while (tutorialStep == TutorialStep.FightingPhase)
                {
                    if (GameObject.FindGameObjectsWithTag("Enemy").IsNullOrEmpty())
                        SetTutorialState(TutorialStep.End);
                    
                    yield return null;
                }
            }
        }
        
        private void EndTutorial()
        {
            StartCoroutine(IEndTutorial());
            
            IEnumerator IEndTutorial()
            {
                _textToDisplay = textWhenPlayerHasntAmmo;
                _dialogueBox.DisplayText(_textToDisplay, cauldron.transform);
                
                yield return new WaitForSeconds(2);
                
                FoodSpawn();
                
                _textToDisplay = textWhenEnd;
                _dialogueBox.DisplayText(_textToDisplay, cauldron.transform);

                Debug.Log("Open door");
            }
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
        
        private void OutlineCauldron(bool value)
        {
            if (cauldron == null)
                return;
            
            if (value)
            {
                OutlineIngredient(false);
                cauldron.GetComponent<Outline>().enabled = true;
            }
        }

        private void OutlineIngredient(bool value)
        {
            if (ingredients == null || ingredients.Count <= 0)
                return;

            if (value)
            {
                OutlineCauldron(false);
                
                foreach (GameObject ingredient in GameObject.FindGameObjectsWithTag("IngredientTutorial"))
                {
                    ingredient.GetComponent<Outline>().enabled = true;
                }
            }
        }
        
        private void SpawnEnemy()
        {
            if (enemies == null || enemies.Count == 0)
                return;

            foreach (GameObject enemy in enemies)
            {
                enemy.SetActive(true);
            }
        }
        
        private void ShowingUI()
        {
            // if (ui == null)
            //     return;
            //
            // ui.SetActive(true);
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
