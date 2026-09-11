using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsUIController : MonoBehaviour
{
    [SerializeField] private GameObject _settingsUI;
    [SerializeField] private KeyCode _settingsKeyCode;

    private bool _hasPressedSettingsKey => Input.GetKeyDown(_settingsKeyCode);
    private bool _isSettingsOpen = false;

    private void Start() => Init();

    private void Update()
    {
        ToggleSettings();
    }

    private void Init()
    {
        _settingsUI.SetActive(false);
    }

    private void ToggleSettings()
    {
        if (!_hasPressedSettingsKey) return;

        if (_isSettingsOpen)
        {
            GameManager.Instance.Run();
            _settingsUI.SetActive(false);
            _isSettingsOpen = false;
        }
        else
        {
            GameManager.Instance.Pause();
            _settingsUI.SetActive(true);
            _isSettingsOpen = true;
        }
    }
}
