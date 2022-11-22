using System.Collections.Generic;
using UnityEngine;

namespace ParkourNews.Scripts
{
    // what you want to save
    [System.Serializable]
    public class GameData
    {
        public int lastLevelUnlocked; // to save the last level that the player has complete
        public List<Vector2> playerResults; // to save for each existing level (level,points) of the level
    }
    
    
    
}