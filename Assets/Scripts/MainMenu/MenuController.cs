using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

using ParkourNews.Scripts;

using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject playMenu;
    [SerializeField] private GameObject backMenu;
    

    private float _imageSegmentDim;
    
    private Button _playButton;
    private Button _optionButton;
    private Button _quitButton;
    private Button _backButton;
    private LevelManager _levelManager;

    [SerializeField] private GameObject keyboardSettings;
    [SerializeField] private GameObject XBOXSettings;
    [SerializeField] private GameObject PS4Settings;
    [SerializeField] private Button rightSettingsButton;
    [SerializeField] private Button leftSettingsButton;

    [SerializeField] private GameObject firstSelectedButtonMainMenu;
    [SerializeField] private GameObject firstSelectedButtonOptions;
    [SerializeField] private GameObject firstSelectedButtonExitOptions;
    private GameObject _lastSelectedEl;
    

    private int _numberOfLevels;
    
    public void Start()
    
    {
        rightSettingsButton.onClick.AddListener(RightButton);
        leftSettingsButton.onClick.AddListener(LeftButton);
        
        keyboardSettings.SetActive(true);
        _levelManager = GameObject.FindObjectOfType<LevelManager>();
        _numberOfLevels = _levelManager.numberOfLevels();
        
        float width = Screen.width;
        float height = Screen.height;

        width = (width * 0.8).ConvertTo<float>();
        height = (height * 0.8).ConvertTo<float>();

        _imageSegmentDim = Mathf.Min(width, height);
        _imageSegmentDim = _imageSegmentDim / _numberOfLevels;
        
        mainMenu.SetActive(true);
        //clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set play as the selected object
        EventSystem.current.SetSelectedGameObject(firstSelectedButtonMainMenu);
        _lastSelectedEl = firstSelectedButtonMainMenu;

        _playButton = GameObject.FindWithTag("PlayButton").GetComponent<Button>();
        _playButton.onClick.AddListener(OnClickPlayButton);
        
        _playButton = GameObject.FindWithTag("OptionsButton").GetComponent<Button>();
        _playButton.onClick.AddListener(OnClickOptionButton);
        
        _playButton = GameObject.FindWithTag("QuitButton").GetComponent<Button>();
        _playButton.onClick.AddListener(OnClickQuitButton);
        
        _backButton = GameObject.FindWithTag("BackButton").GetComponent<Button>();
        _backButton.onClick.AddListener(OnClickBackButton);
        backMenu.SetActive(false);
        
    }

    public void OnClickPlayButton()
    {
        mainMenu.SetActive(false);
        SceneManager.LoadScene("MenuSelector");
        
    }
    
    public void OnClickOptionButton()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        backMenu.SetActive(true);
        
        //clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set play as the selected object
        EventSystem.current.SetSelectedGameObject(firstSelectedButtonOptions);
        
        _backButton = GameObject.FindWithTag("BackButton").GetComponent<Button>();
        _playButton.onClick.AddListener(OnClickBackButton);
    }
    
    public void OnClickQuitButton()
    {
        Application.Quit();
    }

    public void OnClickBackButton()
    {
        backMenu.SetActive(false);
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
        
        //clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set play as the selected object
        EventSystem.current.SetSelectedGameObject(firstSelectedButtonExitOptions);
    }

    public void RightButton()
    {
        if (keyboardSettings.activeSelf)
        {
            keyboardSettings.SetActive(false);
            XBOXSettings.SetActive(true);
        }
        else if (XBOXSettings.activeSelf)
        {
            XBOXSettings.SetActive(false);
            PS4Settings.SetActive(true);
        }
        else
        {
            PS4Settings.SetActive(false);
            keyboardSettings.SetActive(true);
        }
    }

    public void LeftButton()
    {
        if (PS4Settings.activeSelf)
        {
            PS4Settings.SetActive(false);
            XBOXSettings.SetActive(true);
        }
        else if (keyboardSettings.activeSelf)
        {
            keyboardSettings.SetActive(false);
            PS4Settings.SetActive(true);
        }
        else
        {
            XBOXSettings.SetActive(false);
            keyboardSettings.SetActive(true);
        }
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