using UnityEngine;

public enum MovementState
{
    run, crouch, sprint, falling, climbing
}

public static class MovementParameter
{
    public static float movementConstant = 5f;
    public static float jumpingPowerConstant = 5f;

    public static MovementState movementState;
}

public class BasicMovement : MonoBehaviour
{
    [Header("Public Value")]
    public float jumpMultiplier;

    public float movementMultiplier;

    [Header("Private Info")]
    private float inputHorizontal;

    [SerializeField]
    private float currentMovementSpeed;

    [SerializeField]
    private float currentJumpPower;

    [SerializeField]
    private float crouchSpeed;
    [SerializeField]
    private float jumpMovementSpeed;

    private float currentHeight;
    private float maxJumpHeight;

    [Header("Overall Infos")]
    /// <summary>
    /// Other Operators
    /// </summary>
    [SerializeField]
    private LayerMask _groundLayer;

    private Rigidbody2D rb;
    public Transform playerTransform;
    public Transform playerFeetTransform;

    public Collider2D _footCollider;
    public Animator anim;

    private bool _isGrounded;
    private bool _isJumping;
    private bool _isCrouching;
    private bool _isFalling;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = transform;
    }

    private void Update()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        MovementHandler();
        Debug.Log(MovementParameter.movementState);
    }

    /*  private void OnTriggerEnter2D(Collider2D collision)
      {
          _isFalling = false;
      }*/

    /// <summary>
    /// Check if player is grounded
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _isFalling = false;
            _isGrounded = true;
            Debug.Log("Character on the floor");
        }
        SetBoolStates();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _isGrounded = false;
        SetBoolStates();
    }

    public void SetBoolStates()
    {
        anim.SetBool("isGrounded", _isGrounded);
        anim.SetBool("Falling", _isFalling);
       
    }

    /// <summary>
    /// Makes the changes in movement state automatic according to the situations
    /// the first line calculate the body orientation based on the inputHorizontal so that the body
    /// faces the sens it is going to
    /// </summary>
    private void MovementHandler()
    {
        if (_isGrounded)
        {
            MovementParameter.movementState = MovementState.run;
        }
        if (_isGrounded && Input.GetKey(KeyCode.LeftControl))
        {
            MovementParameter.movementState = MovementState.crouch;
        }
        if (!_isGrounded && rb.velocity.y < 0)
        {
            MovementParameter.movementState = MovementState.falling;
        }
        SwitchMovementMechanism();
    }

    /// <summary>
    /// Switch movement mechanism
    /// still work in progress
    /// </summary>
    private void SwitchMovementMechanism()
    {
        switch (MovementParameter.movementState)
        {
            case MovementState.run:
                Move(inputHorizontal, movementMultiplier);
                Jump(jumpMultiplier);
                break;

            case MovementState.crouch:
                Move(inputHorizontal, movementMultiplier / 4);
                Crouch();
                break;

            case MovementState.falling:
                Falling();
                break;

            case MovementState.climbing: break;
        }
    }

    /// <summary>
    /// Initialize the Running movement
    /// </summary>
    /// <param name="horizontalInput"></param>
    private void Move(float horizontalInput, float _movementMultiplier)
    {
        currentMovementSpeed = MovementParameter.movementConstant * _movementMultiplier;
        Vector2 moveDirection = new Vector2(horizontalInput, 0f);
        rb.velocity = new Vector2(moveDirection.x * currentMovementSpeed, rb.velocity.y);
        // Rotate the character based on input
        if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            // Calculate the angle based on the horizontal input
            float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            // Set Z-axis rotation to zero to prevent rotation around the Y-axis
            playerTransform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
        }
        if (horizontalInput > 0 || horizontalInput < 0)
        {
            anim.SetBool("Running", true);
        }
        else
        {
            anim.SetBool("Running", false);
        }
    }

    /// <summary>
    /// Initializing the jumping mechanism
    /// The jumping hight is influenced by the amount of time the space bar is clicked
    /// </summary>
    private void Jump(float _jumpMultiplier)
    {
        currentJumpPower = MovementParameter.jumpingPowerConstant * _jumpMultiplier;
        _isJumping = Input.GetButtonDown("Jump") && _isGrounded;
        if (_isJumping)
        {
            _isJumping = !_isJumping;
            movementMultiplier = jumpMovementSpeed;
            rb.velocity = new Vector2(movementMultiplier, currentJumpPower);
            anim.SetTrigger("Jumping");
        }
    }

    /// <summary>
    /// Trigger the Falling animation
    /// </summary>
    private void Falling()
    {
        _isFalling = true;
        if (_isFalling)
        {
            movementMultiplier = jumpMovementSpeed;
        }
        anim.SetBool("Falling", _isFalling);
    }

    private void Crouch()
    {
        _isCrouching = _isGrounded && Input.GetKey(KeyCode.LeftControl);


        if (_isCrouching)
        {
            movementMultiplier = crouchSpeed;
        }
        if(Input.GetKeyUp(KeyCode.LeftControl)||!_isGrounded)
        {
            _isCrouching = false;
        }
        anim.SetBool("Crouching", _isCrouching);
    }

    // have to fix the bool problem of when jumping and falling DOES NOT TURN OFF
    // crouching (only for 1s and goes out of animation because it is not registering a constant input of isgrounded = true)
}