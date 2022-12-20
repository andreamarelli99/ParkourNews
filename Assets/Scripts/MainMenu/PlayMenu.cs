using System;
using System.Collections.Generic;
using ParkourNews.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class PlayMenu: MonoBehaviour
{
    [SerializeField] private GameObject buttonFirstLevel;
    [SerializeField] private GameObject buttonNextPage;
    [SerializeField] private GameObject buttonPrevPage;
    
    
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
            //clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            //set play as the selected object
            EventSystem.current.SetSelectedGameObject(buttonFirstLevel);
            
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
            
            //clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            //set play as the selected object
            if(buttonNextPage.activeSelf)
                EventSystem.current.SetSelectedGameObject(buttonNextPage);
            else
            {
                EventSystem.current.SetSelectedGameObject(buttonPrevPage);
            }
        }

        public void ClickBack()
        {
            _page --;
            Refresh();
            //clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            //set play as the selected object
            if(buttonPrevPage.activeSelf)
                EventSystem.current.SetSelectedGameObject(buttonPrevPage);
            else
            {
                EventSystem.current.SetSelectedGameObject(buttonNextPage);
            }
        }

        public void ClickBackMenu()
        {
            SceneManager.LoadScene("Menu");
        }

        public void Refresh()
        {
            List<Vector2> playerResults;
            if (!_dataManager.IsNewGame())
            {
                unlockedValue = Convert.ToInt32(_dataManager.getLastUnlockedLevel());
                playerResults = _dataManager.getResults();
            }
            else
            {
                unlockedValue = 1;
                playerResults = new List<Vector2>();
                playerResults.Add(new Vector2(1,0));
            }

            _totalPages = _totalLevel / _pageLevels;
            int index = _page * _pageLevels;
            for (int i = 0; i < _levelButtons.Length; i++)
            {
                int level = index + i + 1;
                if (level <= _totalLevel )
                {
                    _levelButtons[i].gameObject.SetActive(true);
                    if(level<=playerResults.Count)
                        _levelButtons[i].SetUp(level,Convert.ToInt32(playerResults[level-1].y),level<=unlockedValue); 
                    else 
                        _levelButtons[i].SetUp(level,0,level<=unlockedValue);
                }
                else
                {
                    _levelButtons[i].gameObject.SetActive(false);
                }
            }
            checkButton();
        }
    }
