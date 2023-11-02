using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneLoader : MonoBehaviour
{
    public GameObject mainBook;
    public GameObject settingsPrefab;
    public GameObject creditsPrefab;

    private enum State
    {
        Main,
        Settings,
        Credits
    }

    private State _curState;

    PlayerActions _actions;
    
    private void Awake()
    {
        mainBook.SetActive(true);
        settingsPrefab.SetActive(false);
        creditsPrefab.SetActive(false);

        _curState = State.Main;
    }

    private void Start()
    {
        _actions = new();
        _actions.UI.Enable();

        _actions.UI.Return.started += Return;
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowMain()
    {
        switch (_curState)
        {
            case (State.Credits):
                {
                    creditsPrefab.SetActive(false);
                    break;
                }
            case (State.Settings):
                {
                    settingsPrefab.SetActive(false);
                    break;
                }
        }
        
        mainBook.SetActive(true);
        _curState = State.Main;
    }

    public void ShowCredits()
    {
        switch (_curState)
        {
            case (State.Main):
                {
                    mainBook.SetActive(false);
                    break;
                }
            case (State.Settings):
                {
                    settingsPrefab.SetActive(false);
                    break;
                }
        }

        creditsPrefab.SetActive(true);
        _curState = State.Credits;
    }

    public void ShowSettings()
    {
        switch (_curState)
        {
            case (State.Main):
                {
                    mainBook.SetActive(false);
                    break;
                }
            case (State.Credits):
                {
                    creditsPrefab.SetActive(false);
                    break;
                }
        }

        settingsPrefab.SetActive(true);
        _curState = State.Settings;
    }

    public void Exit()
    {
        Application.Quit();
    }

    void Return(InputAction.CallbackContext context)
    {
        ShowMain();
    }
}
