using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

using ParkourNews.Scripts;

using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EndMenuController : MonoBehaviour
{
    [SerializeField] private GameObject playMenu;
    [SerializeField] private GameObject quitButton;
    

    private float _imageSegmentDim;
    
    private Button _playButton;
    private Button _quitButton;

    
    [SerializeField] private GameObject firstSelectedButtonMainMenu;
    private GameObject _lastSelectedEl;
    
    private int _numberOfLevels;
    
    public void Start()
    
    {
       
        
        float width = Screen.width;
        float height = Screen.height;

        width = (width * 0.8).ConvertTo<float>();
        height = (height * 0.8).ConvertTo<float>();

        _imageSegmentDim = Mathf.Min(width, height);
        _imageSegmentDim = _imageSegmentDim / _numberOfLevels;
        
        
        //clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set play as the selected object
        EventSystem.current.SetSelectedGameObject(firstSelectedButtonMainMenu);
        _lastSelectedEl = firstSelectedButtonMainMenu;

        _playButton = GameObject.FindWithTag("PlayButton").GetComponent<Button>();
        _playButton.onClick.AddListener(OnClickPlayButton);
        
        
        _playButton = GameObject.FindWithTag("QuitButton").GetComponent<Button>();
        _playButton.onClick.AddListener(OnClickQuitButton);
        
        
    }

    public void OnClickPlayButton()
    {
        SceneManager.LoadScene("MenuSelector");
        
    }
    
    public void OnClickQuitButton()
    {
        Application.Quit();
    }

   
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