using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MainMenu.MenuNavigation
{
    public class ButtonColoring: MonoBehaviour
    {
        [SerializeField] private Color buttonColor =  new Color32(179,179,179,168);
        [SerializeField] private Color buttonSelectedColor = new Color32(214, 167, 148, 255);
       
        private ColorBlock _colors;
        private Button _button;
        
        private void Start()
        {
            _button=GetComponent<Button>();
            _colors = _button.colors;
            _colors.normalColor = buttonColor;
            _colors.selectedColor = buttonSelectedColor;
            _button.colors = _colors;
        }
        
    }
}