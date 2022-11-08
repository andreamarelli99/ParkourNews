using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Cinemachine;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _drop;
    [SerializeField] private GameObject _spawnEffect;
    [SerializeField] private GameObject _stickman;
    [SerializeField] private Transform _cameraTransform;

    [SerializeField] float _timeLeft;
    private bool _timerOn = false;

    private Vector3 _position;

    // public AudioClip soundEffect;

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

    public void SpawnDrop()
    {
        var dropTransform = transform;
        var targetPosition = _cameraTransform.position;
        dropTransform.position = new Vector3(targetPosition.x, targetPosition.y+13*3, dropTransform.position.z);
        Instantiate(_drop, dropTransform.position, dropTransform.rotation);
    }

    public void ExecuteSpawnEffect(Transform explosion)
    {
        _position = new Vector3(explosion.position.x, (float)(0.1 + explosion.position.y), explosion.position.z);
        Instantiate(_spawnEffect, _position , explosion.rotation);
        _timerOn = true;
        //  AudioSource.PlayClipAtPoint(soundEffect, transform.position);
    }
    
    public void SpawnStickMan()
    {
        Instantiate(_stickman, new Vector3((float)(_position.x), (float)( _position.y), _position.z), Quaternion.identity);
        Destroy(_spawnEffect);
        _timerOn = false;
     //   Destroy(gameObject);

    }

    
}
