using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Gravity")]
    public float gravityScale;
    public float gravityStrength;
    [Space(5)]
    public float fallGravityMult; //Multiplier to the player's gravityScale when falling.
    public float maxFallSpeed; //Maximum fall speed (terminal velocity) of the player when falling.
    [Space(5)]
    public float fastFallGravityMult; //Larger multiplier to the player's gravityScale when they are falling and a downwards input is pressed.                                      //Seen in games such as Celeste, lets the player fall extra fast if they wish.
    public float maxFastFallSpeed; //Maximum fall speed(terminal velocity) of the player when performing a faster fall.
    [Header("Move State")]
    public float movementVelocity = 10f;

    [Header("Jump State")]

    public float jumpVelocity = 15f;
    public int amountOfJumps = 1;
    public float jumpHeight; //Height of the player's jump
    public float jumpTimeToApex;
    [Space(0.5f)]
    public float jumpCutGravityMult; //Multiplier to increase gravity if the player releases thje jump button while still jumping
    [Range(0f, 1)] public float jumpHangGravityMult; //Reduces gravity while close to the apex (desired max height) of the jump
    public float jumpHangTimeThreshold; //Speeds (close to 0) where the player will experience extra "jump hang". The player's velocity.y is closest to 0 at the jump's apex (think of the gradient of a parabola or quadratic function)


    [Header("InAir State")]
    public float cayoteTime = 0.2f;
    public float variableJumpHeightMultiplier = 0.5f;

    [Header("WallSlide State")]
    public float wallSlideVelocity = 3f;

    [Header("WallClimb State")]
    public float wallClimbVelocity = 3f;

    [Header("WallJump State")]
    public float wallJumpVelocity = 20f;
    public float wallJumpTime = 0.4f;
    public float startWallJumpCayoteTime = 0.2f;
    public Vector2 wallJumpAngle = new Vector2(1, 2);

    [Header("LedgeClimb State")]
    public Vector2 startOffSet;
    public Vector2 stopOffSet;

    [Header("Dash State")]
    public float dashCoolDown = 0.5f;
    public float maxHoldDown = 1f;
    public float holdTimeScale = 0.25f; // how much do we sloat down time
    public float dashTime = 0.2f; // time we dash before leaving the state
    public float dashVelocity = 30f;
    public float drag = 10f; //linear drag of rb
    public float dashEndYMultiplier = 0.2f;
    public float distanceBetweenAfterImages = 0.5f;

    [Header("Check Variables")]
    public float groundCheckRadius = 0.3f;
    public float wallCheckDistance = 0.5f;
    public LayerMask whatIsGround;
    private void OnValidate()
    {
        //Calculate gravity strength using the formula (gravity = 2 * jumpHeight / timeToJumpApex^2) 
        gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);

        //Calculate the rigidbody's gravity scale (ie: gravity strength relative to unity's gravity value, see project settings/Physics2D)
        gravityScale = gravityStrength / Physics2D.gravity.y;

    }
}