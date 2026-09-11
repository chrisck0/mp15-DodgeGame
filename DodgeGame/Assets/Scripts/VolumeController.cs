using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _volumeText;
    [SerializeField] private Slider _volumeSlider;

    private AudioSource _audioSource;

    private void Awake() => CacheComponents();

    private void CacheComponents()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        BindSliderEvents();
    }

    private void OnDisable()
    {
        UnbindSliderEvents();
    }

    private void BindSliderEvents()
    {
        _volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    private void UnbindSliderEvents()
    {
        _volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
    }

    private void OnVolumeChanged(float volume)
    {
        int volumeToInt = (int)volume;
        UpdateText(volumeToInt);
        UpdateVolume(volumeToInt);
    }

    private void UpdateText(int volume)
    {
        _volumeText.text = $"Volume: {volume}";
    }

    private void UpdateVolume(int volume)
    {
        _audioSource.volume = (float)volume / 10;
    }
}
