using System;
using ParkourNews.Scripts;
using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameController: MonoBehaviour
{
    private DataManager _dataManager;
    private GameData _stickmanData;
        
        
        private LevelManager _levelManager;
        
        private StickmanController _stickman;
        private Vector2 _initialPosition;
        
        
        
        
        [SerializeField] private int secondsBetweenDash=5;
       

        void Awake()
        {
           // SceneManager.LoadScene("0");
            _levelManager = GameObject.FindObjectOfType<LevelManager>();
            _dataManager = FindObjectOfType<DataManager>();
            
        }
        
        private void Start()
        {
            
            _stickmanData = _dataManager.GetData();
            
            
            
            
            EventManager.StartListening("OnLevelCompletion",OnLevelCompletion);
            
            
          //  _stickman =  GameObject.FindWithTag("Stickman").GetComponent<StickmanController>();
          //  EventManager.StartListening("OnDash",OnDash);
            
          //  EventManager.StartListening("OnPlayerDeath",OnPlayerDeath);
            
            
            

          //  int gameLevel = _levelManager.GetLevel();
           // SceneManager.LoadScene(gameLevel);
          
        }

        public void OnEnable()
        {
           // _initialPosition = FindObjectOfType<StickmanController>().transform.position;
        }
        
        public void PlayNextLevel()
        {
            int gameLevel = _levelManager.GetNextLevel();
            SceneManager.LoadScene(gameLevel.ToString()); //to avoid unity _bug :(
            Debug.Log(gameLevel);
        }

        private void OnDash()
        {
            EventManager.StopListening("OnDash",OnDash);
            Invoke("CanDash", secondsBetweenDash);
        }
        
        private void OnPlayerDeath()
        {
            // move this
            FindObjectOfType<StickmanController>().transform.position = _initialPosition;
            //
        }

        private void OnLevelCompletion() //todo 
        {
            EventManager.StopListening("OnLevelCompletion",OnLevelCompletion);
            
            _stickmanData.playerResults.Add( new Vector2(_levelManager.GetLevel(),2)); //placeholder for now
            _stickmanData.lastLevelUnlocked = _levelManager.GetLevel();
            _dataManager.SetData(_stickmanData);
            EventManager.TriggerEvent("Save");
            
            EventManager.StartListening("OnLevelCompletion",OnLevelCompletion);
            
            PlayNextLevel();
        }


        private void CanDash()
        {
            _stickman.CanDash();
            EventManager.StartListening("OnDash",OnDash);
        }
        
    }
