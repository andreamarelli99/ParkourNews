using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using ParkourNews.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StickmanController : MonoBehaviour
{
    private Spawner _follower;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    public Animator _animator;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private int _facingDirection = 1;

    private Vector2 _initialPosition;

    //----------------------------SPEED-------------------------------//
    //put _Xspeed and _maxXspeed to regulate the stickman acceleration
    [SerializeField] private float _walkSpeed = 1000f;
    [SerializeField] private float _minSpeed = 0f;
    [SerializeField] private float _minRunSpeed = 2f;
    private float _realSpeed;

    //--------------------------MOVEMENTS-----------------------------//
    //to manage actions and/or inputs w/o specifying controllers/keyboard
    private Vector2 _movement = Vector2.zero;

    private StickmanActions _stickmanActions;

    //--Run

    private Boolean _isRunning;
    
    private Boolean _isDead;
    
    //--Crouch
    private Boolean _isCrouched; //check if the stickman is crouching
    [SerializeField] private float _slideForce = 9f;

    //--Somersault
    [SerializeField] private float _somersaultForce = 5f;

    private Boolean _doSommersault;

    //--Jump
    [SerializeField] private float _jumpForce = 12f;
    //to check if the player is jumping
    private Boolean _isJumping;
    //to check if the player is doubleJumping
    private Boolean _isDoubleJumping;
    
    //--Jump Wall
    [SerializeField] private float _wallHopForce = 8f;
    [SerializeField] private float _wallJumpForce = 5f;
    [SerializeField]private Vector2 _wallHopDirection;
    [SerializeField]private Vector2 _wallJumpDirection;
    

    private Boolean _isJumpWall;
    
    //check if it is sliding
    private Boolean _isSliding;

    //--Dash
    [SerializeField] private float _dashForce = 9f;
    private Boolean _canDash;
    
    //--Grappling
    private bool _isGrappling;
    
    //Death
    private Transform _transform;
    private bool _death = false;
    private bool _timerOn = false;
    [SerializeField] private float _timeLeft;
    [SerializeField] private GameObject _spawnEffect;

    private void Start()
    {
        //var cam = GameObject.FindObjectOfType<CameraSet>();
        //cam.SetStickman(gameObject);
        _wallHopDirection.Normalize();
        _wallJumpDirection.Normalize();
        _initialPosition = _transform.position;
    }

    private void Awake()
    {
        //find the rigid body
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _stickmanActions = new StickmanActions();
        _transform = GetComponent<Transform>();
        _follower = GameObject.FindObjectOfType<Spawner>();
    }

    private void OnEnable()
    {
        _stickmanActions.Enable();

        _stickmanActions.Player.Jump.performed += OnJump;
        _stickmanActions.Player.Dash.performed += OnDash;
        _stickmanActions.Player.Crouch.performed += OnCrouch;
        _stickmanActions.Player.Roll.performed += OnSomersault;
        _stickmanActions.Player.Menu.performed += OnMenu;
        
        _isCrouched = false;
        _doSommersault = false;
        _isJumping = false;
        _isDoubleJumping = false;
        _isSliding = false;
        _isRunning = false;
        _canDash = true;
        _isGrappling = false;
        
        EventManager.StartListening("OnBouncey",OnBouncey);
        //_animator.SetBool("IsDead",false);
        EventManager.StartListening("OnDeath",OnDeath);
        EventManager.StartListening("OnWall", OnWall);
        EventManager.StartListening("OnGround", OnGround);
        EventManager.StartListening("InAir", InAir);

    }

    

    private void OnDisable()
    {
        _stickmanActions.Disable();
    }

    [SerializeField] private float _xDecreasingWhenFalling =  1.015f;
    [SerializeField] private float _yIncreasingWhenFalling = 1.02f;
    [SerializeField] private float _xArtificialDragOnFloor = 1.015f;
    
    // Update is called once per frame
    void Update()
    {
        // ----- MOVEMENT LEFT/RIGHT ------
        // allows horizontal movement by pressing wasd/arrows
        _movement.x = Input.GetAxis("Horizontal");


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

        if (_isJumping && _rigidbody2D.velocity.y < 0)
        {
           _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x / _xDecreasingWhenFalling, _rigidbody2D.velocity.y); 
           
        }

        //Augmenting drag, just when is changig direction
        if (_movement.x * _rigidbody2D.velocity.x < 0 && !_isJumping)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x / _xArtificialDragOnFloor, _rigidbody2D.velocity.y);
        }
        
    }

    [SerializeField] private float _airDrag = 1f;
    [SerializeField] private float _addGravity = 1f;
    private void FixedUpdate()
    {
        if(!_isSliding){
            _realSpeed = Mathf.Max(_minSpeed, Mathf.Abs(_rigidbody2D.velocity.x));
            _isRunning=_realSpeed >= _minRunSpeed;
            _animator.SetFloat("Speed",Mathf.Abs(_realSpeed));
            _rigidbody2D.AddForce(_walkSpeed * Time.fixedDeltaTime * _movement);
        }
        if (_timerOn)
        {
            if (_timeLeft > 0)
            {
                _timeLeft -= Time.deltaTime;
                _spawnEffect.transform.position = _transform.position;
            }
            else
            {
                ExecuteSpawnEffect();
                DestroyImmediate(gameObject,true);//Destroy(gameObject);
                _timerOn = false;
            }
        }
        else
        {
            
            _follower.SetPosition(_transform);
            
        }

        if (_isJumping)
        {
            var velocity = transform.InverseTransformDirection(_rigidbody2D.velocity);
            float force_x = -_airDrag * velocity.x;
            _rigidbody2D.AddRelativeForce(new Vector2(force_x, 0));
            _rigidbody2D.AddForce(new Vector2(0f, -_addGravity), ForceMode2D.Force);
            
        }

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.gameObject.CompareTag("Trap") && !_death))
        {
            _rigidbody2D.velocity = new Vector2(0,_rigidbody2D.velocity.y);
            _death = true;
            Die();
        }
        
        if ((col.gameObject.CompareTag("RedLine")) && !_death)
        {
            _rigidbody2D.velocity = new Vector2(0,_rigidbody2D.velocity.y);
            _death = true;
            col.gameObject.SetActive(false);
            Die();
        }

        if (col.gameObject.CompareTag("Hook") && _isJumping)
        {

            _isJumping = false;
            _isGrappling = true;
            _animator.SetBool("IsJumping", true);
            _animator.SetBool("IsGrappling", true);
            _rigidbody2D.isKinematic = true;
            gameObject.transform.position =
                new Vector3(col.gameObject.GetComponent<Collider2D>().transform.position.x,
                    (col.gameObject.GetComponent<Collider2D>().transform.position.y -
                     col.gameObject.GetComponent<SpriteRenderer>().bounds.size.y),
                    0);
            _rigidbody2D.velocity = Vector2.zero;
        }
    }

    private void Die()
    {
        _animator.SetTrigger("IsDeath");
        _timerOn = true;
        EventManager.TriggerEvent("DeathSound");
    }
    
    private void ExecuteSpawnEffect()
    {
        Instantiate(_spawnEffect, _transform.position, _transform.rotation);
   //     EventManager.TriggerEvent("OnDeath");
        //  AudioSource.PlayClipAtPoint(soundEffect, transform.position);
    }

    private void OnMenu(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("MenuSelector");
    }


    //----------------------------------Stickman movements------------------------------------------------------------//
    private void OnJump(InputAction.CallbackContext context)
    {
        if (_isGrappling)
        {
            Debug.Log("Jump from hook!");
            _rigidbody2D.isKinematic = false;
            _animator.SetBool("IsGrappling",false);
            _isGrappling = false;
            _isJumping = true;
            _rigidbody2D.AddForce(new Vector2((m_FacingRight?1:-1)* _jumpForce/4,  _jumpForce/2 ), ForceMode2D.Impulse);
            //EventManager.StartListening("OnHook",OnHook);
        }

        else if (!_isJumping&&!_isCrouched)
        {
            _isJumpWall = false;
            _isJumping = true;
            Debug.Log("Jump!");
            _animator.SetBool("IsJumping",true);
            //_animator.SetBool("IsSlidingWall",false);
            _rigidbody2D.AddForce(new Vector2(0f, _jumpForce), ForceMode2D.Impulse);
            EventManager.StartListening("OnGround",OnGround);
            EventManager.StartListening("OnWall", OnWall);
            
            //Triggering sound for jump
            EventManager.TriggerEvent("JumpSound");
            
        }
        else if (!_isDoubleJumping&&!_isCrouched)
        {
            _isJumpWall = false;
            _isDoubleJumping = true;
            Debug.Log("Double Jump!");
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0f);
            //_animator.SetBool("IsSlidingWall",false);
            _rigidbody2D.AddForce(new Vector2(0f, _jumpForce), ForceMode2D.Impulse);
            EventManager.StartListening("OnGround",OnGround);
            EventManager.StartListening("OnWall", OnWall);
            
            //Triggering sound for DoubleJump
            EventManager.TriggerEvent("DoubleJumpSound");
        }
        else if (_isJumpWall)
        {
            _isJumpWall = false;
            Debug.Log("Jumping from wall");
            _animator.SetBool("IsJumping",true);

            Vector2 forceToAddHop = new Vector2(_wallHopForce * _wallHopDirection.x * -_facingDirection, 
                _wallHopForce * _wallHopDirection.y);
            _rigidbody2D.AddForce(forceToAddHop, ForceMode2D.Impulse);

            int direction = _movement.x > 0 ? 1 : -1;
            Vector2 forceToAddJump =
                new Vector2(_wallJumpDirection.x * _wallJumpForce * direction, _wallJumpDirection.y * _wallJumpForce);
            _rigidbody2D.AddForce(forceToAddJump, ForceMode2D.Impulse);
            
            //Triggering sound for JumpWall
            EventManager.TriggerEvent("JumpWallSound");
            
            EventManager.StartListening("OnWall", OnWall);
            EventManager.StartListening("OnGround", OnGround);
        }
        else 
            Debug.Log("No more than Double Jump!");
    }

    
    private void OnDash(InputAction.CallbackContext context)
    {
        if (_canDash&&!_isCrouched)
        {
            EventManager.TriggerEvent("DashSound");
            _rigidbody2D.AddForce((m_FacingRight ? 1 : -1) * Vector2.right * _dashForce, ForceMode2D.Impulse);
            _canDash = false;
            EventManager.TriggerEvent("OnDash");
        }
    }

    private void OnSomersault(InputAction.CallbackContext context)
    {
        //when you enter here do the roll animation
        _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
    }
    
    //todo real crouch
    private void OnCrouch(InputAction.CallbackContext context)
    {
        if (!_isCrouched) //if the stickman is not in a crouch position -> crouch
        {
            // if the player is running when the crouch is called slide and then crouch
            if (Math.Abs(_rigidbody2D.velocity.x) >= _minRunSpeed)
                StartCoroutine(WaitUntilWalkingSpeed());
            
            Debug.Log("Crouch!");
            _isCrouched = true;
            _animator.SetBool("IsCrouched", true);
        }
        else //if the stickman is in a crouch position -> getUp
        {
            //todo check for collisions
            Debug.Log("Get Up!");
            _isCrouched = false;
            _animator.SetBool("IsCrouched", false);
           
        }
    }

    /*private void OnHook()
    {
        EventManager.StopListening("OnHook",OnHook);
        _isJumping = false;
        _animator.SetBool("IsJumping",false);
        _animator.SetBool("IsGrappling",true);
        
        Debug.Log("Grappling Hook");
    }*/
    
    private void OnWall()
    {
        EventManager.StopListening("OnWall",OnWall);
        _isDoubleJumping = true;
        _isJumping = true;
        _isJumpWall = true;
        _animator.SetBool("IsJumping",false);
        _animator.SetBool("IsSlidingWall",true);
        Debug.Log("Attached to wall");
    }
    
    private void Flip()
    {
        if(!_isSliding){
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;
        _facingDirection *= -1;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        }
    }

    //---- called upon events----//
    private void OnGround()
    {
        EventManager.StopListening("OnGround",OnGround);
        EventManager.StartListening("InAir",InAir);
        _isJumping = false;
        _isDoubleJumping = false;
        _animator.SetBool("IsJumping",false);
        _animator.SetBool("IsSlidingWall", false);
        Debug.Log("Ended Jump!");
    }
    
    private void InAir()
    {
        EventManager.StopListening("InAir",InAir);
        EventManager.StartListening("OnGround",OnGround);
        EventManager.StartListening("OnWall", OnWall);
        _isJumpWall = false;
        _isJumping = true; 
        _animator.SetBool("IsJumping",true);
        //_animator.SetBool("IsSlidingWall",false);
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
        //_animator.SetBool("IsDead",true);
        _rigidbody2D.position = _initialPosition;
        Debug.Log("You Died!");
        EventManager.TriggerEvent("OnPlayerDeath");
        EventManager.StartListening("OnDeath",OnDeath);
    }
    
    public void CanDash()
    {
        Debug.Log("Now you can dash!");
        _canDash = true;
    }

    IEnumerator WaitUntilWalkingSpeed()
    {
        _animator.SetBool("IsSliding", true);
        _isSliding = true;
        _rigidbody2D.AddForce((m_FacingRight ? 1 : -1) * _slideForce* Vector2.right, ForceMode2D.Impulse);
        
        //wait until the stickman speed has decreased enough to "crouch walking"
        while (Math.Abs(_rigidbody2D.velocity.x) >= _minRunSpeed)
        {
            yield return null;
        }
        
        _animator.SetBool("IsSliding", false);
        _isSliding = false;
    }
}

