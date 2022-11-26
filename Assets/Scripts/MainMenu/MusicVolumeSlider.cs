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
        _volume =
            _slider.value = _volume;
        _dataManager = FindObjectOfType<DataManager>();
        _volume = _dataManager.GetMusicVolume();
    }

    private void OnSelect(float value)
    {
        _volume = value;
        _dataManager.SetMusicVolume(value);
    }
}