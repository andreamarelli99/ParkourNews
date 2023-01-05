using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DashCooldownController: MonoBehaviour
{
    private Slider _slider;
    private GameController _gameController;

    private void Start()
    {
        _gameController = FindObjectOfType<GameController>();
        _slider = GetComponent<Slider>();
        _slider.maxValue = _gameController.GetSecondsBetweenDash();
        _slider.value = _slider.maxValue;
        EventManager.StartListening("OnDash", OnDash);
        EventManager.StartListening("CanDash", CanDash);
        EventManager.StartListening("OnRespawn", OnRespawn);
    }

    private void OnDash()
    {
        EventManager.StopListening("OnDash", OnDash);
        _slider.value = 0;
        StartCoroutine("IncreaseSlider");
        EventManager.StartListening("OnDash", OnDash);
    }
    
    private IEnumerator IncreaseSlider()
    {
        for (int i = 0; i < _slider.maxValue; i++)
        {
            yield return new WaitForSeconds(1f);
            _slider.value += 1;
        }
    }

    private void CanDash()
    {
        EventManager.StopListening("CanDash", CanDash);
        _slider.value = _slider.maxValue;
        EventManager.StartListening("CanDash", CanDash);
    }
    
    private void OnRespawn()
    {
        EventManager.StopListening("OnRespawn", OnRespawn);
        _slider.value = _slider.maxValue;
        EventManager.StartListening("OnRespawn", OnRespawn);
    }
}
