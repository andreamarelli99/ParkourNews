using System;
using UnityEngine;

namespace ParkourNews.Scripts
{
    public class DataManager: MonoBehaviour
    {
        public GameData gameData;


        private void Start()
        {
            EventManager.StartListening("Save",Save);
            EventManager.StartListening("Load",Load);
        }


        private void Save()
        {
            
        }

        private void Load()
        {
            
        }
        
    }
}