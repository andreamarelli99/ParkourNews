using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{

    private CanvasGroup _canvas;
    
    // Start is called before the first frame update
    void Start()
    {
        _canvas = gameObject.GetComponent<CanvasGroup>();
        _canvas.alpha = 0;
        EventManager.StartListening("SpawnStickman", OnSpawnStickman);
        EventManager.StartListening("ZoomCamera", OnZoomCamera);
    }

    private void OnZoomCamera()
    {
        if (_canvas.alpha == 0f)
        {
            _canvas.alpha = 1f;
        }
        else
        {
            _canvas.alpha = 0f;
        }
    }
    private void OnSpawnStickman()
    {
        _canvas.alpha = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
