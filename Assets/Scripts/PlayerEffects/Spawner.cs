using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _drop;
    [SerializeField] private GameObject _spawnEffect;
    [SerializeField] private GameObject _stickman;
    private Transform _transform;
    private bool _stickmanCreated = false;
    private Vector2 _initialPosition;

    [SerializeField] float _timeLeft;

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
        _initialPosition = _transform.position;
    }
    
    private void OnEnable()
    {
        EventManager.StartListening("OnDeath", OnDeath);
    }
    
    private void OnDeath()
    {
        _stickmanCreated = false;
        EventManager.StopListening("OnDeath",OnDeath);
        StartCoroutine(RespawnCoroutine());
        EventManager.StartListening("OnDeath",OnDeath);
    }
    
    
    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(1.6f);
        if (!_stickmanCreated)
        {
            SetPosition(_initialPosition);
            SpawnDrop();
        }
    }


    public void SetPosition(Vector2 stickmanTransform)
    {
        var spawnerTransform = transform;
        spawnerTransform.position = new Vector3(stickmanTransform.x, stickmanTransform.y, spawnerTransform.position.z);
        
    }

    public void SpawnDrop()
    {
    //    var dropTransform = transform;
        Instantiate(_drop, new Vector3(_transform.position.x, _transform.position.y+13, _transform.position.z), Quaternion.identity);
    }

    public void ExecuteSpawnEffect(Transform explosion)
    {
        _position = new Vector3((float)(0.13878 +_transform.position.x), (float)(0.62967 + _transform.position.y), _transform.position.z);
        Instantiate(_spawnEffect, _position , explosion.rotation);
        StartCoroutine(SpawnStickManCoroutine());
        //  AudioSource.PlayClipAtPoint(soundEffect, transform.position);
    }

    private IEnumerator SpawnStickManCoroutine()
    {
        yield return new WaitForSeconds(_timeLeft);
        Instantiate(_stickman, new Vector3(_transform.position.x, _transform.position.y, _transform.position.y), Quaternion.identity);
        _stickmanCreated = true;
    }

    
}
