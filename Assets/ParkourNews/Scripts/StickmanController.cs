using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StickmanController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;

    //----------------------------SPEED-------------------------------//
    //put _Xspeed and _maxXspeed to regulate the stickman acceleration
    [SerializeField] private float _walkSpeed = 1000f;
    [SerializeField] private float _maxWalkSpeed = 1000f;

    //--------------------------MOVEMENTS-----------------------------//
    //to manage input - actions w/o specifying controllers/keyboard
    private Vector2 _movement = Vector2.zero;

    private StickmanActions _stickmanActions;

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

    //--Dash
    [SerializeField] private float _dashForce = 8f;

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


    }

    private void OnDisable()
    {
        _stickmanActions.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        // allows horizontal movement by pressing wasd/arrows
        _movement.x = Input.GetAxis("Horizontal");
        // if (Input.GetKey(KeyCode.Q))
        //     transform.Rotate(_somersaultForce*  Time.deltaTime *-Vector3.forward );
        //  else if (Input.GetKey(KeyCode.E))
        //    transform.Rotate(_somersaultForce*  Time.deltaTime *Vector3.forward );

    }

    private void FixedUpdate()
    {
        _rigidbody2D.drag = _walkSpeed / _maxWalkSpeed;
        _rigidbody2D.AddForce(_walkSpeed * Time.fixedDeltaTime * _movement);
    }




    // Stickman movements
    private void OnJump(InputAction.CallbackContext context)
    {
        if (!_isJumping)
        {
            _isJumping = true;
            Debug.Log("Jump!");
            _rigidbody2D.AddForce(new Vector2(0f, _jumpForce), ForceMode2D.Impulse);
            EventManager.StartListening("OnGround",OnGround);
        }
        else if (!_isDoubleJumping)
        {
            _isDoubleJumping = true;
            Debug.Log("Double Jump!");
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
        _rigidbody2D.AddForce(gameObject.transform.forward * _somersaultForce, ForceMode2D.Impulse);
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        Debug.Log("Dash!");
        _rigidbody2D.AddForce(Vector2.right * _dashForce, ForceMode2D.Impulse);
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

    private void OnGround()
    {
        EventManager.StopListening("OnGround",OnGround);
        _isJumping = false;
        _isDoubleJumping = false;
        Debug.Log("Ended Jump!");
    }
}

