using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    #region Gravity
    [Header("Gravity")]
    [Tooltip("Strength of the player's gravity as a multiplier of gravity (set in ProjectSettings/Physics2D).Also the value the player's rigidbody2D.gravityScale is set to.")]
    public float gravityScale;

    [Tooltip("Downwards force (gravity) needed for the desired jumpHeight and jumpTimeToApex.")]
    public float gravityStrength;

    [Tooltip("Multiplier to the player's gravityScale when falling.")]
    [Space(5)]
    public float fallGravityMult;

    [Tooltip("Maximum fall speed (terminal velocity) of the player when falling.")]
    public float maxFallSpeed;

    [Space(5)]
    [Tooltip("Larger multiplier to the player's gravityScale when they are falling and a downwards input is pressed.Seen in games such as Celeste, lets the player fall extra fast if they wish.")]
    public float fastFallGravityMult;

    [Tooltip("Maximum fall speed(terminal velocity) of the player when performing a faster fall.")]
    public float maxFastFallSpeed;
    #endregion
    #region Move State
    [Header("Move State")]
    public float movementVelocity = 10f;
    #endregion
    #region Jump State
    [Header("Jump State")]
    public float jumpVelocity = 15f;

    public int amountOfJumps = 1;
    [Tooltip("Height of the player's jump")]
    public float jumpHeight;
    [Tooltip("Time between applying the jump force and reaching the desired jump height.These values also control the player's gravity and jump force.")]
    public float jumpTimeToApex;

    [Space(0.5f)]
    [Tooltip("Multiplier to increase gravity if the player releases the jump button while still jumping")]
    public float jumpCutGravityMult;

    [Tooltip("Reduces gravity while close to the apex (desired max height) of the jump")]
    [Range(0f, 1)] public float jumpHangGravityMult;

    [Tooltip("Speeds (close to 0) where the player will experience extra jump hang. The player's velocity.y is closest to 0 at the jump's apex (think of the gradient of a parabola or quadratic function)")]
    public float jumpHangTimeThreshold;
    #endregion
    #region In Air State
    [Header("InAir State")]
    public float cayoteTime = 0.2f;

    public float variableJumpHeightMultiplier = 0.5f;
    #endregion
    #region Wall Slide State
    [Header("WallSlide State")]
    public float wallSlideVelocity = 3f;
    #endregion
    #region Wall Climb State
    [Header("WallClimb State")]
    public float wallClimbVelocity = 3f;
    #endregion
    #region Wall Jump State
    [Header("WallJump State")]
    public float wallJumpVelocity = 20f;
    public float wallJumpTimeToApex;
    public float wallJumpTime = 0.4f;
    public float startWallJumpCayoteTime = 0.2f;
    public Vector2 wallJumpAngle = new Vector2(1, 2);
    #endregion
    #region Ledge Climb State
    [Header("LedgeClimb State")]
    public Vector2 startOffSet;
    public Vector2 stopOffSet;
    #endregion
    #region Dash State
    [Header("Dash State")]
    public float dashCoolDown = 0.5f;
    public float maxHoldDown = 1f;
    [Tooltip("how much do we sloat down time")]
    public float holdTimeScale = 0.25f;
    public float dashTime = 0.2f; // time we dash before leaving the state
    public float dashVelocity = 30f;
    [Tooltip("linear drag of rb")]
    public float drag = 10f;
    public float dashEndYMultiplier = 0.2f;
    public float distanceBetweenAfterImages = 0.5f;
    #endregion
    #region Crouch State
    [Header("Crouch State")]
    public float crouchMoveVelocity = 5f;
    [Tooltip("Less than 1 so it fits under a 1 unit block")]
    public float crouchColliderHeight = 0.8f;
    [Tooltip("Current Player Collider Height")]
    public float standColliderHeight = 3.26f;
    #endregion
    #region Check Variables
    [Header("Check Variables")]
    public float groundCheckRadius = 0.3f;
    public float wallCheckDistance = 0.5f;
    public float ceilingCheckRadius = 0.5f;
    public LayerMask whatIsGround;
    #endregion
    private void OnValidate()
    {
        //Calculate gravity strength using the formula (gravity = 2 * jumpHeight / timeToJumpApex^2)
        gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);

        //Calculate the rigidbody's gravity scale (ie: gravity strength relative to unity's gravity value, see project settings/Physics2D)
        gravityScale = gravityStrength / Physics2D.gravity.y;
        //Calculate jumpHeigh using the formula (initialJumpVelocity = gravity * timeToJumpApex)
        jumpVelocity = Mathf.Abs(gravityStrength * jumpTimeToApex);
        wallJumpVelocity = Mathf.Abs(gravityStrength * wallJumpTimeToApex);
    }
}