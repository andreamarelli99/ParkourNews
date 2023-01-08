using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ParkourNews.Scripts
{
    public class LevelManager: MonoBehaviour
    { //todo move coin after finishing levels impl
        
        private float _playerPoints;
        private float _playerPointAtCheckpoint;
        [SerializeField] private float _coinValue = 1;
        
        private int _currentLevel;
        [SerializeField] private int _maxLevel =14;

        [SerializeField] private List<int> coinsPerLevel;
        
        private DataManager _dataManager;
        private int _nextLevel;

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            _dataManager = FindObjectOfType<DataManager>();
            coinsPerLevel = new List<int>();
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
            coinsPerLevel.Add(17);
            coinsPerLevel.Add(19);
            coinsPerLevel.Add(32);
            coinsPerLevel.Add(113);
            coinsPerLevel.Add(238);
        }

        

        public void Start()
        {
            _playerPointAtCheckpoint = 0;
            _currentLevel = 1;
            EventManager.StartListening("OnRespawn",OnRespawn);
            EventManager.StartListening("OnCoin",OnCoin);
            EventManager.StartListening("EndLevel",OnEndLevel);
            EventManager.StartListening("OnReload",OnReload);
            EventManager.StartListening("CheckPointReached", CheckPointReached);
        }

        

        private void OnEndLevel()
        {
            EventManager.StopListening("EndLevel",OnEndLevel);
            _nextLevel = _currentLevel + 1;
            Debug.Log("livello finito :  "+ _currentLevel +" && prossimo livello: "+ _nextLevel + "&& tot livelli: "+coinsPerLevel.Count);
            _dataManager.SetData(GetCurrentLevel(),getPlayerPointsRatio());
            EventManager.TriggerEvent("Save");
            EventManager.TriggerEvent("EndMenu");
            EventManager.StartListening("EndLevel",OnEndLevel);
            _playerPointAtCheckpoint = 0;
        }

        private void OnReload()
        {
            EventManager.StopListening("OnReload",OnReload);
            _playerPointAtCheckpoint = 0;
            EventManager.StartListening("OnReload",OnReload);
        }

        private void CheckPointReached()
        {
            EventManager.StopListening("CheckPointReached", CheckPointReached);
            _playerPointAtCheckpoint = _playerPoints;
            EventManager.StartListening("CheckPointReached", CheckPointReached);
        }
        
        public void setCurrentLevel(int level)
        {
            _currentLevel = level;
        }

        public void OnRespawn()
        {
            EventManager.StopListening("OnRespawn",OnRespawn);
            EventManager.TriggerEvent("onResetCoinsBar");
            _playerPoints = _playerPointAtCheckpoint;
            EventManager.StartListening("OnRespawn",OnRespawn);
        }
        
        public int GetCurrentLevel()
        {
            return _currentLevel;
        }
        
        public int GetNextLevel()
        {
            
            return _nextLevel;
        }
        
        public int GetCoinsCurrentLevel()
        {
            return coinsPerLevel[_currentLevel - 1];
        }
        
        public float getPlayerPointsRatio()
        {
            float collectedCoins = _playerPoints;
            float coins = coinsPerLevel[_currentLevel - 1];
            Debug.Log("Coins: " + _playerPoints);
            Debug.Log("Coins in level: " + coins);
            resetPlayerPoints();
            if(coins>0)
                return collectedCoins/coins;
            return 1;
            
        }

        public void resetPlayerPoints()
        {
            _playerPoints = 0;
        }

        public int numberOfLevels()
        {
            return coinsPerLevel.Count;
        }
        
        private void OnCoin()
        {
            EventManager.StopListening("OnCoin",OnCoin);
            _playerPoints+= _coinValue;
            Debug.Log("Coins: " + _playerPoints);
            EventManager.StartListening("OnCoin",OnCoin);
            
        }

        public int getStars()
        {
            return _dataManager.getStars();
        }
    }
}