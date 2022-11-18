using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColoraMouse : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private TextMeshPro _myText;
    //private Button _myButton;
    private Renderer _renderer;
    
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        //_myButton = GetComponent<Button>();
        _myText = GetComponentInChildren<TextMeshPro>();
        Debug.Log(_myText);
        //_myButton.OnPointerEnter(Hoover());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    


    public void OnPointerEnter(PointerEventData eventData)
    {
        _myText.color = Color.red;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _myText.color = Color.black;
    }
}
