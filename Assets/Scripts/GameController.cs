using System;
using ParkourNews.Scripts;
using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameController: MonoBehaviour
{
    private DataManager _dataManager;
    private LevelManager _levelManager;
    private StickmanController _stickman;

    // dash cooldown value
    [SerializeField] private int secondsBetweenDash=5;
       

        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            _levelManager = GameObject.FindObjectOfType<LevelManager>();
            _dataManager = FindObjectOfType<DataManager>();
            
        }
        
        private void Start()
        {
            
            EventManager.StartListening("OnLevelCompletion",OnLevelCompletion);
            EventManager.StartListening("OnPlayerDeath",OnPlayerDeath);
            EventManager.StartListening("OnPlayLevel",OnPlayLevel);
            EventManager.StartListening("OnDash",OnDash);
            
        }

        // When a level starts "picks" the stickman 
        private void OnPlayLevel()
        {
            _stickman =  GameObject.FindWithTag("Stickman").GetComponent<StickmanController>();
        }
        
        // When the stickman has done the dash action waits a cooldown in order to be able repeat the action
        private void OnDash()
        {
            EventManager.StopListening("OnDash",OnDash);
            Invoke("CanDash", secondsBetweenDash);
        }
        
        // When the stickman dies the level will restart
        private void OnPlayerDeath()
        {
            _levelManager.Reset();
        }

        private void OnLevelCompletion() //todo 
        {
            EventManager.StopListening("OnLevelCompletion",OnLevelCompletion);
            
            _dataManager.SetData(_levelManager.GetCurrentLevel(),_levelManager.getPlayerPoints());
            
            EventManager.TriggerEvent("Save");
            
            EventManager.StartListening("OnLevelCompletion",OnLevelCompletion);
            
            EventManager.TriggerEvent("StartNextLevel");
            
        }

//todo: cambia in modo che il messaggio lo riceve lo stickman controller
        private void CanDash()
        {
            _stickman.CanDash();
            EventManager.StartListening("OnDash",OnDash);
        }
        
    }
