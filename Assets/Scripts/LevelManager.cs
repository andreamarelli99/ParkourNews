using UnityEngine;
using UnityEngine.SceneManagement;

namespace ParkourNews.Scripts
{
    public class LevelManager: MonoBehaviour
    {
        //to reset stickman initial position upon death
        private Vector2 _initialPosition;
        
        public void OnEnable()
        {
           EventManager.StartListening("OnPlayerDeath",OnPlayerDeath);
           _initialPosition = FindObjectOfType<StickmanController>().transform.position;
        }

        private void OnPlayerDeath()
        {
            FindObjectOfType<StickmanController>().transform.position = _initialPosition;
        }
    }
}