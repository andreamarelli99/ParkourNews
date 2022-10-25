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
                //col.GetContact(0).collider = contect collider
                if(col.GetContact(0).collider.CompareTag("StickmanFoot"))
                    EventManager.TriggerEvent("OnGround"); 
            }
        }
    }
}