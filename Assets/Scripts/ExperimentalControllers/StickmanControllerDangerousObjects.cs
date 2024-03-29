using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Cinemachine;
using ParkourNews.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class StickmanControllerDangerousObjects : MonoBehaviour
{
    private Spawner _follower;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    public Animator _animator;
    // private bool m_FacingRight = true;
    private int _facingDirection = 1; // For determining which way the player is currently facing.

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
    
    private bool _canMove;

    //--Run

    private Boolean _isRunning;
    
    private Boolean _isDead;
    
    //--Flip
    private Boolean _justFlipped = false;
    
    //--Crouch
    private Boolean _isCrouched; //check if the stickman is crouching
    [SerializeField] private float _slideForce = 9f;

    //--Roll
    private bool _canRoll;

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
    [SerializeField] private float _incrementForceY = 1.07f;
    
    //-- Booleans for listening events
    private Boolean _onGroundEvent = false;
    private Boolean _onWallEvent = false;
    private Boolean _inAirEvent = false;
    private bool _isSlidingOblique;

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
    [SerializeField] private GameObject _deathEffect;
    
    // Slide oblique
    [SerializeField] private float slidingObliqueForce = 10f;
    private float _slidingObliqueColliderRadius = 0.5f;
    private Vector2 _slideObliqueDir;
    private bool _isSlidingObliqueRight;

    private void Start()
    {
        //var cam = GameObject.FindObjectOfType<CameraSet>();
        //cam.SetStickman(gameObject);
        Application.targetFrameRate = 30;
        QualitySettings.vSyncCount = 1;
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
        _spriteRenderer = _rigidbody2D.GetComponent<SpriteRenderer>();
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
        _isJumping = false;
        _isDoubleJumping = false;
        _isSliding = false;
        _isRunning = false;
        _canDash = true;
        _isGrappling = false;
        _canRoll = false;
        _canMove = true;
        
        EventManager.StartListening("OnBouncey",OnBouncey);
        //_animator.SetBool("IsDead",false);
        EventManager.StartListening("OnWall", OnWall);
        _onWallEvent = true;
        EventManager.StartListening("OnWallButCantJump", OnWallButCantJump);
        EventManager.StartListening("OnGround", OnGround);
        _onGroundEvent = true;
        EventManager.StartListening("InAir", InAir);
        EventManager.StartListening("OnSlidingObliqueExit", OnSlidingObliqueExit);
        EventManager.StartListening("OnSlidingObliqueLeftEnter", OnSlidingObliqueLeftEnter);
        EventManager.StartListening("OnSlidingObliqueRightEnter", OnSlidingObliqueRightEnter);
        _inAirEvent = true;
        
        //Triggering SpawnSound
        EventManager.TriggerEvent("SpawnSound");
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
        if (_canMove)
        {
            _movement.x = Input.GetAxis("Horizontal");
        }

        // If the input is moving the player right and the player is facing left...
        if (_movement.x > 0 && _facingDirection == -1)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (_movement.x < 0 && _facingDirection == 1)
        {
            // ... flip the player.
            Flip();
        }

        if (_isJumping && _rigidbody2D.velocity.y < 0)
        {
           _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x / _xDecreasingWhenFalling, _rigidbody2D.velocity.y*_yIncreasingWhenFalling); 
           
        }

        //Augmenting drag, just when is changig direction
        if (_movement.x * _rigidbody2D.velocity.x < 0 && !_isJumping)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x / _xArtificialDragOnFloor, _rigidbody2D.velocity.y);
        }
        
        if (_isSlidingOblique &&  _rigidbody2D.velocity.magnitude < slidingObliqueForce)
        {
            if ((_isSlidingObliqueRight && _facingDirection == -1) || (!_isSlidingObliqueRight && _facingDirection == 1))
            {
                Flip();
            }
            
            _rigidbody2D.velocity = _slideObliqueDir * slidingObliqueForce;
            // Here we should find a way to apply a force for a natural acceleration
            // _rigidbody2D.AddForce(_slideObliqueDir * slidingObliqueForce);
        }
    }

    private void OnSlidingObliqueLeftEnter()
    {
        _isSlidingObliqueRight = false;
        OnSlidingObliqueEnter();
    }

    private void OnSlidingObliqueRightEnter()
    {
        _isSlidingObliqueRight = true;
        OnSlidingObliqueEnter();
    }

    private void OnSlidingObliqueEnter()
    {
        _canMove = false;
        _isSlidingOblique = true;
        _isJumping = false;
        _isDoubleJumping = false;
        _isCrouched = false;
        _isSliding = false;

        var size = _spriteRenderer.bounds.size;
        var position = _spriteRenderer.transform.position;

        var footX = position.x - size.x / 2;
        var footY = position.y - size.y / 2; 
        
        //Debug.DrawLine(new Vector3(0,0,0),_slideObliqueDir, Color.cyan, 1000);
        
        _slideObliqueDir = new Vector2(footX, footY).normalized;
        _animator.SetBool("IsSlidingOblique", true);
    }

    private void OnSlidingObliqueExit()
    {
        _canMove = true;
        _isSlidingOblique = false;
        _animator.SetBool("IsSlidingOblique", false);
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
        
        _follower.SetPosition(_transform.position);
        
        if (_isJumping &&!_death)
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
        StartCoroutine(ExecuteDeathEffectCoroutine());
    }
    
    private IEnumerator ExecuteDeathEffectCoroutine()
    {
        yield return new WaitForSeconds(_timeLeft);
        Instantiate(_deathEffect, _transform.position, _transform.rotation);
        EventManager.TriggerEvent("OnDeath");
        DestroyImmediate(gameObject,true);
    }
    
    private void OnMenu(InputAction.CallbackContext context)
    {
        EventManager.TriggerEvent("OnMenu");
    }


    //----------------------------------Stickman movements------------------------------------------------------------//
    private void OnJump(InputAction.CallbackContext context)
    {
        //EventManager.StopListening("InAir",InAir);
        _canRoll = true;
        _canMove = true;
        if (_isGrappling)
        {
            Debug.Log("Jump from hook!");
            _rigidbody2D.isKinematic = false;
            _animator.SetBool("IsGrappling",false);
            _isGrappling = false;
            _isJumping = true;
            _rigidbody2D.AddForce(new Vector2(_facingDirection * _jumpForce/4,  _jumpForce/2 ), ForceMode2D.Impulse);
            //EventManager.StartListening("OnHook",OnHook);
        }
        else if (_isJumpWall || _justFlipped)
        {
            _isJumpWall = false;
            Debug.Log("Jumping from wall");
            _animator.SetBool("IsJumping",true);
            
            int oppositeDirection = 1;
            float incForce = 1;
            Debug.Log("Jumping with justFlipped: " + _justFlipped);
            if (_justFlipped)
            {
                oppositeDirection = -1;
                incForce = _incrementForceY;
            }

            Vector2 forceToAddHop = new Vector2(_wallHopForce * _wallHopDirection.x * -_facingDirection * oppositeDirection, 
                _wallHopForce * _wallHopDirection.y * incForce);
            _rigidbody2D.AddForce(forceToAddHop, ForceMode2D.Impulse);

            

            Vector2 forceToAddJump =
                new Vector2(_wallJumpDirection.x * _wallJumpForce * _facingDirection * oppositeDirection, _wallJumpDirection.y * _wallJumpForce * incForce);
            _rigidbody2D.AddForce(forceToAddJump, ForceMode2D.Impulse);
            
            //Triggering sound for JumpWall
            //           EventManager.TriggerEvent("JumpWallSound");

            if (!_onWallEvent)
            {
                EventManager.StartListening("OnWall", OnWall);
                _onWallEvent = true;
            }
            if (!_onGroundEvent)
            {
                EventManager.StartListening("OnGround", OnGround);
                _onGroundEvent = true;
            }
        }
        else if (!_isJumping&&!_isCrouched)
        {
            _isJumpWall = false;
            _isJumping = true;
            Debug.Log("Jump!");
            _animator.SetBool("IsJumping",true);
            //_animator.SetBool("IsSlidingWall",false);
            // A normal jump is directed up
            var dir = Vector2.up;
       /*     if (_isSlidingOblique)
            {
                // In sliding oblique the player follows the direction of the slided floor.
                // The jump direction is the resultant force of the tangent and normal vector relative to the sliding oblique.
                dir = _slideObliqueDir + Vector2.Perpendicular(_slideObliqueDir);
        /}*/
            _rigidbody2D.AddForce(_jumpForce * dir, ForceMode2D.Impulse);
            if (!_onWallEvent)
            {
                EventManager.StartListening("OnWall", OnWall);
                _onWallEvent = true;
            }
            if (!_onGroundEvent)
            {
                EventManager.StartListening("OnGround", OnGround);
                _onGroundEvent = true;
            }

            //Triggering sound for jump
            EventManager.TriggerEvent("JumpSound");
            
        }
        else if (!_isDoubleJumping && !_isCrouched)
        {
            _isJumpWall = false;
            _isDoubleJumping = true;
            Debug.Log("Double Jump!");
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0f);
            //_animator.SetBool("IsSlidingWall",false);
            _rigidbody2D.AddForce(new Vector2(0f, _jumpForce), ForceMode2D.Impulse);
            if (!_onWallEvent)
            {
                EventManager.StartListening("OnWall", OnWall);
                _onWallEvent = true;
            }
            if (!_onGroundEvent)
            {
                EventManager.StartListening("OnGround", OnGround);
                _onGroundEvent = true;
            }
            
            //Triggering sound for DoubleJump
            EventManager.TriggerEvent("DoubleJumpSound");
        }
        else 
            Debug.Log("No more than Double Jump!");
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (_canDash&&!_isCrouched)
        {
            EventManager.TriggerEvent("DashSound");
            _rigidbody2D.AddForce(_facingDirection * Vector2.right * _dashForce, ForceMode2D.Impulse);
            _canDash = false;
            _animator.SetBool("IsSliding", false);
            _animator.SetBool("IsCrouched", false);
            EventManager.TriggerEvent("OnDash");
        }
    }

    private void OnSomersault(InputAction.CallbackContext context)
    {
        //when you enter here do the roll animation
        if (_canRoll)
        {
            Debug.Log("Somersault");
            _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
            _animator.SetTrigger("IsRolling");
            _canRoll = false;
        }
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
            if(!_isJumping)
            {
                _animator.SetBool("IsCrouched", true);
            }
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
        if (_onWallEvent)
        {
            EventManager.StopListening("OnWall",OnWall);
            _onWallEvent = false;
        }

        if (!_inAirEvent)
        {
            EventManager.StartListening("InAir", InAir);
            _inAirEvent = true;
        }
       
    //    EventManager.StartListening("OffWall",OffWall);
        _isDoubleJumping = true;
        _isJumping = false;
        _isJumpWall = true;
        _animator.SetBool("IsJumping",false);
        _animator.SetBool("IsSlidingWall",true);
        Debug.Log("Attached to wall");
    }
    
    private void OnWallButCantJump()
    {
        if (_onWallEvent)
        {
            EventManager.StopListening("OnWall",OnWall);
            _onWallEvent = false;
        }

        if (!_inAirEvent)
        {
            EventManager.StartListening("InAir", InAir);
            _inAirEvent = true;
        }
       
        _isDoubleJumping = true; // if flase after reaching the same wall the player can perform another NORMAL jump (like double jumping)
        _isJumping = true;
        _isJumpWall = false;
        _animator.SetBool("IsJumping",false);
        _animator.SetBool("IsSlidingWall",true);
        Debug.Log("Attached to wall");
    }
    
    private void Flip()
    {
        if(!_isSliding || !_isJumpWall || !_isSlidingOblique) {
            Debug.Log("flip!");
            
            if (_isJumpWall)
            {
                Debug.Log("Setting justFlipped true");
                _justFlipped = true;
                StartCoroutine(DisableJustFlippedCoroutine());
            }

            // Switch the way the player is labelled as facing.
            // m_FacingRight = !m_FacingRight;
            _facingDirection *= -1;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    IEnumerator DisableJustFlippedCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        _justFlipped = false;
        Debug.Log("Setting justFlipped false");
    }

    //---- called upon events----//
    private void OnGround()
    {
        if (_onGroundEvent)
        {
            EventManager.StopListening("OnGround",OnGround);
            _onGroundEvent = false;
        }

        if (_onWallEvent)
        {
            EventManager.StopListening("OnWall",OnWall);
            _onWallEvent = false;
        }

        if (!_inAirEvent)
        {
            EventManager.StartListening("InAir",InAir);
            _inAirEvent = true;
        }
        
        _isJumping = false;
        _isDoubleJumping = false;
        _isCrouched = false;
        _isJumpWall = false;
        _animator.SetBool("IsJumping",false);
        _animator.SetBool("IsSlidingWall", false);
        _animator.SetBool("IsCrouched", false);

        Debug.Log("Ended Jump!");
    }
    
    
    private void InAir()
    {
        _isJumpWall = false;
        _isJumping = true;

        if (_inAirEvent)
        {
            EventManager.StopListening("InAir",InAir);
            _inAirEvent = false;
        }

        if (!_onGroundEvent)
        {
            EventManager.StartListening("OnGround",OnGround);
            _onGroundEvent = true;
        }

        if (!_onWallEvent)
        {
            EventManager.StartListening("OnWall", OnWall);
            EventManager.StartListening("OnWallButCantJump",OnWallButCantJump);
            _onWallEvent = true;
        }
        
        _animator.SetBool("IsJumping",true);
        Debug.Log("inair");
        _animator.SetBool("IsSlidingWall",false);
    }

    private void OnBouncey()
    {
        
        EventManager.StopListening("OnBouncey",OnBouncey);
        _isDoubleJumping = true;
        // to allow player to jump after a bouncy collision
        _isJumping = false; 
        EventManager.StartListening("OnBouncey",OnBouncey);
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
        _rigidbody2D.AddForce(_facingDirection * _slideForce* Vector2.right, ForceMode2D.Impulse);
        
        //wait until the stickman speed has decreased enough to "crouch walking"
        while (Math.Abs(_rigidbody2D.velocity.x) >= _minRunSpeed)
        {
            yield return null;
        }
        
        _animator.SetBool("IsSliding", false);
        _isSliding = false;
    }
}

