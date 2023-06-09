using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionSelector : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _resolutionDropdown;

    private Resolution[] _resolutions;
    private List<Resolution> _filteredResolutions;

    private float _currentRefreshRate;
    private int currentResolutionIndex = 0;

    private Resolution _currentResolution;
    private bool FullScreen = true;

    // Start is called before the first frame update
    void Start()
    {
        _resolutions = Screen.resolutions;
        _filteredResolutions = new List<Resolution>();

        _resolutionDropdown.ClearOptions();
        _currentRefreshRate = Screen.currentResolution.refreshRate;

        for (int i = 0; i < _resolutions.Length; i++)
        {
            if (_resolutions[i].refreshRate == _currentRefreshRate)
                _filteredResolutions.Add(_resolutions[i]);
        }

        List<string> options = new List<string>();
        for (int i = 0; i < _filteredResolutions.Count; i++)
        {
            string resolutionOption = _filteredResolutions[i].width + "x" + _filteredResolutions[i].height + " " + _filteredResolutions[i].refreshRate;
            options.Add(resolutionOption);

            if (_filteredResolutions[i].width == Screen.width && _filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.value = currentResolutionIndex;
        _resolutionDropdown.RefreshShownValue();

    }

    private void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, FullScreen);
    }

    public void SetFullScreen(bool fullScreen)
    {
        Debug.Log(fullScreen);
        Screen.fullScreen = fullScreen;
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
}
