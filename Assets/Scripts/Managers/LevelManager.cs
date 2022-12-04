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

        private DataManager _dataManager;

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            _dataManager = FindObjectOfType<DataManager>();
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
            coinsPerLevel.Add(17);
            coinsPerLevel.Add(28);
        }

        

        public void Start()
        {
            _currentLevel = 1;
            
            EventManager.StartListening("StartNextLevel",OnStartNextLevel);
            EventManager.StartListening("OnRespawn",OnRespawn);
            EventManager.StartListening("OnCoin",OnCoin);
            
            EventManager.StartListening("EndLevel",OnEndLevel);
            
        }

        private void OnEndLevel()
        {
            EventManager.StopListening("EndLevel",OnEndLevel);
            
            int nextLevel = _currentLevel + 1;
            Debug.Log("livello finito :  "+ _currentLevel +" && prossimo livello: "+ nextLevel + "&& tot livelli: "+coinsPerLevel.Count);
            _dataManager.SetData(GetCurrentLevel(),getPlayerPointsRatio());
            EventManager.TriggerEvent("Save");
            _currentLevel = nextLevel;
            if (nextLevel <= coinsPerLevel.Count)
                SceneManager.LoadScene(nextLevel.ToString());
            else
                SceneManager.LoadScene("MenuSelector");
            EventManager.StartListening("EndLevel",OnEndLevel);
            
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
            
            //if (_currentLevel < _maxLevel)
           // {
            //    _currentLevel += 1;
            //    SceneManager.LoadScene(_currentLevel.ToString());
            //    EventManager.TriggerEvent("OnPlayLevel");
            //    EventManager.StartListening("StartNextLevel", OnStartNextLevel);
          //  }
          //  else 
                SceneManager.LoadScene("MenuSelector");
                EventManager.StartListening("StartNextLevel", OnStartNextLevel);
           
        }
        
        public float getPlayerPointsRatio()
        {
            float coins=coinsPerLevel[_currentLevel - 1];
            if(coins>0)
                return _playerPoints/coins;
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
            EventManager.StartListening("OnCoin",OnCoin);
            
        }
    }
}