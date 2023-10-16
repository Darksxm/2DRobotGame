using UnityEngine;

public enum MovementState
{
    Regular, Crouching, sprint, Falling, climbing, Sliding, hanging
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


    [Header("Overall Infos")]
    public Animator anim;

    private Rigidbody2D rb;
    public BoxCollider2D GroundCheckCollider;
    public BoxCollider2D playerCollider2D;
    public BoxCollider2D crouchCollider2D;
    public Transform playerTransform;
    public Transform playerFeetTransform;

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
        SetBoolAnimatorStates();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _isGrounded = false;
        _isCrouching = false;
        SetBoolAnimatorStates();
    }

    public void SetBoolAnimatorStates()
    {
        anim.SetBool("isGrounded", _isGrounded);
        anim.SetBool("Falling", _isFalling);
        anim.SetBool("Crouching", _isCrouching);
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
            MovementParameter.movementState = MovementState.Regular;
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                MovementParameter.movementState = MovementState.Crouching;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                MovementParameter.movementState = MovementState.Sliding;
            }
        }
        if (!_isGrounded)
        {
            if (rb.velocity.y < 0)
            {
                MovementParameter.movementState = MovementState.Falling;
            }
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
            case MovementState.Regular:
                Move(inputHorizontal, movementMultiplier);
                Jump(jumpMultiplier);
                break;

            case MovementState.Crouching:
                Move(inputHorizontal, movementMultiplier / 4);
                Crouch();
                break;

            case MovementState.Falling:
                Falling();
                break;

            case MovementState.Sliding:
                Sliding();
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
        if (horizontalInput != 0)
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
        _isCrouching = _isGrounded && Input.GetKeyDown(KeyCode.LeftControl);        
        if (_isCrouching)
        {
            movementMultiplier = crouchSpeed;
            playerCollider2D.enabled = false;
            crouchCollider2D.enabled = true;
        }
        else
        {
            playerCollider2D.enabled = true;
            crouchCollider2D.enabled = false;
            _isCrouching = false;
        }      
        anim.SetBool("Crouching", _isCrouching);
    }

    private void Sliding()
    {
    }

    // have to fix the bool problem of when jumping and falling DOES NOT TURN OFF
    // crouching (only for 1s and goes out of animation because it is not registering a constant input of isgrounded = true)
}