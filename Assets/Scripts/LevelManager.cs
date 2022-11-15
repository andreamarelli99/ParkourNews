using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ParkourNews.Scripts
{
    public class LevelManager: MonoBehaviour
    { //todo move coin after finishing levels impl
        
        private double _playerPoints;
        [SerializeField] private double _coinValue = 1;
        
        private int _currentLevel;
        [SerializeField] private int _maxLevel =2;

        public void Reset()
        {
            _currentLevel = 1;
        }

        public void Start()
        {
            _currentLevel = 1;
        }

        public int GetLevel()
        {
            return _currentLevel;
        }

        public int GetNextLevel() 
        {
            _currentLevel = Math.Min(_currentLevel + 1,_maxLevel); // replay the last level
            return _currentLevel;
            
        }
        
        private void Awake()
        {
            _playerPoints = 0;
        }

        public void OnEnable()
        {
            EventManager.StartListening("OnCoin",OnCoin);
        }
        
        public double getPlayerPoints()
        {
            return _playerPoints;
        }

        public void resetPlayerPoints()
        {
            _playerPoints = 0;
        }

        private void OnCoin()
        {
            // move this
            EventManager.StopListening("OnCoin",OnCoin);
            _playerPoints+= _coinValue;
            Debug.Log("Collected Coin! points:"+ _playerPoints);
            EventManager.StartListening("OnCoin",OnCoin);
            //
        }
    }
}