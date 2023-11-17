using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TotemMenuManager : MonoBehaviour
{
    public static TotemMenuManager _instance;
    [SerializeField] GameObject _totemUI;
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }

        _instance = this;
    }

    public void SwitchUI()
    {
        if (!_totemUI.activeInHierarchy)
            OpenUI();
        else
            CloseUI();
        
    }
    public void OpenUI()
    {
        _totemUI.SetActive(true);

            PlayerController.Instance.GetPlayerAction().UI.Enable();
            PlayerController.Instance.GetPlayerAction().Cooking.Disable();
            PlayerController.Instance.AbleToMove(false);
            TotemSelectorScript.Instance.Init();
        

    }

    public void CloseUI()
    {
        _totemUI.SetActive(false);
        PlayerController.Instance.GetPlayerAction().UI.Disable();
        PlayerController.Instance.GetPlayerAction().Cooking.Enable();
        PlayerController.Instance.AbleToMove(true);
    }
}
