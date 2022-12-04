using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

/* Controller of the stickman/player, here there're:                     
 - values of speed for the various player movements
 - player movements
 - death of the stickman
 */


public class StickmanController : MonoBehaviour
{
    
    [SerializeField] private Rigidbody2D _rigidbody2D;
    private Spawner _follower;
    private SpriteRenderer _spriteRenderer;
    public Animator _animator;
    
    #region SPEED_VALUES
    //------------------------------------- SPEED VALUES -------------------------------------//
    [SerializeField] private float _walkSpeed = 1000f;
    
    [SerializeField] private float _minSpeed = 0f;
    
    // When the stickman speed >= _minRunSpeed -> RUN
    [SerializeField] private float _minRunSpeed = 2f;
    
    private float _realSpeed;
    #endregion
    
    #region MOVEMENTS
    //------------------------------------- MOVEMENTS -------------------------------------//
    // For determining which way the stickman is currently facing
    private int _facingDirection = 1; 
    
    // To manage actions and/or inputs without specifying controllers/keyboard
    private StickmanActions _stickmanActions;
    
    // Set to zero
    private Vector2 _movement = Vector2.zero;
    
    // Check if the stickman can move
    private bool _canMove;

    //----- Run -----//
    // To check if the stickman is running
    private Boolean _isRunning;
    
    //----- Flip -----//
    // To check if the stickman has flipped its direction
    private Boolean _justFlipped = false;
    // To check if the stickman can flip
    private bool _canFlip;
    
    //----- Crouch -----//
    // To check if the stickman is crouching
    private Boolean _isCrouched; 
    // Force applied when the stickman slides
    [SerializeField] private float _slideForce = 9f;
    
    //----- Roll -----//
    // To check if the stickman can roll
    private bool _canRoll;
    
    //----- Jump -----//
    // To check on On Ground Event 
    private Boolean _onGroundEvent = false;
    // To check on In Air Event 
    private Boolean _inAirEvent = false;
    // Force applied when the stickman jumps
    [SerializeField] private float _jumpForce = 12f;
    // To check if the player is jumping
    private Boolean _isJumping;
    // To check if the player is doubleJumping
    private Boolean _isDoubleJumping;
    
    //-----Jump: Jump Wall -----//
    // To check of the player is jumping on the wall
    private Boolean _isJumpWall;
    // To check on On Wall Event 
    private Boolean _onWallEvent = false;
    // Force applied when the stickman is wall jumping
    [SerializeField] private float _wallHopForce = 8f;
    // Force applied by the wall to the stickman when wall jumping
    [SerializeField] private float _wallJumpForce = 5f;
    // Positive Force on y applied when the stickman is wall jumping
    [SerializeField] private float _incrementForceY = 1.07f;
    // Wall Jump directions
    [SerializeField]private Vector2 _wallHopDirection;
    [SerializeField]private Vector2 _wallJumpDirection;
    
    //----- Slide -----//
    // To check if the stickman is sliding
    private Boolean _isSliding;
    // The minimum amount of time between two slides 
    [SerializeField] private float SlideRate = 2f; 
    // The minimum time when the stickman will be able to slide again
    private float NextSlide = 0f;
    // To check if the stickman can slide
    private Boolean _canSlide = true;

    //----- Dash -----//
    // Force applied when the stickman is dashing
    [SerializeField] private float _dashForce = 9f;
    // To check if the stickman can dash
    private Boolean _canDash;
    
    //----- Grappling -----//
    // To check if the player is grappling 
    private bool _isGrappling;
    
    //----- Slide oblique -----//
    // Force applied when the stickman is sliding on an oblique object
    [SerializeField] private float slidingObliqueForce = 10f;
    // To check if the stickman is sliding on an oblique object
    private bool _isSlidingOblique;
    #endregion

