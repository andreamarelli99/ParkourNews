using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _drop;
    [SerializeField] private GameObject _spawnEffect;
    [SerializeField] private GameObject _stickman;
    private Transform _transform;
    private bool _stickmanCreated = false;

    [SerializeField] float _timeLeft;
    private bool _timerOn = false;

    private Vector3 _position;

    // public AudioClip soundEffect;

    
    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
    //    ExecuteSpawnEffect();
        SpawnDrop();
    }

    // Update is called once per frame
    void Update()
    {
        if (_timerOn)
        {
            if (_timeLeft > 0)
            {
                _timeLeft -= Time.deltaTime;
            }
            else
            {
                SpawnStickMan();
            }
        }
    }

    public void SetPosition(Transform stickmanTransform)
    {
        var spawnerTransform = transform;
        spawnerTransform.position = new Vector3(stickmanTransform.position.x, stickmanTransform.position.y, spawnerTransform.position.z);
        
    }
    
    public Transform GetPosition()
    {
        return _transform;
    }

    public void SpawnDrop()
    {
    //    var dropTransform = transform;
    Instantiate(_drop, new Vector3(_transform.position.x, _transform.position.y+13*3, _transform.position.y), Quaternion.identity);
    }

    public void ExecuteSpawnEffect(Transform explosion)
    {
        _position = new Vector3((float)(0.13878 +_transform.position.x), (float)(0.62967 + _transform.position.y), _transform.position.z);
        Instantiate(_spawnEffect, _position , explosion.rotation);
        _timerOn = true;
        //  AudioSource.PlayClipAtPoint(soundEffect, transform.position);
    }
    
    public void SpawnStickMan()
    {
        Instantiate(_stickman, new Vector3(_transform.position.x, _transform.position.y, _transform.position.y), Quaternion.identity);
      //  _spawnEffect.SetActive(true);
        _timerOn = false;
        _stickmanCreated = true;

    }

    
}
