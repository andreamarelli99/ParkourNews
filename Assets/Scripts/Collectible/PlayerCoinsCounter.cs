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

    private void AddCoin()
    {
        _inkers++;
        _inkersText.text = "Inkers: "+ _inkers;
    }
    
}
