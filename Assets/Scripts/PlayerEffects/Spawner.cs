using System.Collections;
using System.Linq;
using ParkourNews.Scripts;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour, ISingleton
{
    [SerializeField] private GameObject _drop;
    [SerializeField] private GameObject _spawnEffect;
    [SerializeField] private GameObject _stickman;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject levelMenu;
    [SerializeField] private GameObject endMenu;
    
    private LevelManager _levelManager;
    private int _currentLevel;
    private int _nextLevel;
    private int _maxLevel;

    private SpriteRenderer[] _stickmanSprite;

    private Vector2 _stickmanPos;
    
    private bool _pauseIsOn;
    
    private Transform _transform;
    private bool _stickmanCreated = false;
    private Vector2 _initialPosition;

    private StickmanActions _stickmanActions;
    [SerializeField] float _timeLeft;

    private Vector3 _position;
    // public AudioClip soundEffect;

    
    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _stickmanActions = new StickmanActions();
    }
    
    // Start is called before the first frame update
    void Start()
    {
    //    ExecuteSpawnEffect();
        _stickmanSprite = GetComponentsInChildren<SpriteRenderer>();
        SpawnDrop();
        _pauseIsOn = false;
        _initialPosition = _transform.position;
        pauseMenu.SetActive(false);
        endMenu.SetActive(false);

        _levelManager = FindObjectOfType<LevelManager>();
        
        EventManager.StartListening("OpenMenu",OnOpenMenu);
        EventManager.StartListening("EndMenu",OnEndMenu);
    }
    
    private void OnEndMenu()
    {
        Debug.Log("EndMenu");
        EventManager.StopListening("EndMenu",OnEndMenu);
        endMenu.SetActive(true);
        Time.timeScale = 0f;
        _currentLevel = _levelManager.GetCurrentLevel();
        _nextLevel = _levelManager.GetNextLevel();
        _maxLevel = _levelManager.numberOfLevels();
        EventManager.StartListening("EndMenu",OnEndMenu);
        if(_currentLevel>=_maxLevel)
            GameObject.FindWithTag("NextLevelButton").SetActive(false);
    }
    
    private void OnEnable()
    {
       // _stickmanActions.Enable();
       
        EventManager.StartListening("OnDeath", OnDeath);
        
    }
    
    

    private void OnDisable()
    {
      
        _stickmanActions.Disable();
    }


    private void OnOpenMenu()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void LevelSelector()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MenuSelector");
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(_nextLevel.ToString());
    }

    public void Redo()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(_currentLevel.ToString());
    }
    
    
    
    public void Quit()
    {
        Application.Quit();
    }
    
    
    
    private void OnDeath()
    {
        EventManager.StopListening("OnDeath",OnDeath);
        _stickmanCreated = false;
        StartCoroutine(RespawnCoroutine());
        EventManager.StartListening("OnDeath",OnDeath);
    }
    
    
    private IEnumerator RespawnCoroutine()
    {
        EventManager.TriggerEvent("OnRespawn"); 
        yield return new WaitForSeconds(2f);
        if (!_stickmanCreated)
        {
            SetPosition(_initialPosition);
            SpawnDrop();
        }
    }


    public void SetPosition(Vector2 stickmanTransform)
    {
        var spawnerTransform = transform;
        spawnerTransform.position = new Vector3(stickmanTransform.x, stickmanTransform.y, spawnerTransform.position.z);
        
    }

    public void SpawnDrop()
    {
    //    var dropTransform = transform;
        Instantiate(_drop, new Vector3(_transform.position.x, _transform.position.y+13, _transform.position.z), Quaternion.identity);
    }

    public void ExecuteSpawnEffect(Transform explosion)
    {
        _position = new Vector3((float)(0.13878 +_transform.position.x), (float)(0.62967 + _transform.position.y), _transform.position.z);
        Instantiate(_spawnEffect, _position , explosion.rotation);
        StartCoroutine(SpawnStickManCoroutine());
        //  AudioSource.PlayClipAtPoint(soundEffect, transform.position);
    }

    private IEnumerator SpawnStickManCoroutine()
    {
        yield return new WaitForSeconds(_timeLeft);
        Instantiate(_stickman, new Vector3(_transform.position.x, _transform.position.y, _transform.position.z), Quaternion.identity, gameObject.transform.parent);
        _stickmanCreated = true;
    }

    
}
