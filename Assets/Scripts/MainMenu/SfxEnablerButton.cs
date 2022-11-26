using ParkourNews.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class SfxEnablerButton : MonoBehaviour
{
    [SerializeField] public Sprite offSprite;
    [SerializeField] public Sprite onSprite;
    private Button _button;

    private DataManager _dataManager;
    private bool _enabled;
    private Image _image;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
        _image = GetComponent<Image>();
        ChangeImage();
        _dataManager = FindObjectOfType<DataManager>();
        _enabled = _dataManager.GetMusicEnabled();
    }

    private void OnClick()
    {
        ChangeImage();
        _enabled = !_enabled;
        _dataManager.SetMusicEnabled(_enabled);
    }

    private void ChangeImage()
    {
        _image.sprite = _enabled ? onSprite : offSprite;
    }
}