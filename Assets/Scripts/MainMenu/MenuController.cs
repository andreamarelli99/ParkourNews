using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject playMenu;
    [SerializeField] private GameObject backMenu;
    

    private Button _playButton;
    private Button _optionButton;
    private Button _quitButton;
    private Button _backButton;

    public void Start()
    {
        mainMenu.SetActive(true);
        
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
        playMenu.SetActive(true);
        backMenu.SetActive(true);
    }
    
    public void OnClickOptionButton()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        backMenu.SetActive(true);
        
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
        playMenu.SetActive(false);
        mainMenu.SetActive(true);
        
    }
}