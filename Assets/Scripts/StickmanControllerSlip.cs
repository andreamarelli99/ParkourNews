using UnityEngine;

public class StickmanControllerSlip : MonoBehaviour
{
    [SerializeField] private float slipForce = 10f;
    private bool _isSlipping;

    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _slipDir;
    private Transform _transform;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // check if a slippery ground is touched by the stickman
        if (_isSlipping) _rigidbody2D.AddForce(slipForce * _slipDir);
        // _transform.Rotate(0, 180, 0);
    }

    
    
}