using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroundControllerRedLine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
 /*   private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("RedLine"))
        {
         //   Destroy(gameObject);
            print("TOCCATO!!");
        }
    }*/
    
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("RedLine")){
            //col.GetContact(0).collider = contact collider
            Destroy(gameObject);
        }
    }

}
