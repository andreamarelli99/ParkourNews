using UnityEngine;

namespace ParkourNews.Scripts
{
    [CreateAssetMenu(menuName = "ParkourNews/GameLevel")]
    public class ParkourNewsLevel: ScriptableObject
    {
        public int numberOfCoins;
        public int numberOfEndLevelCollectibles;
    }
}