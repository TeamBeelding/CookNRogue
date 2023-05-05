using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem.XR;
using static System.Net.Mime.MediaTypeNames;

public class DialogueBox : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI DialogueText;
    [SerializeField] private float TextSpeed;
    private float TimeBtwLetters;
    IEnumerator DisplayEnumerable;
    private bool isEnumeratorRunning = false;
    string textToDisplay;

    [SerializeField] private float TextTimeOnScreen;

    public void DisplayDialogueText(string text)
    {
        //STOPS COROUTINE IF IT IS CURRENTLY RUNNING
        if (isEnumeratorRunning)
        {
            StopCoroutine(DisplayEnumerable);
            isEnumeratorRunning = false;
        }

        //INITIALISE THE TEXT TO DISPLAY AND THE PARAMETERS
        textToDisplay = text;
        DialogueText.text = "";
        TimeBtwLetters = 1 / TextSpeed;
        DisplayEnumerable = Display(textToDisplay);

        //LAUNCHING THE DISPLAY OF THE TEXT
        StartCoroutine(DisplayEnumerable);
    }
    
    IEnumerator Display(string textToDisplay)
    {
        isEnumeratorRunning = true;
        int i = 0;
        while (i < textToDisplay.Length)
        {
            DialogueText.text += textToDisplay[i];
            i++;
            yield return new WaitForSeconds(TimeBtwLetters);
        }
        isEnumeratorRunning = false;

        yield return new WaitForSeconds(TextTimeOnScreen);
        DialogueText.text = "";
    }
    
}
