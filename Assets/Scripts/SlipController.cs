using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipController : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    [SerializeField] private float slipForce = 10f;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        // check if a slippery ground is touched by the stickman
        if (col.gameObject.CompareTag("Slip"))
        {
            Vector3 dir = Quaternion.Euler(0,0, col.transform.rotation.z) * Vector3.down;
            dir = dir.normalized;
            _rigidbody2D.AddForce(slipForce * dir);
        }
    }
}
