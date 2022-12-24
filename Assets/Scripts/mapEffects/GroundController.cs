using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ParkourNews.Scripts
{
    public class GroundController:MonoBehaviour
    {
        
        [SerializeField] private bool _onGround = false;
        
        [SerializeField] private bool _onWall = false;

        [SerializeField] private bool _canJump = true;

        private float _coyoteTime = 0.1f;
        private float _timer = 0;
        private bool _coyoteTimerOn = false;

        private void FixedUpdate()
        {
            if (_coyoteTimerOn)
            {
                _timer += Time.deltaTime;
                
                if (_timer >= _coyoteTime)
                {
                    if (!_onGround && !_onWall)
                    {
                        EventManager.TriggerEvent("InAir");
                    }
                    
                    _coyoteTimerOn = false;
                    _timer = 0;

                }
            }
            
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            //check if ground is touched by the stickman
            if (col.gameObject.CompareTag("Stickman")){
                if(col.GetContact(0).collider.CompareTag("StickmanFoot")){
                    EventManager.TriggerEvent("WalkingSound");
                }
            }
        }
        
        private void OnEnable()
        {
            EventManager.StartListening("WallJumpMessage", WallJumpMessage);
            EventManager.StartListening("OnDeath", ResetValues);
        }

        private void WallJumpMessage()
        {
      //      EventManager.StopListening("WallJumpMessage", WallJumpMessage);
            _canJump = true;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("GroundCheck"))
            {
                if (!_onGround)
                {
                    EventManager.TriggerEvent("OnGround");
                    EventManager.TriggerEvent("WallJumpMessage");
                    _onGround = true;
                }

            }
            else if (col.gameObject.CompareTag("Wall"))
            {
                if (_canJump)
                {
                    if (!_onWall)
                    {
                        if (!_onGround)
                        {
                            EventManager.TriggerEvent("OnWall");
                        }
                        _onWall = true;
                        EventManager.TriggerEvent("WallJumpMessage");
                    }
                }
                else
                {
                    if (!_onWall)
                    {
                        if (!_onGround)
                        {
                            EventManager.TriggerEvent("OnWallButCantJump");
                        }
                        _onWall = true;
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("GroundCheck"))
            {
                if (_onGround && !_onWall)
                {
                    _coyoteTimerOn = true;
                    _onWall = false;
                    _onGround = false;
                }
                else if (_onWall)
                {
                    EventManager.TriggerEvent("OnWall"); 
                    _onGround = false;
                }
                
            }
            else if (col.gameObject.CompareTag("Wall"))
            {
                if (!_onGround)
                {
                    Debug.Log("In air from wall");
                    EventManager.TriggerEvent("InAir");
                    EventManager.TriggerEvent("WallJumpMessage");
                }
                _onWall = false;
                _canJump = false;
            }
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("GroundCheck"))
            {
                EventManager.TriggerEvent("OnGround");
                EventManager.TriggerEvent("WallJumpMessage");
                _onGround = true;
            }
            if (col.gameObject.CompareTag("Wall")&&_canJump)
            {
                if (!_onGround)
                {
                    EventManager.TriggerEvent("OnWall");
                }
            }
            
        }

        private void ResetValues()
        {
            _onGround = false;
            _onWall = false;
            _canJump = true;
        }
    }
}