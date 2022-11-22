using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Editor;
using ParkourNews.Scripts;

using TMPro;
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
    

    private int _numberOfLevels;
    
    public void Start()
    
    {
        _levelManager = GameObject.FindObjectOfType<LevelManager>();
        _numberOfLevels = _levelManager.numberOfLevels();
        
        float width = Screen.width;
        float height = Screen.height;

        width = (width * 0.8).ConvertTo<float>();
        height = (height * 0.8).ConvertTo<float>();

        _imageSegmentDim = Mathf.Min(width, height);
        _imageSegmentDim = _imageSegmentDim / _numberOfLevels;
        
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
        SceneManager.LoadScene("MenuSelector");
        
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
        mainMenu.SetActive(true);
    }
    
}