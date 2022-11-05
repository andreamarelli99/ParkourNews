using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraSet : MonoBehaviour
{
    private GameObject _stickman;
    private CinemachineVirtualCamera _cam;
    
    // Start is called before the first frame update
    void Start()
    {
        _cam = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {

        if (_stickman != null)
        {
            _cam.Follow = _stickman.transform;
        }

    }

    public void SetStickman(GameObject stickman)
    {
        _stickman = stickman;
    }
}
