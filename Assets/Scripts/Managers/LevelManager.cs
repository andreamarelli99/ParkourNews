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
            coinsPerLevel=new List<int>();
            coinsPerLevel.Add(6);
            coinsPerLevel.Add(35);
            coinsPerLevel.Add(28);
            coinsPerLevel.Add(20);
            coinsPerLevel.Add(27);
            coinsPerLevel.Add(31);
            coinsPerLevel.Add(47);
            coinsPerLevel.Add(40);
            coinsPerLevel.Add(35);
            coinsPerLevel.Add(27);
        }

        

        public void Start()
        {
            _currentLevel = 1;
            
            EventManager.StartListening("StartNextLevel",OnStartNextLevel);
            EventManager.StartListening("OnRespawn",OnRespawn);
            EventManager.StartListening("OnCoin",OnCoin);
            
        }

        public void setCurrentLevel(int level)
        {
            _currentLevel = level;
        }

        public void OnRespawn()
        {
            EventManager.StopListening("OnRespawn",OnRespawn);
            _playerPoints = 0;
            EventManager.StartListening("OnRespawn",OnRespawn);
        }
        
        public int GetCurrentLevel()
        {
            return _currentLevel;
        }

        public void OnStartNextLevel() 
        {
            EventManager.StopListening("StartNextLevel",OnStartNextLevel);
            
            if (_currentLevel < _maxLevel)
            {
                _currentLevel += 1;
                SceneManager.LoadScene(_currentLevel.ToString());
                EventManager.TriggerEvent("OnPlayLevel");
                EventManager.StartListening("StartNextLevel", OnStartNextLevel);
            }
            else 
                SceneManager.LoadScene("Menu");
           
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