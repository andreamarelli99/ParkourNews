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
                gameData.lastLevelUnlocked = 1;
                gameData.playerResults = new List<Vector2>();
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
        
        public void SetData(int cLevel,float plPoints)
        {
            Debug.Log("clevel= "+cLevel+"points= "+plPoints);
            gameData.lastLevelUnlocked = Math.Max(cLevel + 1, gameData.lastLevelUnlocked);

            int stars;
            
                switch (plPoints) { //to assign stars
                
                case var expression when plPoints >=1:
                    stars = 3;
                    break;
                case var expression when plPoints >= ((float)2) / 3:
                    stars = 2;
                    break;
                case var expression when plPoints >= ((float)1) / 3:
                    stars = 1;
                    break;
                default:
                    stars = 0;
                    break;
                
            }
            
            Debug.Log("stars= "+stars);
            
            if(gameData.playerResults.Count<cLevel)
                gameData.playerResults.Add(new Vector2(cLevel,plPoints));
            else if (stars > gameData.playerResults[cLevel-1].y)
                gameData.playerResults[cLevel-1]= new Vector2(cLevel,stars);
        }

        public double getLastUnlockedLevel()
        {
            return gameData.lastLevelUnlocked;
        }

        public List<Vector2> getResults()
        {
            return gameData.playerResults;
        }
    }
}