    #region DEATH
    //------------------------------------- DEATH -------------------------------------//
    // To check if the stickman is dead
    private Boolean _isDead;
    private Transform _transform;
    private bool _death = false;
    private bool _timerOn = false;
    [SerializeField] private float _timeLeft;
    [SerializeField] private GameObject _deathEffect;
    #endregion
    
    private void Start()
    {
        Application.targetFrameRate = 30;
        QualitySettings.vSyncCount = 1;
        _wallHopDirection.Normalize();
        _wallJumpDirection.Normalize();
        
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
        // Enable stickman actions
        _stickmanActions.Enable();
        _stickmanActions.Player.Jump.performed += OnJump;
        _stickmanActions.Player.Dash.performed += OnDash;
        _stickmanActions.Player.Crouch.performed += OnCrouch;
        _stickmanActions.Player.Roll.performed += OnSomersault;
        _stickmanActions.Player.Menu.performed += OnMenu;
        
        // Default values for checks on stickman's actions
        _isCrouched = false;
        _isJumping = false;
        _isDoubleJumping = false;
        _isSliding = false;
        _isRunning = false;
        _canDash = true;
        _isGrappling = false;
        _canRoll = false;
        _canMove = true;
        _canFlip = true;
        _canSlide = true;
        
        
        // Start listening on Events
        EventManager.StartListening("OnBouncey",OnBouncey);
        EventManager.StartListening("OnWall", OnWall);
        _onWallEvent = true;
        EventManager.StartListening("OnWallButCantJump", OnWallButCantJump);
        EventManager.StartListening("OnGround", OnGround);
        _onGroundEvent = true;
        EventManager.StartListening("InAir", InAir);
        _inAirEvent = true;
        EventManager.StartListening("PreDeath", Die);
        EventManager.StartListening("CanDash", CanDash);
        EventManager.StartListening("OnSlidingObliqueExit", OnSlidingObliqueExit);
        EventManager.StartListening("OnSlidingObliqueLeftEnter", OnSlidingObliqueLeftEnter);
        EventManager.StartListening("OnSlidingObliqueRightEnter", OnSlidingObliqueRightEnter);
        EventManager.TriggerEvent("SpawnSound");
    }

    private void OnDisable()
    {
        _stickmanActions.Disable();
    }

    [SerializeField] private float _xDecreasingWhenFalling =  1.015f;
    [SerializeField] private float _yIncreasingWhenFalling = 1.02f;
    [SerializeField] private float _xArtificialDragOnFloor = 1.015f;
    
    //auto rotate
    public Vector2 _botRight,_botLeft;
    public LayerMask _whatIsGround =  1 << 8;
    
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

