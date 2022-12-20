using UnityEngine;
using UnityEngine.EventSystems;

namespace MainMenu
{
    public class EndMenu : MonoBehaviour
    {
        [SerializeField] private GameObject nextLevelButton;
        private GameObject _lastSelectedEl;

        // Start is called before the first frame update
        void Start()
        {
            //clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            //set play as the selected object
            EventSystem.current.SetSelectedGameObject(nextLevelButton);
            _lastSelectedEl = nextLevelButton;

        }

        // Update is called once per frame
        void Update()
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(_lastSelectedEl);
            }
            else
            {
                _lastSelectedEl = EventSystem.current.currentSelectedGameObject;
            }
        }
        
    }
}