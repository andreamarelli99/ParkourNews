using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ParkourNews.Scripts
{
    
    public class EndCollectible: MonoBehaviour
    {
        private bool _gameIsOn;

        private void Start()
        {
            _gameIsOn = false;
            EventManager.StartListening("StickmanSpawned",OnStickmanSpawned);
        }

        private void OnStickmanSpawned()
        {
            EventManager.StopListening("StickmanSpawned",OnStickmanSpawned);
            _gameIsOn = true;
            Debug.Log("Game is On");
            EventManager.StartListening("StickmanSpawned",OnStickmanSpawned);
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_gameIsOn&&collision.gameObject.CompareTag("Stickman")){
                EventManager.TriggerEvent("EndLevel");
                EventManager.TriggerEvent("FinishSound");
                gameObject.SetActive(false);
                _gameIsOn = false;
            }
        }
    }
}