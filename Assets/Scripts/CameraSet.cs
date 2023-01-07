using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSet : MonoBehaviour
{
    private Spawner _spawner;
    private GameObject _mapCenter;
    private CinemachineVirtualCamera _cam;
    private StickmanActions _stickmanActions;

    [SerializeField] private float _zoomSpeed = 3f;
    [SerializeField] private float _zoomInMax = 11.36f;
    [SerializeField] private float _zoomOutMax = 30f;
    private float _currentFov = 30f;
    private float _incZoom = 1f;
    private bool _speedUpKey = false;
    [SerializeField] private float _percentageFollowing = 10f;
    [SerializeField] private float _deltaZoomIncrement = 0.01f;
    [SerializeField] private float _dumping = 3f;


    private void Start()
    {
        
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        _stickmanActions = new StickmanActions();
        _stickmanActions.Enable();
        _stickmanActions.Player.ZoomMap.started += OnZoomOutMap;
        _stickmanActions.Player.ZoomMap.canceled += OnZoomInMap;
        _cam = GetComponent<CinemachineVirtualCamera>();
        _cam.m_Lens.OrthographicSize = _zoomOutMax;
        _spawner = GameObject.FindObjectOfType<Spawner>();
        _mapCenter = GameObject.FindGameObjectWithTag("CenterLevel");
        _cam.Follow = _mapCenter.transform;
        _cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = _dumping;
        _cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = _dumping;
        _incZoom = 1f;
        _currentFov = 30f;
        //_cam.Follow = _spawner.transform;
        StartCoroutine(ZoomInCoroutine());
    }

    private void OnDisable()
    {
        _stickmanActions.Disable();
    }

    private void OnZoomOutMap(InputAction.CallbackContext context)
    {
        /*_cam.Follow = _mapCenter.transform;
        _cam.m_Lens.OrthographicSize = _zoomOutMax;
        _cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = _dumping;
        _cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = _dumping;
        _incZoom = 1f;
        _currentFov = 30f;*/
        EventManager.TriggerEvent("ZoomCamera");
        StartCoroutine(ZoomOutCoroutine());
        
    }
    
    private void OnZoomInMap(InputAction.CallbackContext context)
    {
        EventManager.TriggerEvent("ZoomCamera");
        StartCoroutine(ZoomInCoroutine());
    }

    IEnumerator ZoomInCoroutine()
    {
        while (_currentFov > _zoomInMax + 1)
        {
            _incZoom += _deltaZoomIncrement;
            ZoomScreen(-_incZoom);
            
            if (_currentFov < _zoomOutMax - ((_zoomOutMax - _zoomInMax) / 100) * _percentageFollowing)
            {
                _cam.Follow = _spawner.transform;
            }
            
            yield return new WaitForFixedUpdate();
        }
        
        _cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = 1f;
        _cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = 1f;
        EventManager.TriggerEvent("SpawnStickman");
    }
    
    IEnumerator ZoomOutCoroutine()
    {
        while (_currentFov < _zoomOutMax - 1)
        {
            _incZoom -= _deltaZoomIncrement;
            ZoomScreen(_incZoom);

            if (_currentFov > _zoomInMax + ((_zoomOutMax - _zoomInMax) / 100) * _percentageFollowing)
            {
                _cam.Follow = _mapCenter.transform;
            }

            yield return new WaitForFixedUpdate();
        }

        _cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = 1f;
        _cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = 1f;
    }
    
    public void ZoomScreen(float inc)
    {
        float fov = _cam.m_Lens.OrthographicSize;
        _currentFov = fov;
        float target = Mathf.Clamp(fov+inc, _zoomInMax, _zoomOutMax);
        _cam.m_Lens.OrthographicSize = Mathf.Lerp(fov, target, _zoomSpeed * Time.deltaTime);
    }
    
    // Update is called once per frame
    void Update()
    {

        if (Input.anyKey && !_speedUpKey)
        {
            Debug.Log("KeyPressedZoom");
            _deltaZoomIncrement = 0.5f;
            _speedUpKey = true;
        }

    }

    public void SetStickman(GameObject stickman)
    {
        //_stickman = stickman;
    }
}
