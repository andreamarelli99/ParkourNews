using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        //check if a bouncey element is touched by the stickman
        if (col.gameObject.CompareTag("Stickman")){
            EventManager.TriggerEvent("OnCoin"); 
            Destroy(this.gameObject);       
        }
    }
   
}
