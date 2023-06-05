using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class TutorialManager : MonoBehaviour
{
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

    public int step
    {
        get => step;
        private set => step = value;
    }
    
    [SerializeField] private GameObject cauldron;
    [SerializeField] private string[] dialogue;
    private DialogueBox _dialogueBox;

    private void Start()
    {
        _dialogueBox = GameObject.FindObjectOfType<DialogueBox>();
    }

    public void ApproachCauldron()
    {
        step = 1;
        string[] dialogueToDisplay = { dialogue[0] };
        _dialogueBox.DisplayDialogueText(dialogueToDisplay, cauldron.transform);
    }

    public void ValidateIngredient()
    {
        step = 2;
        string[] dialogueToDisplay = { dialogue[2] };
        _dialogueBox.DisplayDialogueText(dialogueToDisplay, cauldron.transform);

        FindObjectOfType<PlayerController>().GetPlayerAction().Default.Cook.performed += this.CookMenuOpen;
    }

    public void UnValidateIngredient()
    {
        step = 1;
        string[] dialogueToDisplay = { dialogue[1] };
        _dialogueBox.DisplayDialogueText(dialogueToDisplay, cauldron.transform);
    }


    public void CookMenuOpen(InputAction.CallbackContext context)
    {
        FindObjectOfType<PlayerController>().GetPlayerAction().Default.Cook.performed -= this.CookMenuOpen;
        step = 3;
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
