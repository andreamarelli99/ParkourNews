using UnityEngine;
using UnityEngine.UI;


    public class LevelStar: MonoBehaviour
    {
        public Sprite starsSprite0;
        public Sprite starsSprite1;
        public Sprite starsSprite2;
        public Sprite starsSprite3;

        public Image image;

        public void SetStarsSprite(int starAmount)
        {
            switch (starAmount)
            {
                case 0:
                    image.sprite = starsSprite0;
                    break;
                case 1:
                    image.sprite = starsSprite1;
                    break;
                case 2:
                    image.sprite = starsSprite2;
                    break;
                case 3:
                    image.sprite = starsSprite3;
                    break;
            }
        }
    }
