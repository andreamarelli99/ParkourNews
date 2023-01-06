using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LettersManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening("OnRespawn", OnRespawn);
        EventManager.StartListening("CheckPointReached", OnRespawn);
        
        //gameObject.SetActive(true);
    }

    private void OnRespawn()
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}
