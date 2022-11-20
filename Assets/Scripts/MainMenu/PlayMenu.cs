using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayMenu: MonoBehaviour
    {
        public int totalLevel = 0;
        public int unlockedValue = 0; //last level unlocked
        private int _totalPages=0;
        private int _page=0;
        private int _pageLevels = 9;

        public GameObject nextButton;
        public GameObject prevButton;

        private LevelButton[] _levelButtons;

        private void Start()
        {
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
        }

        public void Refresh()
        {
            _totalPages = totalLevel / _pageLevels;
            int index = _page * _pageLevels;
            for (int i = 0; i < _levelButtons.Length; i++)
            {
                int level = index + i + 1;
                if (level <= totalLevel)
                {
                    _levelButtons[i].gameObject.SetActive(true);
                    _levelButtons[i].SetUp(level,level<=unlockedValue);
                }
                else
                {
                    _levelButtons[i].gameObject.SetActive(false);
                }
            }
            checkButton();
        }
    }
