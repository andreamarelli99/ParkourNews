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
                //col.GetContact(0).collider = contact collider
                if(col.GetContact(0).collider.CompareTag("StickmanFoot")){
          //          EventManager.TriggerEvent("OnGround");
                }
            }
        }
        
        /*    private void OnCollisionExit2D(Collision2D col)
       {
           //check if ground is touched by the stickman
           if (col.gameObject.CompareTag("Stickman")){
               //col.GetContact(0).collider = contact collider
                   EventManager.TriggerEvent("InAir");
                   Debug.Log("HEY!");
           }
       }*/
        private bool _onGround = false;
        
        private bool _onWall = false;
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("GroundCheck"))
            {
                if (!_onGround)
                {
                    EventManager.TriggerEvent("OnGround");
                    _onGround = true;
                    _onWall = false;
                }
            }
            else if (col.gameObject.CompareTag("Wall"))
            {
                if (!_onGround && !_onWall)
                {
                    EventManager.TriggerEvent("OnWall"); 
                    _onWall = true;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("GroundCheck"))
            {
                if (_onGround) 
                {
                    EventManager.TriggerEvent("InAir");
                    _onGround = false;
                    _onWall = false;
                }
            }
            else if (col.gameObject.CompareTag("Wall"))
            {
                Debug.Log("OnGround= " + _onGround);
                if (!_onGround)
                {
                    Debug.Log("In air from wall");
                    EventManager.TriggerEvent("InAir");
                    _onWall = false;
                }
            }
            
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Wall"))
            {
                if (!_onGround && !_onWall)
                {
                    EventManager.TriggerEvent("OnWall");
                    _onWall = true;
                }
            }
            
        }
    }
}