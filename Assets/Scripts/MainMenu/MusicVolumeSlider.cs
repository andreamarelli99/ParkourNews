using System;
using ParkourNews.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{
    private DataManager _dataManager;
    private Slider _slider;

    private float _volume;

    private void Start()
    {
        _slider = GetComponent<Slider>();
        _slider.onValueChanged.AddListener(OnSelect);
        _dataManager = FindObjectOfType<DataManager>();
        _volume = _dataManager.GetMusicVolume();
        _slider.value = _volume;
    }

    private void OnSelect(float value)
    {
        _volume = (float)Math.Round(value, 2);
        _dataManager.SetMusicVolume(value);
    }
}