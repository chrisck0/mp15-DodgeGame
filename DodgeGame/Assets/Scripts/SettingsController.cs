using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private GameObject _settingsUI;
    [SerializeField] private TextMeshProUGUI _volumeText;
    [SerializeField] private KeyCode _settingsKeyCode;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private GameManager _gameManager;

    private AudioSource _audioSource;

    private bool _hasPressedSettingsKey => Input.GetKeyDown(_settingsKeyCode);
    private bool _isSettingsOpen = false;


    private void Awake() => CacheComponents();
    private void Start() => Init();

    private void Update()
    {
        ToggleSettings();
    }

    private void CacheComponents()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Init()
    {
        BindSliderEvents();
        _settingsUI.SetActive(false);
    }

    private void BindSliderEvents()
    {
        _volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    private void OnVolumeChanged(float volume)
    {
        int volumeToInt = (int)(volume * 10);
        UpdateText(volumeToInt);
        UpdateVolume(volumeToInt);
    }

    private void UpdateText(int volume)
    {
        _volumeText.text = $"Volume: {volume}";
    }

    private void ToggleSettings()
    {
        if (!_hasPressedSettingsKey) return;

        if (_isSettingsOpen)
        {
            _settingsUI.SetActive(false);
            _gameManager.LockCursor();
            _isSettingsOpen = false;
        }
        else
        {
            _settingsUI.SetActive(true);
            _gameManager.UnlockCursor();
            _isSettingsOpen = true;
        }
    }

    private void UpdateVolume(int volume)
    {
        float volumeToFloat = (float)volume / 10;
        _audioSource.volume = volumeToFloat;
    }
}
