using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ParkourNews.Scripts
{
    public class DataManager : MonoBehaviour
    {
        [SerializeField] private GameData gameData;
        private string _filePath;
        private bool _newGame = true;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _filePath = Path.Combine(Application.persistentDataPath, "savegame.json");
            if (File.Exists(_filePath))
            {
                Load();
                _newGame = false;
            }
            else
            {
                FileStream oFileStream = null;
                oFileStream = new FileStream(_filePath, FileMode.Create);
                oFileStream.Close();
            }

            EventManager.StartListening("Save", Save);
            EventManager.StartListening("Load", Load);
        }

        public bool IsNewGame()
        {
            return _newGame;
        }

        private void Save()
        {
            File.WriteAllText(_filePath, JsonUtility.ToJson(gameData));
        }

        private void Load()
        {
            gameData = JsonUtility.FromJson<GameData>(File.ReadAllText(_filePath));
        }

        public GameData GetData()
        {
            return gameData;
        }

        public void SetData(int cLevel, float plPoints)
        {
            Debug.Log("clevel= " + cLevel + "points= " + plPoints);
            gameData.lastLevelUnlocked = Math.Max(cLevel + 1, gameData.lastLevelUnlocked);

            int stars;

            switch (plPoints)
            {
                //to assign stars

                case >= 1:
                    stars = 3;
                    break;
                case >= (float)2 / 3:
                    stars = 2;
                    break;
                case >= (float)1 / 3:
                    stars = 1;
                    break;
                default:
                    stars = 0;
                    break;
            }

            Debug.Log("stars= " + stars);

            if (gameData.playerResults.Count < cLevel)
                gameData.playerResults.Add(new Vector2(cLevel, stars));
            else if (stars > gameData.playerResults[cLevel - 1].y)
                gameData.playerResults[cLevel - 1] = new Vector2(cLevel, stars);
        }

        public double getLastUnlockedLevel()
        {
            return gameData.lastLevelUnlocked;
        }

        public List<Vector2> getResults()
        {
            return gameData.playerResults;
        }

        public float GetMusicVolume()
        {
            return gameData.musicVolume;
        }

        public void SetMusicVolume(float volume)
        {
            gameData.musicVolume = volume;
        }

        public float GetSfxVolume()
        {
            return gameData.sfxVolume;
        }

        public void SetSfxVolume(float volume)
        {
            gameData.sfxVolume = volume;
        }

        public bool GetMusicEnabled()
        {
            return gameData.musicEnabled;
        }

        public void SetMusicEnabled(bool musicEnabled)
        {
            gameData.musicEnabled = musicEnabled;
        }

        public bool GetSfxEnabled()
        {
            return gameData.sfxEnabled;
        }

        public void SetSfxEnabled(bool sfxEnabled)
        {
            gameData.sfxEnabled = sfxEnabled;
        }
    }
}