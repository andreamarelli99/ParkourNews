using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideObliqueLeftController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("ObliqueCheck"))
        {
            Debug.Log("slide in");
            EventManager.TriggerEvent("OnSlidingObliqueLeftEnter");
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("ObliqueCheck"))
        {
            Debug.Log("slide out");
            EventManager.TriggerEvent("OnSlidingObliqueExit");
        }
    }
}
