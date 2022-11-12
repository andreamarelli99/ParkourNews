using System;
using System.Collections;
using System.Diagnostics.Tracing;
using ParkourNews.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;

    public class GameController: MonoBehaviour
    {
        private LevelManager _levelManagers;
        private StickmanController _stickman;
        [SerializeField] private int secondsBetweenDash=5;
        private GameData _stickmanData;

        private String _filePath;

        private void Start()
        {
            _stickmanData = new GameData(); 
            //LOAD
            _filePath = Path.Combine(Application.persistentDataPath, "savegame.json");
            if (File.Exists(_filePath)) 
                _stickmanData = JsonUtility.FromJson<GameData>(File.ReadAllText(_filePath));
            else
            {
                File.Create(_filePath);
                _stickmanData.lastLevelCompleted = 0;
                _stickmanData.playerResults = new Vector2(0, 0);
            }
            //END LOAD
           
            _stickman =  GameObject.FindWithTag("Stickman").GetComponent<StickmanController>();
            EventManager.StartListening("OnDash",OnDash);
            EventManager.StartListening("OnLevelCompletion",OnLevelCompletion);
        }
        
        private void OnDash()
        {
            EventManager.StopListening("OnDash",OnDash);
            Invoke("CanDash", secondsBetweenDash);
        }

        private void OnLevelCompletion() //todo 
        {
            EventManager.StopListening("OnLevelCompletion",OnLevelCompletion);
            //-------------------------------saving-------------------------------------//
            _stickmanData.playerResults = new Vector2(1,2); //placeholder for now
            _stickmanData.lastLevelCompleted = 1;
            
            File.WriteAllText(_filePath,JsonUtility.ToJson(_stickmanData)); 
            
            Debug.Log(_stickmanData.playerResults.y);
            
            //-------------------------------------------------------------------------//
            EventManager.StartListening("OnLevelCompletion",OnLevelCompletion);
        }


        private void CanDash()
        {
            _stickman.CanDash();
            EventManager.StartListening("OnDash",OnDash);
        }
        
    }
