using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RedLineController : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform _transform;
    private Rigidbody2D _rb;
    private Vector2 _movement;
    private GameObject _spawnerGameObject;
    Transform _spawner;
    [SerializeField] private float _speed = 2;
    private void FixedUpdate()
    {
        moveLine(_movement);
    }

    private void Update()
    {
        Vector3 direction = _spawner.position - _transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _rb.rotation = angle;
        direction.Normalize();
        _movement = direction;
    }

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody2D>();
        _spawnerGameObject = GameObject.FindGameObjectWithTag("Spawner");
    }

    private void OnEnable()
    {
        EventManager.StartListening("OnDeath",OnDeath);
        EventManager.StartListening("ReachedEndRedLineCheckPoint",OnDeath);
    }

    private void Start()
    {
        _spawner = _spawnerGameObject.transform;
    }

    private void moveLine(Vector2 direction)
    {
        _rb.MovePosition((Vector2)_transform.position + (direction*_speed*Time.deltaTime));
    }
    
    
    private void OnDeath()
    {
        EventManager.StopListening("OnDeath",OnDeath);
        Destroy(gameObject);
        EventManager.StartListening("OnDeath",OnDeath);
    }

}
