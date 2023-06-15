using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Dialogues
{
    [Serializable]
    public class Dialogue
    {
        public string[] dialogues;
    }

    public class DialogueBox : MonoBehaviour
    {
        [SerializeField] Vector3 defaultPosition;
        [SerializeField] private TextMeshProUGUI DialogueText;
        [SerializeField] private float TextSpeed;
        private float TimeBtwLetters;
        public float TimeBtwTexts;
        IEnumerator DisplayEnumerable;
        private bool isEnumeratorRunning = false;
        string[] textToDisplay;

        [SerializeField] private float TextTimeOnScreen;

        [SerializeField] Camera cam;
        [SerializeField] Transform Lookat;
        [SerializeField] Vector3 _offset;
        private void Start()
        {
            defaultPosition = GetComponent<RectTransform>().position;
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

        public void DisplayText(string text, Transform lookAt)
        {
            DialogueText.text = text;
            Lookat = lookAt;
        }
        
        public void DisplayDialogueText(string[] text, Transform lookat)
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
    
        IEnumerator Display(string[] textToDisplay)
        {
            isEnumeratorRunning = true;
            int i = 0;

            for (int j = 0; j < textToDisplay.Length; j++)
            {
                DialogueText.text = "";
                while (i < textToDisplay[j].Length)
                {
                
                    DialogueText.text += textToDisplay[j][i];
                    i++;
                    yield return new WaitForSecondsRealtime(TimeBtwLetters);
                }

                i = 0;
                yield return new WaitForSecondsRealtime(TimeBtwTexts); 
            }

            yield return new WaitForSecondsRealtime(TextTimeOnScreen);
            DialogueText.text = "";
            Lookat = null;
            transform.position = defaultPosition;

            isEnumeratorRunning = false;
        }
    }
}