using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinControllerCheckPoint : MonoBehaviour
{
    private bool _alreadyGot = false;
    private bool _toRecreate = true;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player")){
            EventManager.TriggerEvent("OnCoin");
            _alreadyGot = true;
            gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        EventManager.StartListening("OnRespawn",OnRespawn);
        EventManager.StartListening("CheckPointReached", CheckPointReached);
    }

    private void CheckPointReached()
    {
        if (_alreadyGot)
        {
            _toRecreate = false;
        }
    }

    private void OnRespawn()
    {
        EventManager.StopListening("OnRespawn",OnRespawn);
        if (_toRecreate)
        {
            gameObject.SetActive(true);
            _alreadyGot = false;
            EventManager.StartListening("OnRespawn",OnRespawn);
        }
    }

}
