using System;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player")){
            EventManager.TriggerEvent("OnCoin"); 
            gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        EventManager.StartListening("OnRespawn",OnRespawn);
    }

    private void OnRespawn()
    {
        Debug.Log("respawn");
        EventManager.StopListening("OnRespawn",OnRespawn);
        gameObject.SetActive(true);
        EventManager.StartListening("OnRespawn",OnRespawn);
    }

}
