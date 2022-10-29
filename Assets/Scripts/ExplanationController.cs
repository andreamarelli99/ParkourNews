using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ExplanationController : MonoBehaviour
{
    private Boolean _text;

    private void OnEnable()
    {
        _text = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Enter");
        if (col.gameObject.CompareTag("Stickman"))
            _text = true;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        Debug.Log("Exit");
        if (col.gameObject.CompareTag("Stickman"))
            _text = false;
    }

    public void OnGUI()
    {
        if(_text)GUI.Box(new Rect(0,0,100,100),"Press E ");
    }
}
