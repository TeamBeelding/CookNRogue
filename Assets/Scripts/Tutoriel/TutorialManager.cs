using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    public int step
    {
        get => step;
        private set => step = value;
    }
    
    [SerializeField] GameObject Cauldron;
    [SerializeField] private string[] _dialogue;
    private DialogueBox _dialogueBox;

    private void Start()
    {
        _dialogueBox = GameObject.FindObjectOfType<DialogueBox>();
    }

    public void ApproachCauldron()
    {
        step = 1;
        string[] dialogueToDisplay = { _dialogue[0] };
        _dialogueBox.DisplayDialogueText(dialogueToDisplay, Cauldron.transform);
    }

    public void ValidateIngredient()
    {
        step = 2;
        string[] dialogueToDisplay = { _dialogue[2] };
        _dialogueBox.DisplayDialogueText(dialogueToDisplay, Cauldron.transform);

        FindObjectOfType<PlayerController>().GetPlayerAction().Default.Cook.performed += this.CookMenuOpen;
    }

    public void UnValidateIngredient()
    {
        step = 1;
        string[] dialogueToDisplay = { _dialogue[1] };
        _dialogueBox.DisplayDialogueText(dialogueToDisplay, Cauldron.transform);
    }


    public void CookMenuOpen(InputAction.CallbackContext context)
    {
        FindObjectOfType<PlayerController>().GetPlayerAction().Default.Cook.performed -= this.CookMenuOpen;
        step = 3;
        string[] dialogueToDisplay = { _dialogue[3] };
        Time.timeScale = 0;
        _dialogueBox.DisplayDialogueText(dialogueToDisplay, Cauldron.transform);
    }

    public void FoodSpawn()
    {
        
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
        
    }
    
    public void ShowInteractButton()
    {
        
    }
    
    
}
