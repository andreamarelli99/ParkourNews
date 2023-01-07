using System;
using TMPro;
using UnityEngine;
public class PlayerCoinsCounter : MonoBehaviour
{
    private int _inkers = 0;
    private int _inkersInLastCheckPoint = 0;

    private GameObject _inkersTextObject;
    
    private void OnEnable()
    {
        EventManager.StartListening("OnCoin", AddCoin);
    }

    private void Start()
    {
        EventManager.StartListening("OnRespawn",OnRespawn);
        EventManager.StartListening("CheckPointReached", CheckPointReached);
    }

    private void CheckPointReached()
    {
        _inkersInLastCheckPoint = _inkers;
    }

    private void AddCoin()
    {
        _inkers++;
        EventManager.TriggerEvent("onIncreaseCoinsBar");
    }

    private void OnRespawn()
    {
        EventManager.StopListening("OnRespawn",OnRespawn);
        EventManager.TriggerEvent("OnResetCoinsBar");
        EventManager.StartListening("OnRespawn",OnRespawn);
    }

}
