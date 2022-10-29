using System;
using System.Collections;
using System.Collections.Generic;
using ParkourNews.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StickmanController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    public Animator _animator;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
   
    
    //----------------------------SPEED-------------------------------//
    //put _Xspeed and _maxXspeed to regulate the stickman acceleration
    [SerializeField] private float _walkSpeed = 1000f;
    [SerializeField] private float _minSpeed = 500f;
    [SerializeField] private float _minRunSpeed = 750;
    private float _realSpeed;

    //--------------------------MOVEMENTS-----------------------------//
    //to manage actions and/or inputs w/o specifying controllers/keyboard
    private Vector2 _movement = Vector2.zero;

    private StickmanActions _stickmanActions;

    //--Run

    private Boolean _isRunning;
    
    //--Crouch
    private Boolean _isCrouched; //check if the stickman is crouching

    //--Somersault
    [SerializeField] private float _somersaultForce = 5f;

    private Boolean _doSommersault;

    //--Jump
    [SerializeField] private float _jumpForce = 12f;
    //to check if the player is jumping
    private Boolean _isJumping;
    //to check if the player is doubleJumping
    private Boolean _isDoubleJumping;
    
    //check if it is sliding
    private Boolean _isSliding;

    //--Dash
    [SerializeField] private float _dashForce = 9f;

    private void Awake()
    {
        //find the rigid body
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _stickmanActions = new StickmanActions();
    }

    private void OnEnable()
    {
        _stickmanActions.Enable();

        _stickmanActions.Player.Jump.performed += OnJump;
        _stickmanActions.Player.Dash.performed += OnDash;
        _stickmanActions.Player.Crouch.performed += OnCrouch;
        _stickmanActions.Player.Somersault.performed += OnSomersault;

        _isCrouched = false;
        _doSommersault = false;
        _isJumping = false;
        _isDoubleJumping = false;
        _isSliding = false;
        _isRunning = false;
        
        EventManager.StartListening("OnBouncey",OnBouncey);
        EventManager.StartListening("OnDeath",OnDeath);

    }

    

    private void OnDisable()
    {
        _stickmanActions.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        // ----- MOVEMENT LEFT/RIGHT ------
        // allows horizontal movement by pressing wasd/arrows
        _movement.x = Input.GetAxis("Horizontal");
        _animator.SetFloat("Speed",Mathf.Abs(_movement.x));
        
        // If the input is moving the player right and the player is facing left...
        if (_movement.x > 0 && !m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (_movement.x < 0 && m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
        
        
        // if (Input.GetKey(KeyCode.Q))
        //     transform.Rotate(_somersaultForce*  Time.deltaTime *-Vector3.forward );
        //  else if (Input.GetKey(KeyCode.E))
        //    transform.Rotate(_somersaultForce*  Time.deltaTime *Vector3.forward );

    }

    private void FixedUpdate()
    {
        _realSpeed = Mathf.Max(_minSpeed, _rigidbody2D.velocity.y);
        _isRunning=_realSpeed >= _minRunSpeed;
        _rigidbody2D.AddForce(_walkSpeed * Time.fixedDeltaTime * _movement);
    }




    // Stickman movements
    private void OnJump(InputAction.CallbackContext context)
    {
        if (!_isJumping)
        {
            _isJumping = true;
            Debug.Log("Jump!");
            _animator.SetBool("IsJumping",true);
            _animator.SetBool("IsFlying",true);
            _rigidbody2D.AddForce(new Vector2(0f, _jumpForce), ForceMode2D.Impulse);
            EventManager.StartListening("OnGround",OnGround);
            
        }
        else if (!_isDoubleJumping)
        {
            _isDoubleJumping = true;
            Debug.Log("Double Jump!");
            _animator.SetBool("IsJumping",false);
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0f);
            _rigidbody2D.AddForce(new Vector2(0f, _jumpForce), ForceMode2D.Impulse);
            EventManager.StartListening("OnGround",OnGround);
        }
        else 
            Debug.Log("No more than Double Jump!");


    }

    //todo add collisions to block the action?
    private void OnSomersault(InputAction.CallbackContext context)
    {
        Debug.Log("Somersault!");
        //todo direction as general
        if (_isSliding)
        {
            _isSliding = !_isSliding;
            _animator.SetBool("IsSliding", false);
        }
        else
        {
            _isSliding = !_isSliding;
            _animator.SetBool("IsSliding", true);
        }
        _rigidbody2D.AddForce(gameObject.transform.forward * _somersaultForce, ForceMode2D.Impulse);
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        Debug.Log("Dash!");
        _rigidbody2D.AddForce((m_FacingRight?1:-1) * Vector2.right * _dashForce, ForceMode2D.Impulse);
    }

    //todo real crouch
    private void OnCrouch(InputAction.CallbackContext context)
    {
        if (!_isCrouched) //if the stickman is not in a crouch position -> crouch
        {
            Debug.Log("Crouch!");
            _isCrouched = true;
            transform.Rotate(Vector3.forward * 90);
        }
        else //if the stickman is in a crouch position -> getUp
        {
            //todo check for collisions
            Debug.Log("Get Up!");
            _isCrouched = false;
            transform.Rotate(Vector3.forward * (-90));
        }

    }
    
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    //---- called upon events----//
    private void OnGround()
    {
        EventManager.StopListening("OnGround",OnGround);
        _isJumping = false;
        _isDoubleJumping = false;
        _animator.SetBool("IsFlying",false);
        Debug.Log("Ended Jump!");
    }

    private void OnBouncey()
    {
        EventManager.StopListening("OnBouncey",OnBouncey);
        _isDoubleJumping = true;
        // to allow player to jump after a bouncy collision
        _isJumping = false; 
        EventManager.StartListening("OnBouncey",OnBouncey);
    }

    private void OnDeath()
    {
        EventManager.StopListening("OnDeath",OnDeath);
        Debug.Log("You Died!");
        EventManager.TriggerEvent("OnPlayerDeath");
        EventManager.StartListening("OnDeath",OnDeath);
    }
}

