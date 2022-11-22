using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
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

        [SerializeField] private List<int> coinsPerLevel;

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
            EventManager.TriggerEvent("OnPlayLevel");
            EventManager.StartListening("StartNextLevel",OnStartNextLevel);
        }
        
        public float getPlayerPointsRatio()
        {
            float coins=coinsPerLevel[_currentLevel - 1];
            if(coins>0)
                return _playerPoints/_currentLevel;
            return 1;
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