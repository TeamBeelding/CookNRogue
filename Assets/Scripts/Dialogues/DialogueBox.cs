using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem.XR;
using static System.Net.Mime.MediaTypeNames;
using System.Linq;

public class DialogueBox : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI DialogueText;
    [SerializeField] private float TextSpeed;
    private float TimeBtwLetters;
    IEnumerator DisplayEnumerable;
    private bool isEnumeratorRunning = false;
    string textToDisplay;

    [SerializeField] private float TextTimeOnScreen;

    [SerializeField] Camera cam;
    [SerializeField] Transform Lookat;
    [SerializeField] Vector3 _offset;
    private void Start()
    {
        cam = Camera.main;
        DialogueText.text = "";
        TimeBtwLetters = 1 / TextSpeed;
    }
    private void Update()
    {
        if(Lookat != null)
        {
            Vector3 pos = cam.WorldToScreenPoint(Lookat.position  + _offset);

            if (transform.position != pos)
                transform.position = pos;
        }

    }
    public void DisplayDialogueText(string text, Transform lookat)
    {
        //STOPS COROUTINE IF IT IS CURRENTLY RUNNING
        if (isEnumeratorRunning)
        {
            StopCoroutine(DisplayEnumerable);
            isEnumeratorRunning = false;
        }

        //INITIALISE THE TEXT TO DISPLAY AND THE PARAMETERS
        Lookat = lookat;
        textToDisplay = text;
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
