using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb2D;
    private Animator _animator;

    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _jumpForce = 60f;
    private bool _isJumping;
    private bool _facingRight;
    private float _moveHorizontal;
    private float _moveVertical;

    // Start is called before the first frame update
    void Start()
    {
        _rb2D = gameObject.GetComponent<Rigidbody2D>();
        _animator = gameObject.GetComponent<Animator>();
        _isJumping = false;
        _facingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        _moveHorizontal = Input.GetAxisRaw("Horizontal");
        _moveVertical = Input.GetAxisRaw("Vertical");
        //if(_moveHorizontal>0) 
        _animator.SetFloat("Speed", Mathf.Abs(_moveHorizontal));
    }

    void FixedUpdate()
    {
        if (_moveHorizontal > 0.1f || _moveHorizontal < -0.1f)
        {
            _rb2D.AddForce(new Vector2(_moveHorizontal * _moveSpeed, 0f), ForceMode2D.Impulse);
        }

        if (!_isJumping && _moveVertical > 0.1f)
        {
            _rb2D.AddForce(new Vector2(0f, _moveVertical * _jumpForce), ForceMode2D.Impulse);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Platform")
        {
            _isJumping = false;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            _isJumping = true;
        }
    }
}
