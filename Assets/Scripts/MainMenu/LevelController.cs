using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField] private string sceneName;
    
    public void PlayScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
