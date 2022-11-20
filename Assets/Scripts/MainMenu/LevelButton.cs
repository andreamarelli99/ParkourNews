using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


    public class LevelButton:MonoBehaviour
    {
        public Sprite lockSprite;
        public Text levelText;
        private int _level = 0;
        private Button _button;
        private Image _image;
        
        private void OnEnable()
        {
            _button=GetComponent<Button>();
            _image=GetComponent<Image>();
        }

        public void SetUp(int level, bool isUnlock)
        {
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

        public void OnClick()
        {
            
        }
    }
