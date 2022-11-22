using System;
using ParkourNews.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayMenu: MonoBehaviour
    {
        private int _totalLevel;
        public int unlockedValue = 0; //last level unlocked
        private int _totalPages=0;
        private int _page=0;
        private int _pageLevels = 9;

        public GameObject nextButton;
        public GameObject prevButton;

        private LevelManager _levelManager;
        private LevelButton[] _levelButtons;

        private void Start()
        {
            _levelManager = FindObjectOfType<LevelManager>();
            _totalLevel = _levelManager.numberOfLevels();
            
            
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
            SceneManager.LoadScene("0");
            EventManager.TriggerEvent("OnPlayLevel");
        }

        public void Refresh()
        {
            _totalPages = _totalLevel / _pageLevels;
            int index = _page * _pageLevels;
            for (int i = 0; i < _levelButtons.Length; i++)
            {
                int level = index + i + 1;
                if (level <= _totalLevel)
                {
                    _levelButtons[i].gameObject.SetActive(true);
                    _levelButtons[i].SetUp(level,2,level<=unlockedValue); //todo manage stars
                }
                else
                {
                    _levelButtons[i].gameObject.SetActive(false);
                }
            }
            checkButton();
        }
    }
