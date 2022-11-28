using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ParkourNews.Scripts
{
    public class WallController:MonoBehaviour
    {
        
        
  /*      private void OnCollisionEnter2D(Collision2D col)
        {
            //check if ground is touched by the stickman
            if (col.gameObject.CompareTag("Stickman")){
                if(col.GetContact(0).collider.CompareTag("StickmanHand"))
                    EventManager.TriggerEvent("OnWall"); 
            }
        }


        private void OnCollisionExit2D(Collision2D col)
        {
            //check if ground is touched by the stickman
            if (col.gameObject.CompareTag("Stickman")){
                //col.GetContact(0).collider = contact collider
                //if(col.GetContact(0).collider.CompareTag("StickmanHand"))
                    EventManager.TriggerEvent("OnJump"); 
            }
        }*/
        /*private bool _onWall = false;
        
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!_onWall)
            {
                if (col.gameObject.CompareTag("Wall")){
                    EventManager.TriggerEvent("OnWall"); 
                    _onWall = true;
                }
            }
            
        }


        private void OnTriggerExit2D(Collider2D col)
        {
            if (_onWall)
            {
                if (col.gameObject.CompareTag("Wall")){
                    EventManager.TriggerEvent("InAir"); 
                    _onWall = false;
                }
            }
            
        }*/
    }
}