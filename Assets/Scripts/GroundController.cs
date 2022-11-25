using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ParkourNews.Scripts
{
    public class GroundController:MonoBehaviour
    {
     /*   private void OnCollisionEnter2D(Collision2D col)
        {
            //check if ground is touched by the stickman
            if (col.gameObject.CompareTag("Stickman")){
                //col.GetContact(0).collider = contact collider
                if(col.GetContact(0).collider.CompareTag("StickmanFoot")){
                    EventManager.TriggerEvent("OnGround");
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
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("GroundCheck")){
                //col.GetContact(0).collider = contact collider
                //      if(col.CompareTag("StickmanFoot"))
                EventManager.TriggerEvent("OnGround");
            }
        }


        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("GroundCheck")){
                //col.GetContact(0).collider = contact collider
                EventManager.TriggerEvent("InAir");
            }
        }
    
    }
}