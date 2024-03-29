﻿using UnityEngine;

namespace ParkourNews.Scripts
{
    public class BounceyController:MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D col)
        {
            //check if a bouncey element is touched by the stickman
            if (col.gameObject.CompareTag("Stickman")){
                EventManager.TriggerEvent("OnBouncey"); 
            }
        }
    }
}