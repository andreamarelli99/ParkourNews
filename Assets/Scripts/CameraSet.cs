using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraSet : MonoBehaviour
{
    private Spawner _spawner;
    private GameObject _mapCenter;
    private CinemachineVirtualCamera _cam;

    [SerializeField] private float _zoomSpeed = 3f;
    [SerializeField] private float _zoomInMax = 11.36f;
    [SerializeField] private float _zoomOutMax = 30f;
    private float _currentFov = 30f;
    
    // Start is called before the first frame update
    void Start()
    {
        _cam = GetComponent<CinemachineVirtualCamera>();
        _cam.m_Lens.OrthographicSize = _zoomOutMax;
        _spawner = GameObject.FindObjectOfType<Spawner>();
        _mapCenter = GameObject.FindGameObjectWithTag("CenterLevel");
        _cam.Follow = _mapCenter.transform;
        _cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = 8f;
        _cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = 8f;
        //_cam.Follow = _spawner.transform;
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

        if (_currentFov > _zoomInMax+0.5)
        {
            ZoomScreen(-1);
        }
        else
        {
            _cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = 1f;
            _cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = 1f;
        }

        if (_currentFov < (_zoomOutMax - _zoomInMax) / 2 + _zoomInMax)
        {
            _cam.Follow = _spawner.transform;
        }

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
