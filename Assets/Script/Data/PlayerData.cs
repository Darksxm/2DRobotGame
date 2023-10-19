using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float movementVelocity = 10f;

    [Header("Jump State")]
    public float jumpVelocity = 15f;
    public int amountOfJumps = 1;

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
}