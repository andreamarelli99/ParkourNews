using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraSet : MonoBehaviour
{
    private Spawner _spawner;
    private CinemachineVirtualCamera _cam;
    
    // Start is called before the first frame update
    void Start()
    {
        _cam = GetComponent<CinemachineVirtualCamera>();
        _spawner = GameObject.FindObjectOfType<Spawner>();
        _cam.Follow = _spawner.transform;
    }

    // Update is called once per frame
    void Update()
    {

        /*if (_spawner != null)
        {
            _cam.Follow = _spawner.transform; //_stickman.transform;
        }*/

    }

    public void SetStickman(GameObject stickman)
    {
        //_stickman = stickman;
    }
}
