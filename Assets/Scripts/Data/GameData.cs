using System;
using System.Collections.Generic;
using UnityEngine;

namespace ParkourNews.Scripts
{
    // what you want to save
    [Serializable]
    public class GameData
    {
        public int lastLevelUnlocked; // to save the last level that the player has complete
        public List<Vector2> playerResults; // to save for each existing level (level,points) of the level
        public float musicVolume = 0.4f; // music volume
        public bool musicEnabled = true; // enable music in game
        public float sfxVolume = 0.5f; // sounds effects volume
        public bool sfxEnabled = true;// enable sound effects in game
    }
}