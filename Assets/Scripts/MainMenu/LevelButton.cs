using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


    public class LevelButton:MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Sprite lockSprite;
        public Text levelText;
        private int _level = 0;
        private Button _button;
        private ColorBlock _colors;
        private Image _image;
        private Color buttonColor = Color.white;
        private Color buttonTextColor = Color.black;
        private Color buttonSelectedColor = Color.green;
        private Color buttonHoverColor = Color.black;
        private Color buttonHoverTextColor=Color.white;
        
        private void OnEnable()
        {
            _button=GetComponent<Button>();
            _image=GetComponent<Image>();
            
            _button.onClick.AddListener(OnLevelButtonClick);
        }

        public void SetUp(int level, bool isUnlock)
        {
            levelText.alignment=TextAnchor.MiddleCenter;
            _colors = _button.colors;
            _colors.normalColor = buttonColor;
            _colors.pressedColor = buttonSelectedColor;
            _colors.highlightedColor = buttonHoverColor;
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
            }
            else
            {
                _image.sprite = lockSprite;
                _button.enabled = false;
                //hide the level text
                levelText.gameObject.SetActive(false);
            }
        }

        public void OnLevelButtonClick()
        {
            SceneManager.LoadScene(levelText.text);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            levelText.color = buttonHoverTextColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            levelText.color = buttonTextColor;
        }
    }
