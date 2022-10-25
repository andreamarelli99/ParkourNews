using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ParkourNews.Scripts
{
    public class GroundController:MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D col)
        {
            //check if ground is touched by the stickman
            if (col.gameObject.CompareTag("Stickman")){
                EventManager.TriggerEvent("OnGround"); 
            }
        }
    }
}