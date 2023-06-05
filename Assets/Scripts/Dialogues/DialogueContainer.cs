using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueContainer : MonoBehaviour
{
    private DialogueBox _dialogueBox;
    [SerializeField] private string[] _dialogue;
    // Start is called before the first frame update
    void Start()
    {
        _dialogueBox = GameObject.FindObjectOfType<DialogueBox>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_dialogueBox != null && other.CompareTag("Player"))
            _dialogueBox.DisplayDialogueText(_dialogue, transform);
    }
}
