using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class DropExplosion : MonoBehaviour
{
    private Spawner _spawner;
    private Transform _transform;
    
    // Start is called before the first frame update

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _spawner = Object.FindObjectOfType<Spawner>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        _spawner.ExecuteSpawnEffect(_transform);
        Destroy(gameObject);
    }
}
