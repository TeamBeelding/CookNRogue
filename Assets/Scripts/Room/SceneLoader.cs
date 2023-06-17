using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject mainBook;
    public GameObject settingsPrefab;
    public GameObject creditsPrefab;

    private void Awake()
    {
        mainBook.SetActive(true);
        settingsPrefab.SetActive(false);
        creditsPrefab.SetActive(false);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowMain()
    {
        settingsPrefab.SetActive(false);
        creditsPrefab.SetActive(false);
        mainBook.SetActive(true);
    }

    public void ShowCredits()
    {
        mainBook.SetActive(false);
        settingsPrefab.SetActive(false);
        creditsPrefab.SetActive(true);
    }

    public void ShowSettings()
    {
        mainBook.SetActive(false);
        creditsPrefab.SetActive(false);
        settingsPrefab.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
