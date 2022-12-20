using System;
using System.Collections;
using ParkourNews.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


    public class LevelButton:MonoBehaviour
    {
        public Sprite lockSprite;
        public Text levelText;
        private int _level = 0;
        private Button _button;
        private ColorBlock _colors;
        public GameObject levelStarPrefab;
        private Image _image;
        [SerializeField] private Color buttonColor =  new Color32(179,179,179,70);
        [SerializeField] private Color buttonSelectedColor = new Color32(160, 251, 232, 255);
        private LevelStar _levelStar;
        private LevelManager _levelManager;

        private void OnEnable()
        {
            _button=GetComponent<Button>();
            _image=GetComponent<Image>();
            _levelStar = Instantiate(levelStarPrefab, gameObject.transform).GetComponent<LevelStar>();
            
            _button.onClick.AddListener(OnLevelButtonClick);
        }

        private void Start()
        {
            _levelManager = FindObjectOfType<LevelManager>();
        }

        public void SetUp(int level,int stars, bool isUnlock)
        {
            levelText.alignment=TextAnchor.MiddleCenter;
            _colors = _button.colors;
            _colors.normalColor = buttonColor;
            _colors.selectedColor = buttonSelectedColor;
            _button.colors = _colors;
            
            
            this._level = level;
            levelText.text = level.ToString();

            // check if the player has unlock the level
            if (isUnlock)
            {
                _image.sprite = null;
                _button.enabled = true;
                //show the level text
                levelText.gameObject.SetActive(true);
                _levelStar.SetStarsSprite(stars);
                _levelStar.gameObject.SetActive(true);
            }
            else
            {
                _image.sprite = lockSprite;
                _button.enabled = false;
                //hide the level text
                levelText.gameObject.SetActive(false);
                _levelStar.gameObject.SetActive(false);
            }
        }

        public void OnLevelButtonClick()
        {
            _levelManager.setCurrentLevel(Convert.ToInt32(levelText.text));
            SceneManager.LoadScene(levelText.text);
            
        }
        
    }
