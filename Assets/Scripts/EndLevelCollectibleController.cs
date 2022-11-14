using Unity.VisualScripting;
using UnityEngine;

namespace ParkourNews.Scripts
{
    public class EndLevelCollectibleController: MonoBehaviour
    {
        
        
        
        //todo: when the stickman collides with an end level collectibles a new level will start (if it does exist)
        // for now we've just to save
        private void OnCollisionEnter2D(Collision2D col)
        {
            //check if a bouncey element is touched by the stickman
            if (col.gameObject.CompareTag("Stickman")){
                EventManager.TriggerEvent("OnLevelCompletion");
                Destroy(this.gameObject);       
            }
        }
        
    }
}