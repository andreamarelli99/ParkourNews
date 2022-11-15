using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLineController : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform _transform;
    [SerializeField] private float _speed = 5;
    [SerializeField] float _amplitude;
    [SerializeField] float _frequenz;
    float _x, _y;

    private void FixedUpdate()
    {
        _x = _x + _speed * Time.deltaTime;
        _y = Mathf.Sin(_x * _frequenz) * _amplitude;
        transform.position = new Vector2(_x, _y);
    }

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }
    void Start()
    {
        _x = _transform.position.x;
        _y = _transform.position.y;
    }
/*
    private void FixedUpdate()
    {
        _transform.position = _transform.position + _speed * Time.fixedDeltaTime * _transform.right;
    }*/
}
