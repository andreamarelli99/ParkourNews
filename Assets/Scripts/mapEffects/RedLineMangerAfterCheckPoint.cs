using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLineMangerAfterCheckPoint : MonoBehaviour
{
    [SerializeField] private GameObject _redLine;
    private Vector2 _initialPosition;

    private void Start()
    {
        _initialPosition = transform.position;
        EventManager.StartListening("StartRedLineFromCheckPoint",StartLine);
    }

    private void StartLine()
    {
        EventManager.StopListening("StartRedLineFromCheckPoint",StartLine);
        EventManager.StartListening("OnDeath", OnDeath);
        EventManager.StartListening("ReachedEndRedLineCheckPoint",ReachedEndRedLineCheckPoint);
        
        Instantiate(_redLine, _initialPosition, Quaternion.identity);
    }

    private void ReachedEndRedLineCheckPoint()
    {
        Destroy(gameObject);
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
        yield return new WaitForSeconds(3);
        Instantiate(_redLine, _initialPosition, Quaternion.identity);
    }
}
