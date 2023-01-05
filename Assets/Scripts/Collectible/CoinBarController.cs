using UnityEngine;
using UnityEngine.UI;
using ParkourNews.Scripts;

public class CoinBarController : MonoBehaviour
{
    private Slider _slider;
    private LevelManager _levelManager;

    [SerializeField] private Image starsImage;
    [SerializeField] private Sprite noStars; 
    [SerializeField] private Sprite oneStars;
    [SerializeField] private Sprite twoStars;
    [SerializeField] private Sprite threeStars;
    
    void Start()
    {
        _slider = GetComponent<Slider>();
        _levelManager = FindObjectOfType<LevelManager>();
        _slider.value = 0;
        starsImage.sprite = noStars;
        _slider.maxValue = _levelManager.GetCoinsCurrentLevel();
        EventManager.StartListening("OnCoin", OnCoin);
        EventManager.StartListening("OnRespawn", OnRespawn);
    }

    void OnCoin()
    {
        EventManager.StopListening("OnCoin", OnCoin);
       
        _slider.value += 1;
        var newImage = noStars;
        if (_slider.value >= _slider.maxValue) newImage = threeStars;
        else if(_slider.value >= _slider.maxValue * 0.66) newImage = twoStars;
        else if(_slider.value >= _slider.maxValue * 0.33) newImage = oneStars;
        starsImage.sprite = newImage;
        
        EventManager.StartListening("OnCoin", OnCoin);
    }
    
    void OnRespawn()
    {
        EventManager.StopListening("OnRespawn", OnRespawn);
        _slider.value = 0;
        starsImage.sprite = noStars;
        EventManager.StartListening("OnRespawn", OnRespawn);
    }
}
