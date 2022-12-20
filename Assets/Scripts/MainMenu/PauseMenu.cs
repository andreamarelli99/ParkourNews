using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject resumeButton;
    // Start is called before the first frame update
    void Start()
    {
        //clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set play as the selected object
        EventSystem.current.SetSelectedGameObject(resumeButton);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
