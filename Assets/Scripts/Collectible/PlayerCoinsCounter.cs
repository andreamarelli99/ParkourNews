using System;
using TMPro;
using UnityEngine;
public class PlayerCoinsCounter : MonoBehaviour
{
    private int _inkers = 0;

    private GameObject _inkersTextObject;

    private TextMeshProUGUI _inkersText;
    private void OnEnable()
    {
        _inkersTextObject = GameObject.FindGameObjectWithTag("CoinIndex");
        _inkersText = _inkersTextObject.GetComponent<TextMeshProUGUI>();
        EventManager.StartListening("OnCoin", AddCoin);
    }

    private void Start()
    {
        EventManager.StartListening("OnRespawn",OnRespawn);
    }

    private void AddCoin()
    {
        _inkers++;
        _inkersText.text = "Inkers: "+ _inkers;
    }

    private void OnRespawn()
    {
        EventManager.StopListening("OnRespawn",OnRespawn);
        _inkersText.text = "Inkers: 0";
        EventManager.StartListening("OnRespawn",OnRespawn);
    }

}
