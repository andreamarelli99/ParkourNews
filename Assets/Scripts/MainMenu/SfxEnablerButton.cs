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
        _dataManager = FindObjectOfType<DataManager>();
        _enabled = _dataManager.GetSfxEnabled();
        ChangeImage();
    }

    private void OnClick()
    {
        _enabled = !_enabled;
        _dataManager.SetMusicEnabled(_enabled);
        ChangeImage();
    }

    private void ChangeImage()
    {
        _image.sprite = _enabled ? onSprite : offSprite;
    }
}