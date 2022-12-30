using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingLettersGroundController : MonoBehaviour
{
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
                EventManager.TriggerEvent("InAir");
                _coyoteTimerOn = false;
                _timer = 0;

            }
        }
            
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Stickman"))
        {
            EventManager.TriggerEvent("OnGround");
            _coyoteTimerOn = false;
            _timer = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Stickman"))
        {
            _coyoteTimerOn = true;
        }
    }
}
