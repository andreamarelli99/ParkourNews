using UnityEngine;

namespace ParkourNews.Scripts
{
    public class TranslateUpDown : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float movement;
        [SerializeField] private bool upDown;
        private float initial_position ;
        private float end_position;
        private bool moveUp;

        void Start()
        {
           //moveUp = true;
            initial_position = transform.position.y;
            if (upDown)
            {
                moveUp = true;;
                end_position = initial_position + movement;
            }
            else
            {
                moveUp = false;
                end_position = initial_position - movement;
            }
        }
        
        // Update is called once per frame
        void Update()
        {
            if (upDown)
            {
                if (transform.position.y > end_position) moveUp = false;
                if (transform.position.y < initial_position) moveUp = true;
            }
            else
            {
                if (transform.position.y < end_position) moveUp = true;
                if (transform.position.y > initial_position) moveUp = false;
            }

            if (moveUp)
                transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);
            else
                transform.position = new Vector2(transform.position.x, transform.position.y - speed * Time.deltaTime);
        }
    }
}