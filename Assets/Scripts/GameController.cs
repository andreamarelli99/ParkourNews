using System;
using ParkourNews.Scripts;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class GameController: MonoBehaviour
    {
        private LevelManager _levelManager;
        
        private StickmanController _stickman;
        private Vector2 _initialPosition;
        
        private GameData _stickmanData;
        private String _filePath;
        
        [SerializeField] private int secondsBetweenDash=5;
       

        void Awake()
        {
            _levelManager = GameObject.FindObjectOfType<LevelManager>();
            _stickmanData = new GameData(); 
            
            // load the save file or build a new one
            _filePath = Path.Combine(Application.persistentDataPath, "savegame.json");
            if (File.Exists(_filePath)) 
                _stickmanData = JsonUtility.FromJson<GameData>(File.ReadAllText(_filePath));
            else
            {
                File.Create(_filePath);
                _stickmanData.lastLevelCompleted = 0;
                _stickmanData.playerResults = new Vector2(0, 0); //todo
            }
            
        }
        
        private void Start()
        {
            _stickman =  GameObject.FindWithTag("Stickman").GetComponent<StickmanController>();
            EventManager.StartListening("OnDash",OnDash);
            EventManager.StartListening("OnLevelCompletion",OnLevelCompletion);
            EventManager.StartListening("OnPlayerDeath",OnPlayerDeath);
            
            
            

          //  int gameLevel = _levelManager.GetLevel();
           // SceneManager.LoadScene(gameLevel);
          
        }

        public void OnEnable()
        {
            _initialPosition = FindObjectOfType<StickmanController>().transform.position;
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
            //-------------------------------saving-------------------------------------//
            _stickmanData.playerResults = new Vector2(_levelManager.GetLevel(),2); //placeholder for now
            _stickmanData.lastLevelCompleted = _levelManager.GetLevel();
            
            File.WriteAllText(_filePath,JsonUtility.ToJson(_stickmanData)); 
            
            Debug.Log(_stickmanData.playerResults.y);
            
            PlayNextLevel();
            
            //-------------------------------------------------------------------------//
            EventManager.StartListening("OnLevelCompletion",OnLevelCompletion);
        }


        private void CanDash()
        {
            _stickman.CanDash();
            EventManager.StartListening("OnDash",OnDash);
        }
        
    }
