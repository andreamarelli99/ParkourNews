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
        public float musicVolume; // music volume
        public bool musicEnabled; // enable music in game
        public float sfxVolume; // sounds effects volume
        public bool sfxEnabled; // enable sound effects in game
    }
}