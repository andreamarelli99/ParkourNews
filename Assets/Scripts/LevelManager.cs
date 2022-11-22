using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ParkourNews.Scripts
{
    public class LevelManager: MonoBehaviour
    { //todo move coin after finishing levels impl
        
        private float _playerPoints;
        [SerializeField] private float _coinValue = 1;
        
        private int _currentLevel;
        [SerializeField] private int _maxLevel =3;
        
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        

        public void Start()
        {
            _currentLevel = 1;
            
            EventManager.StartListening("StartNextLevel",OnStartNextLevel);
            EventManager.StartListening("Reset",OnReset);
            EventManager.StartListening("OnCoin",OnCoin);
        }
        
        public void OnReset()
        {
            EventManager.StopListening("Reset",OnReset);
            _playerPoints = 0;
            SceneManager.LoadScene(_currentLevel.ToString());
            EventManager.StartListening("Reset",OnReset);
        }
        
        public int GetCurrentLevel()
        {
            return _currentLevel;
        }

        public void OnStartNextLevel() 
        {
            EventManager.StopListening("StartNextLevel",OnStartNextLevel);
            _currentLevel = Math.Min(_currentLevel+1,_maxLevel); 
            SceneManager.LoadScene(_currentLevel.ToString());
            Debug.Log(_currentLevel);
            EventManager.StartListening("StartNextLevel",OnStartNextLevel);
        }
        
        public float getPlayerPoints()
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
            EventManager.StopListening("OnCoin",OnCoin);
            _playerPoints+= _coinValue;
            EventManager.StartListening("OnCoin",OnCoin);
            
        }
    }
}