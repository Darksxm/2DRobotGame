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
    /// <summary>
    /// Public Fields Value
    /// </summary>

    /// <summary>
    /// Private Fields Value
    /// </summary>
    private float inputHorizontal;
    private float movementMultiplier;
    private float currentMovementSpeed;
    private float jumpMultiplier;
    private float currentJumpPower;
    private float currentHeight;
    private float maxJumpHeight;

    /// <summary>
    /// Other Operators
    /// </summary>
    [SerializeField]
    private LayerMask _groundLayer;
    private bool _isGrounded;
    private Rigidbody2D rb;
    public Transform playerTransform;

    public Collider2D _footCollider;
    public Animator anim;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = transform;
    }

    private void Update()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        OnTriggerEnter2D(_footCollider);
        MovementHandler();
        SwitchMovementMechanism();
        Debug.Log(MovementParameter.movementState);
    }

   

    /// <summary>
    /// Check if player is grounded
    /// </summary>
    /// <param name="collision"></param>

    private void OnTriggerEnter2D(Collider2D collision)
    {
        anim.SetBool("isGrounded",_isGrounded);
        if (collision.IsTouchingLayers(_groundLayer))
        {
            _isGrounded = true;
            Debug.Log("Character on the floor");
        }
        else
        {
            _isGrounded = false;
        }
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
        if (!_isGrounded && currentHeight >= maxJumpHeight)
        {
            MovementParameter.movementState = MovementState.falling;
        }
        else if (!_isGrounded)
        {
            MovementParameter.movementState = MovementState.falling;
        }
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
                movementMultiplier = 1f;
                jumpMultiplier = 3f;
                Move(inputHorizontal);
                Jump();
                break;

            case MovementState.crouch:
                movementMultiplier = 0.5f;
                jumpMultiplier = 0f;
                Move(inputHorizontal);
                anim.SetBool("Crouching", true);
                break;

            case MovementState.climbing: break;
            case MovementState.falling:
                movementMultiplier = 0f;
                jumpMultiplier = 0f;
                Falling();
                break;
        }
    }

    /// <summary>
    /// Initialize the Running movement
    /// </summary>
    /// <param name="horizontalInput"></param>
    private void Move(float horizontalInput)
    {
        currentMovementSpeed = MovementParameter.movementConstant * movementMultiplier;
        Vector2 moveDirection = new Vector2(horizontalInput, 0f);
        rb.velocity = new Vector2(moveDirection.x * currentMovementSpeed, rb.velocity.y);
        // Rotate the character based on input
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
    private void Jump()
    {
        currentJumpPower = MovementParameter.jumpingPowerConstant * jumpMultiplier;
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, currentJumpPower);

            if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
            CalculateMaxHeight();
           
            anim.SetTrigger("Jumping");
        }
    }

    private void Falling()
    {
        anim.SetTrigger("Falling");
    }


    /// <summary>
    /// Calculate the maximum jumping height the player can jump at according to the current rb gravity.
    /// </summary>
    private void CalculateMaxHeight()
    {
        float g = Mathf.Abs(Physics2D.gravity.y) * rb.gravityScale; // Assuming gravity is acting downward
        maxJumpHeight = (currentJumpPower * currentJumpPower) / (2 * g);
        currentHeight = Mathf.Max(currentHeight, transform.position.y);
    }

}