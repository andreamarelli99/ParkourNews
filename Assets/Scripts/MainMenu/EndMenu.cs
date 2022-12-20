using UnityEngine;
using UnityEngine.EventSystems;

namespace MainMenu
{
    public class EndMenu : MonoBehaviour
    {
        [SerializeField] private GameObject nextLevelButton;

        // Start is called before the first frame update
        void Start()
        {
            //clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            //set play as the selected object
            EventSystem.current.SetSelectedGameObject(nextLevelButton);

        }

        // Update is called once per frame
        void Update()
        {

        }
        
    }
}