using System;
using ParkourNews.Scripts;
using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameController: MonoBehaviour
{
    private DataManager _dataManager;
    private LevelManager _levelManager;
    private StickmanController _stickman;

    private Vector3 _initialPos;
    
    // dash cooldown value
    [SerializeField] private int secondsBetweenDash=5;
    [SerializeField] private int secondsBetweenSlide=2;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        _levelManager = GameObject.FindObjectOfType<LevelManager>();
        _dataManager = FindObjectOfType<DataManager>();
        
    }
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        EventManager.StartListening("OnLevelCompletion",OnLevelCompletion);
        EventManager.StartListening("OnPlayLevel",OnPlayLevel);
        EventManager.StartListening("OnDash",OnDash);
    }

    public int GetSecondsBetweenDash()
    {
        return secondsBetweenDash;
    }

    // When a level starts "picks" the stickman 
    private void OnPlayLevel()
    {
        _stickman =  GameObject.FindWithTag("Stickman").GetComponent<StickmanController>();
        _initialPos = _stickman.transform.position;
    }
    
    // When the stickman has done the dash action waits a cooldown in order to be able repeat the action
    private void OnDash()
    {
        EventManager.StopListening("OnDash",OnDash);
        Invoke("CanDash", secondsBetweenDash);
        EventManager.StartListening("OnDash",OnDash);
    }
    
    // When the stickman dies the level will restart
    
    private void OnLevelCompletion() //todo 
    {
        EventManager.StopListening("OnLevelCompletion",OnLevelCompletion);
        EventManager.TriggerEvent("StartNextLevel");
        EventManager.StartListening("OnLevelCompletion",OnLevelCompletion);
    }

//todo: cambia in modo che il messaggio lo riceve lo stickman controller
    private void CanDash()
    {
        EventManager.TriggerEvent("CanDash");
        EventManager.StartListening("OnDash",OnDash);
    }
}
