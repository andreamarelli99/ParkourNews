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
        [SerializeField] private int _maxLevel =3;
        
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        public void Reset()
        {
            _playerPoints = 0;
            _currentLevel = 1;
        }

        public void Start()
        {
            _currentLevel = 1;
        }

        public int GetCurrentLevel()
        {
            return _currentLevel;
        }

        public int GetNextLevel() 
        {
            _currentLevel = _currentLevel+1; // replay the last level
            if(_currentLevel<=_maxLevel)
                return _currentLevel;
            return _maxLevel;
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

        public int numberOfLevels()
        {
            return _maxLevel;
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