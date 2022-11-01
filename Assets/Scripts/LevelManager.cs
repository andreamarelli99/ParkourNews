using UnityEngine;
using UnityEngine.SceneManagement;

namespace ParkourNews.Scripts
{
    public class LevelManager: MonoBehaviour
    {
        //to reset stickman initial position upon death
        private Vector2 _initialPosition;
        private double _playerPoints;
        [SerializeField] private double _coinValue = 1;
        
        private void Awake()
        {
            _playerPoints = 0;
        }

        public void OnEnable()
        {
           EventManager.StartListening("OnPlayerDeath",OnPlayerDeath);
           EventManager.StartListening("OnCoin",OnCoin);
           _initialPosition = FindObjectOfType<StickmanController>().transform.position;
        }

        private void OnPlayerDeath()
        {
            FindObjectOfType<StickmanController>().transform.position = _initialPosition;
        }

        private void OnCoin()
        {
            EventManager.StopListening("OnCoin",OnCoin);
            _playerPoints+= _coinValue;
            Debug.Log("Collected Coin! points:"+ _playerPoints);
            EventManager.StartListening("OnCoin",OnCoin);
        }
    }
}