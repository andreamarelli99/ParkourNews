using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ParkourNews.Scripts
{
    public class DataManager: MonoBehaviour
    {
        [SerializeField] private GameData gameData;
        private String _filePath;

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        private void  Start()
        {
            
            _filePath = Path.Combine(Application.persistentDataPath, "savegame.json");
            if (File.Exists(_filePath)) 
                gameData = JsonUtility.FromJson<GameData>(File.ReadAllText(_filePath));
            else
            {
                System.IO.FileStream oFileStream = null;
                oFileStream = new System.IO.FileStream(_filePath, System.IO.FileMode.Create);
                oFileStream.Close ();
            }
            
            EventManager.StartListening("Save",Save);
            EventManager.StartListening("Load",Load);
        }

        
        private void Save()
        {
            File.WriteAllText(_filePath,JsonUtility.ToJson(gameData)); 
        }

        private void Load()
        {
            gameData = JsonUtility.FromJson<GameData>(File.ReadAllText(_filePath));
        }

        public GameData GetData()
        {
            return gameData;
        }
        
        public void SetData(GameData gd)
        {
            gameData = gd;
        }

        public int getLastUnlockedLevel()
        {
            return gameData.lastLevelUnlocked;
        }

        public List<Vector2> getResults()
        {
            return gameData.playerResults;
        }
    }
}