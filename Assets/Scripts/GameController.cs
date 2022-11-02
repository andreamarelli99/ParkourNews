using System;
using System.Collections;
using System.Diagnostics.Tracing;
using ParkourNews.Scripts;
using Unity.VisualScripting;
using UnityEngine;


    public class GameController: MonoBehaviour
    {
        private LevelManager _levelManagers;
        private StickmanController _stickman;
        [SerializeField] private int _secondsBetweenDash=5;

        private void Start()
        {
            _stickman =  GameObject.FindWithTag("Stickman").GetComponent<StickmanController>();
            EventManager.StartListening("OnDash",OnDash);
        }
        
        private void OnDash()
        {
            EventManager.StopListening("OnDash",OnDash);
            Invoke("CanDash", _secondsBetweenDash);
        }


        private void CanDash()
        {
            _stickman.CanDash();
            EventManager.StartListening("OnDash",OnDash);
        }
        
    }
