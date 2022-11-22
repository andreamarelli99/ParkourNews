using System;
using System.Collections.Generic;
using ParkourNews.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class PlayMenu: MonoBehaviour
    {
        private int _totalLevel;
        public int unlockedValue = 0; //last level unlocked
        private int _totalPages=0;
        private int _page=0;
        private int _pageLevels = 9;

        public GameObject nextButton;
        public GameObject prevButton;
        public GameObject backButton;

        private LevelManager _levelManager;
        private LevelButton[] _levelButtons;
        private DataManager _dataManager;

        private void Start()
        {
            _levelManager = FindObjectOfType<LevelManager>();
            _totalLevel = _levelManager.numberOfLevels();
            _dataManager = FindObjectOfType<DataManager>();
            
            Refresh();
        }

        void OnEnable()
        {
            _levelButtons = GetComponentsInChildren<LevelButton>();
        }

        private void checkButton()
        {
            prevButton.SetActive(_page>0);
            nextButton.SetActive(_page<_totalPages);
        }

        public void ClickNext()
        {
            Debug.Log("Next");
            _page ++;
            Refresh();
        }

        public void ClickBack()
        {
            _page --;
            Refresh();
        }

        public void ClickBackMenu()
        {
            SceneManager.LoadScene("Menu");
        }

        public void Refresh()
        {
            List<Vector2> playerResults = _dataManager.getResults();
            _totalPages = _totalLevel / _pageLevels;
            int index = _page * _pageLevels;
            for (int i = 0; i < _levelButtons.Length; i++)
            {
                int level = index + i + 1;
                if (level <= _totalLevel && level<=playerResults.Count)
                {
                    _levelButtons[i].gameObject.SetActive(true);
                    _levelButtons[i].SetUp(level,Convert.ToInt32(playerResults[level-1].y),level<=unlockedValue); //todo manage stars
                }
                else
                {
                    _levelButtons[i].gameObject.SetActive(false);
                }
            }
            checkButton();
        }
    }
