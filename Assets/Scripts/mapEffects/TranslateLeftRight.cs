using UnityEngine;

namespace ParkourNews.Scripts
{
    public class TranslateLeftRight : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float movement;
        [SerializeField] private bool leftRight;
        private float initial_position ;
        private float end_position;
        private bool moveLeft;

        void Start()
        {
            //moveUp = true;
            initial_position = transform.position.x;
            if (leftRight)
            {
                moveLeft = true;;
                end_position = initial_position - movement;
            }
            else
            {
                moveLeft = false;
                end_position = initial_position + movement;
            }
        }
        
        // Update is called once per frame
        void Update()
        {
            if (leftRight)
            {
                if (transform.position.x < end_position) moveLeft = false;
                if (transform.position.x > initial_position) moveLeft = true;
            }
            else
            {
                if (transform.position.x > end_position) moveLeft = true;
                if (transform.position.x < initial_position) moveLeft = false;
            }

            if (moveLeft)
                transform.position = new Vector2(transform.position.x- speed * Time.deltaTime, transform.position.y );
            else
                transform.position = new Vector2(transform.position.x+ speed * Time.deltaTime, transform.position.y );
        }
    }
}