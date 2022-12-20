using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject resumeButton;

    private GameObject _lastSelectedEl;
    // Start is called before the first frame update
    void Start()
    {
        //clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set play as the selected object
        EventSystem.current.SetSelectedGameObject(resumeButton);
        _lastSelectedEl = resumeButton;

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
