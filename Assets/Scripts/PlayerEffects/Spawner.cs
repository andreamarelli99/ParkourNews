using System.Collections;
using ParkourNews.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Spawner : MonoBehaviour, ISingleton
{
    [SerializeField] private GameObject _drop;
    [SerializeField] private GameObject _spawnEffect;
    [SerializeField] private GameObject _stickman;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject levelMenu;
    [SerializeField] private GameObject endMenu;
    [SerializeField] private Image image;
    [SerializeField] private GameObject infoCanvas;
    
    public Sprite starsSprite0;
    public Sprite starsSprite1;
    public Sprite starsSprite2;
    public Sprite starsSprite3;
    
    private LevelManager _levelManager;
    private DataManager _dataManager;
    private int _currentLevel;
    private int _nextLevel;
    private int _maxLevel;

    private SpriteRenderer[] _stickmanSprite;

    private Vector2 _stickmanPos;
    
    private bool _pauseIsOn;
    
    private Transform _transform;
    public static bool _stickmanCreated = false;
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
        //SpawnDrop();
        _pauseIsOn = false;
        _initialPosition = _transform.position;
        
        pauseMenu.SetActive(false);
        endMenu.SetActive(false);

        _levelManager = FindObjectOfType<LevelManager>();
        
        EventManager.StartListening("OpenMenu",OnOpenMenu);
        EventManager.StartListening("EndMenu",OnEndMenu);
        EventManager.StartListening("SpawnStickman",OnSpawnStickman);
    }

    private void OnEndMenu()
    {
        _currentLevel = _levelManager.GetCurrentLevel();
        _nextLevel = _levelManager.GetNextLevel();
        _maxLevel = _levelManager.numberOfLevels();
        
        Debug.Log(_currentLevel);
        if (_currentLevel <_maxLevel)
        {

            Debug.Log("EndMenu");
            EventManager.StopListening("EndMenu", OnEndMenu);
            infoCanvas.SetActive(false);
            endMenu.SetActive(true);
            Time.timeScale = 0f;
            
            EventManager.StartListening("EndMenu", OnEndMenu);

            EventManager.StartListening("SpawnStickman", OnSpawnStickman);

            int stars = _levelManager.getStars();
            Debug.Log("end menu stars" + stars);
            switch (stars)
            {
                case 0:
                    image.sprite = starsSprite0;
                    break;
                case 1:
                    image.sprite = starsSprite1;
                    break;
                case 2:
                    image.sprite = starsSprite2;
                    break;
                case 3:
                    image.sprite = starsSprite3;
                    break;
                default:
                    image.sprite = starsSprite0;
                    break;
            }
        }
        else
        {
            SceneManager.LoadScene("EndGame");
        }

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
        infoCanvas.SetActive(false);
        Time.timeScale = 0f;
    }

    private void OnSpawnStickman()
    {
        SpawnDrop();
        EventManager.StopListening("SpawnStickman", OnSpawnStickman);
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        infoCanvas.SetActive(true);
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
        _levelManager.setCurrentLevel(_currentLevel+1);
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
