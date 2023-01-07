using System;
using System.Collections;
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_gameIsOn&&collision.gameObject.CompareTag("Player")){
                //EventManager.TriggerEvent("EndLevel");
                EventManager.TriggerEvent("WinAnimation");
                EventManager.TriggerEvent("ZoomIn");
                EventManager.TriggerEvent("FinishSound");
                gameObject.GetComponent<SpriteRenderer>().color = new Color(
                     gameObject.GetComponent<SpriteRenderer>().color.r,
                     gameObject.GetComponent<SpriteRenderer>().color.g,
                     gameObject.GetComponent<SpriteRenderer>().color.b, 0f);
                _gameIsOn = false;
                StartCoroutine(EndLevelCoroutine());
            }
        }

        IEnumerator EndLevelCoroutine()
        {
            yield return new WaitForSeconds(2);
            EventManager.TriggerEvent("EndLevel");
            gameObject.SetActive(false);
        }
    }
}