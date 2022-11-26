using ParkourNews.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class SfxVolumeSlider : MonoBehaviour
{
    private DataManager _dataManager;
    private Slider _slider;

    private float _volume;

    private void Start()
    {
        _slider = GetComponent<Slider>();
        _slider.onValueChanged.AddListener(OnSelect);
        _slider.value = _volume;
        _dataManager = FindObjectOfType<DataManager>();
        _volume = _dataManager.GetSfxVolume();
    }

    private void OnSelect(float value)
    {
        _volume = value;
        _dataManager.SetSfxVolume(value);
    }
}