        //Augmenting drag, just when is changing direction
        if (_movement.x * _rigidbody2D.velocity.x < 0 && !_isJumping)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x / _xArtificialDragOnFloor, _rigidbody2D.velocity.y);
        }
        
        if (_isSlidingOblique &&  _rigidbody2D.velocity.magnitude < slidingObliqueForce)
        {
            // Apply a force bottom right or bottom left based on the direction of the sliding oblique.
            // In this way, the player is pushed by gravity against the floor and in the direction of it.
            _rigidbody2D.AddForce((Vector2.down + Vector2.right * _facingDirection) * slidingObliqueForce);
        }
    }

    private void OnMenu(InputAction.CallbackContext context)
    {
        EventManager.TriggerEvent("OpenMenu");
    }

    private void OnSlidingObliqueLeftEnter()
    {
        OnSlidingObliqueEnter(false);
    }

    private void OnSlidingObliqueRightEnter()
    {
        OnSlidingObliqueEnter(true);
    }

    private void OnSlidingObliqueEnter(bool isRight)
    {
        Debug.Log("sliding oblique in");
        
        _isSlidingOblique = true;
        _animator.SetBool(IsSlidingOblique, true);

        // Push the player against the floor.
        _rigidbody2D.AddForce(Vector2.down * slidingObliqueForce);
        
        // Avoid double jump.
        _isJumping = true;
        
        // Set other player states to false.
        _canMove = false;
        _isDoubleJumping = false;
        _isCrouched = false;
        _isSliding = false;
        _justFlipped = false;

        // Flip the player when it is looking to the opposite side of the sliding platform.
        if ((isRight && _facingDirection == -1) || (!isRight && _facingDirection == 1))
        {
            Flip();
        }
        
        // Now block the player flip until it exits the sliding oblique.
        _canFlip = false;
    }

    private void OnSlidingObliqueExit()
    {
        Debug.Log("sliding oblique out");
        _isSlidingOblique = false;
        StartCoroutine("DisableSlidingObliqueCoroutine");
    }
    
    IEnumerator DisableSlidingObliqueCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        // Check if meantime the player has not fallen again on the sliding oblique.
        if (!_isSlidingOblique)
        {
            Debug.Log("disable sliding oblique");
            _canMove = true;
            _canFlip = true;
            _animator.SetBool(IsSlidingOblique, false);
        }
    }

    [SerializeField] private float _airDrag = 1f;
    [SerializeField] private float _addGravity = 1f;
    
    // Cached Properties
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int IsSlidingOblique = Animator.StringToHash("IsSlidingOblique");
    private static readonly int IsSlidingWall = Animator.StringToHash("IsSlidingWall");
    private static readonly int IsCrouched = Animator.StringToHash("IsCrouched");
    private static readonly int IsSliding = Animator.StringToHash("IsSliding");
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int IsGrappling = Animator.StringToHash("IsGrappling");
    private static readonly int IsDeath = Animator.StringToHash("IsDeath");

    private void FixedUpdate()
    {
        if(!_isSliding){
            _realSpeed = Mathf.Max(_minSpeed, Mathf.Abs(_rigidbody2D.velocity.x));
            _isRunning=_realSpeed >= _minRunSpeed;
            _animator.SetFloat(Speed,Mathf.Abs(_realSpeed));
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
        if ((col.gameObject.CompareTag("Trap") || col.gameObject.CompareTag("RedLine"))&& !_death)
        {
            _rigidbody2D.velocity = new Vector2(0,_rigidbody2D.velocity.y);
            _death = true;
            Die();
        }
        else if (col.gameObject.CompareTag("Hook") && _isJumping)
        {
            EventManager.TriggerEvent("WallJumpMessage");
            Debug.Log("Attached to Hook");
            _isJumping = false;
            _isGrappling = true;
            _canRoll = false;
            _animator.SetBool(IsJumping, true);
            _animator.SetBool(IsGrappling, true);
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
        EventManager.StopListening("PreDeath", Die);
        _animator.SetTrigger(IsDeath);
        StartCoroutine(ExecuteDeathEffectCoroutine());
    }
    
    private IEnumerator ExecuteDeathEffectCoroutine()
    {
        yield return new WaitForSeconds(_timeLeft);
        Instantiate(_deathEffect, _transform.position, _transform.rotation);
        EventManager.TriggerEvent("OnDeath");
        DestroyImmediate(gameObject,true);
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
            _animator.SetBool(IsGrappling,false);
            _isGrappling = false;
            _isJumping = true;
            _isDoubleJumping = false;
            _canRoll = true;
            _rigidbody2D.AddForce(new Vector2(_facingDirection * _jumpForce,  0 ), ForceMode2D.Impulse);
            _rigidbody2D.AddForce(new Vector2(0,  _jumpForce/2 ), ForceMode2D.Impulse);
            //EventManager.StartListening("OnHook",OnHook);
        }
        else if (_isJumpWall || _justFlipped)
        {
            _isJumpWall = false;
            Debug.Log("Jumping from wall");
            _animator.SetBool(IsJumping,true);
            
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
        else if (!_isJumping)
        {

            if (_isSliding || _isCrouched) _walkSpeed *= 2;
            _isJumpWall = false;
            _isJumping = true;
            _isCrouched = false;
            _isSliding = false;
            _isSlidingOblique = false;
            Debug.Log("Jump!");
            _animator.SetBool(IsJumping,true);
            _animator.SetBool(IsCrouched,false);
            _animator.SetBool(IsSliding,false);
            _animator.SetBool(IsSlidingOblique,false);
            //_animator.SetBool("IsSlidingWall",false);
            _rigidbody2D.AddForce(_jumpForce * Vector2.up, ForceMode2D.Impulse);
            
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
            // A normal jump is directed up
            var dir = Vector2.up;
            if (_isSlidingOblique)
            {
                // Apply a 45' force in same direction of the sliding oblique.
                dir = Vector2.up + Vector2.right * _facingDirection;
            }
            _rigidbody2D.AddForce(dir * _jumpForce, ForceMode2D.Impulse);
            
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
        if (_canDash&&!_isCrouched && !_isSlidingOblique)
        {
            EventManager.TriggerEvent("DashSound");
            _rigidbody2D.AddForce(_facingDirection * Vector2.right * _dashForce, ForceMode2D.Impulse);
            _canDash = false;
            _isSliding = false;
            _isCrouched = false;
            _animator.SetBool(IsSliding, false);
            _animator.SetBool(IsCrouched, false);
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
        if (!_isCrouched && !_isJumping && !_isSliding && !_isSlidingOblique) //if the stickman is not in a crouch position -> crouch
        {
                // if the player is running when the crouch is called slide and then crouch
                if (Math.Abs(_rigidbody2D.velocity.x) >= _minRunSpeed && _isRunning && Time.time > NextSlide)
                {
                    StartCoroutine(WaitUntilWalkingSpeed());
                    NextSlide = Time.time + SlideRate;
                    Debug.Log("Can slide");
                }

                Debug.Log("Crouch!");
                _isCrouched = true;
                _animator.SetBool("IsCrouched", true);
                _walkSpeed /= 2;

        }
        else if(!_isJumping && !_isSliding && !_isSlidingOblique)//if the stickman is in a crouch position -> getUp
        {
            //todo check for collisions
            Debug.Log("Get Up!");
            _isCrouched = false;
            _animator.SetBool("IsCrouched", false);
            _walkSpeed *= 2;

        }
    }

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
        _animator.SetBool(IsJumping,false);
        _animator.SetBool(IsSlidingWall,true);
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
        _animator.SetBool(IsJumping,false);
        _animator.SetBool(IsSlidingWall,true);
        Debug.Log("Attached to wall");
    }
    
    private void Flip()
    {
        if (!_canFlip)
            return;
        
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
        _isSlidingOblique = false;
        _animator.SetBool(IsJumping,false);
        _animator.SetBool(IsSlidingWall, false);
        _animator.SetBool(IsCrouched, false);
        _animator.SetBool(IsSlidingOblique,false);
        _canFlip = true;
        _canMove = true;

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
        
        _animator.SetBool(IsJumping,true);
        Debug.Log("InAir");
        _animator.SetBool(IsSlidingWall,false);
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
        EventManager.StopListening("CanDash",CanDash);
        Debug.Log("Now you can dash!");
        _canDash = true;
        EventManager.StartListening("CanDash",CanDash);
    }

    public void CanSlide()
    {
        EventManager.StopListening("CanSlide",CanSlide);
        Debug.Log("Now you can slide!");
        _canSlide = true;
        EventManager.StartListening("CanSlide",CanSlide);
    }
    
    IEnumerator WaitUntilWalkingSpeed()
    {
        if (!_isSliding)
        {
            _animator.SetBool(IsSliding, true);
            _isSliding = true;
            _rigidbody2D.AddForce(_facingDirection * _slideForce* Vector2.right, ForceMode2D.Impulse);
        }
        
        //wait until the stickman speed has decreased enough to "crouch walking"
        while (Math.Abs(_rigidbody2D.velocity.x) >= _minRunSpeed)
        {
            yield return null;
        }
        
        _animator.SetBool(IsSliding, false);
        _isSliding = false;
    }
}

