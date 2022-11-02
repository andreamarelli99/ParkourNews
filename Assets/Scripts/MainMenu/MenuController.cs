using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject playMenu;

    public void Start()
    {
        mainMenu.SetActive(true);
        playMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}