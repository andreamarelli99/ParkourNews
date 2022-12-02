using System;
using System.Collections;
using ParkourNews.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


    public class DashCooldownButton:MonoBehaviour
    {
        public Sprite lockSprite;
        private Button _button;
        private Image _image;
        
        private void Start()
        {
            _button=GetComponent<Button>();
            _image=GetComponent<Image>();
            EventManager.StartListening("OnDash",OnDash);
            EventManager.StartListening("CanDash",CanDash);
            _image.sprite = null;
            _button.enabled = true;
        }


        private void OnDash()
        {
            EventManager.StopListening("OnDash",OnDash);
            _image.sprite = lockSprite;
            _button.enabled = false;
            EventManager.StartListening("OnDash",OnDash);
        }

        private void CanDash()
        {
            EventManager.StopListening("CanDash",CanDash);
            _image.sprite = null;
            _button.enabled = true;
            EventManager.StartListening("CanDash",CanDash);
        }
     
        
    }
