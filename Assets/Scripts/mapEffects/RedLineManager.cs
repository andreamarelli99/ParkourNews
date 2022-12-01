using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLineManager : MonoBehaviour
{
    [SerializeField] private GameObject _redLine;
    [SerializeField] private Vector2 _initialPosition;

    private void OnEnable()
    {
        EventManager.StartListening("OnDeath", OnDeath);
        Instantiate(_redLine, _initialPosition, Quaternion.identity);
    }
    
    
    private void OnDeath()
    { 
        StartCoroutine(StartMovingCoroutine());
        EventManager.StopListening("OnDeath",OnDeath);
        EventManager.TriggerEvent("OnPlayerDeath");
        EventManager.StartListening("OnDeath",OnDeath);
    }
    
    private IEnumerator StartMovingCoroutine()
    {
        yield return new WaitForSeconds(2);
        Instantiate(_redLine, _initialPosition, Quaternion.identity);
    }
    
    
}
