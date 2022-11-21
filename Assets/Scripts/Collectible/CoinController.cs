using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CoinController : MonoBehaviour
{
    private int _inkers = 0;

    private GameObject _inkersTextObject;

    private TextMeshProUGUI _inkersText;

    private void Awake()
    {
      //  _inkersTextObject = GameObject.FindGameObjectWithTag("CoinIndex");
      //  _inkersText = _inkersTextObject.GetComponent<TextMeshProUGUI>();
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Coin")){
            EventManager.TriggerEvent("OnCoin"); 
            Destroy(col.gameObject);
            _inkers++;
            _inkersText.text = "Inkers: "+ _inkers;
        }
    }

}
