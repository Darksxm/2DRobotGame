using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerWallClimbState WallClimbState { get; private set; }
    public PlayerWallGrabState WallGrabState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerLedgeClimbState LedgeClimbState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerCrouchIdleState CrouchIdleState { get; private set; }
    public PlayerCrouchMoveState CrouchMoveState { get; private set; }
    public PlayerAttackState PrimaryAttackState { get; private set; }
    public PlayerAttackState SecondaryAttackState { get; private set; }


    [SerializeField]
    private PlayerData playerData;
    #endregion
    #region Components
    public Animator Anim { get; private set; }
    public PlayerInputHandler inputHandler { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    public Transform DashDirectionIndicator { get; private set; }
    public BoxCollider2D MovementCollider { get; private set; }
    public PlayerInventory Inventory { get; private set; }
    #endregion
    #region Check Transforms
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private Transform WallCheck; 
    [SerializeField]
    private Transform ledgeCheck;   
    [SerializeField]
    private Transform ceilingCheck;
    #endregion
    #region Other Variables
    public Vector2 CurrentVelocity { get; private set; }  
    public int FacingDirection { get; private set; }    

    private Vector2 workSpace;
    #endregion
    #region Draw Gizmos Booleans
    private bool isTouchingCeiling;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingWallBack;
    #endregion
    #region Unity Callback Functions
    /// <summary>
    /// Set up all the different states and booleans in the animator
    /// </summary>
    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        WallClimbState = new PlayerWallClimbState(this, StateMachine, playerData, "wallClimb");
        WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "wallGrab");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "inAir");
        LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, "ledgeClimbState");
        DashState = new PlayerDashState(this, StateMachine, playerData, "dash");
        CrouchIdleState = new PlayerCrouchIdleState(this, StateMachine, playerData, "crouchIdle");
        CrouchMoveState = new PlayerCrouchMoveState(this, StateMachine, playerData, "crouchMove");
        PrimaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack");
        SecondaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack");

    }
    /// <summary>
    /// Gets all the component
    /// Tell us which direction we are facing when we start
    /// Initialize default state which is idle
    /// </summary>
    private void Start()
    {
        Anim = GetComponent<Animator>();
        inputHandler = GetComponent<PlayerInputHandler>();
        Rb = GetComponent<Rigidbody2D>();
        DashDirectionIndicator = transform.Find("DashDirectionIndicator");
        MovementCollider = GetComponent<BoxCollider2D>();
        Inventory = GetComponent<PlayerInventory>();

        FacingDirection = 1;

        PrimaryAttackState.SetWeapon(Inventory.weapons[(int)CombatInputs.primary]);
/*        SecondaryAttackState.SetWeapon(Inventory.weapons[(int)CombatInputs.primary]);
*/        StateMachine.Initialize(IdleState);
        SetGravityScale(playerData.gravityScale);
    }
    /// <summary>
    /// Update the current player velocity
    /// Update us on which state we are currently in
    /// </summary>
    private void Update()
    {
        CurrentVelocity = Rb.velocity;
        StateMachine.CurrentState.LogicUpdate();
       
    }
    /// <summary>
    /// Calls the DoCheck() function that is called in the PhysicsUpdate() within the PlayerState.cs through the PlayerStateMachine.cs(CurrentState)
    /// </summary>
    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion
    #region Set Functions
    public void SetVelocityZero()
    {
        Rb.velocity = Vector2.zero;
        CurrentVelocity = Vector2.zero;
    }
    /// <summary>
    /// set velocity at an angle
    /// </summary>
    /// <param name="velocity"></param>
    /// <param name="angle"></param>
    /// <param name="direction"></param>
    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        workSpace.Set(angle.x * velocity * direction, angle.y * velocity);
        Rb.velocity = workSpace;
        CurrentVelocity = workSpace;
    }
    /// <summary>
    /// function used in dashstate
    /// </summary>
    /// <param name="velocity"></param>
    /// <param name="direction"></param>
    public void SetVelocity(float velocity, Vector2 direction)
    {
        workSpace = direction * velocity;
        Rb.velocity = workSpace;
        CurrentVelocity = workSpace;
    }
    /// <summary>
    /// Set the player Speed on X axis
    /// </summary>
    /// <param name="velocity"></param>
    public void SetVelocityX(float velocity)
    {
        workSpace.Set(velocity, CurrentVelocity.y);
        Rb.velocity = workSpace;
        CurrentVelocity = workSpace;
    }
    /// <summary>
    /// Set the player Speed on Y axis
    /// </summary>
    /// <param name="velocity"></param>
    public void SetVelocityY(float velocity)
    {
        workSpace.Set(CurrentVelocity.x, velocity);
        Rb.velocity = workSpace;
        CurrentVelocity = workSpace;
    }
    public void SetGravityScale(float scale)
    {
        Rb.gravityScale = scale;
    }

    #endregion
    #region Check Functions
    public bool CheckForCeiling()
    {
        isTouchingCeiling= Physics2D.OverlapCircle(ceilingCheck.position, playerData.ceilingCheckRadius, playerData.whatIsGround);
        return isTouchingCeiling;
    }
    public bool CheckIfGrounded()
    {
        isGrounded= Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
        return isGrounded;
    }
    public bool CheckIfTouchingWall()
    {
        isTouchingWall= Physics2D.Raycast(WallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
        return isTouchingWall;
    }
    public bool CheckIfTouchingLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);        
    }
    public bool CheckIfTouchingWallBack()
    {
        isTouchingWallBack = Physics2D.Raycast(WallCheck.position, Vector2.right * -FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
        return isTouchingWallBack;
    }
    /// <summary>
    /// Checks if we need to flip the player according to the xInput which is set in PlayerInputHandler.cs
    /// It is Called in the PlayerMoveState.cs/LogicUpdate().
    /// /// </summary>
    /// <param name="xInput"></param>
    public void CheckIfShouldFlip(int xInput)
    {
        if(xInput !=0 && xInput != FacingDirection)
        {
            Flip();
        }
    }

    #endregion
    #region Gravity
    public void CheckGravity()
    {
        //Higher gravity if we've released the jump input or are falling
        if (WallSlideState.isSliding || LedgeClimbState.isHanging)
        {
            SetGravityScale(0);
        }
        else if (CurrentVelocity.y < 0 && inputHandler.NormalizedInputY < 0)
        {
            //Much higher gravity if holding down
            SetGravityScale(playerData.gravityScale * playerData.fastFallGravityMult);
            //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
            CurrentVelocity = new Vector2(CurrentVelocity.x, Mathf.Max(CurrentVelocity.y, -playerData.maxFastFallSpeed));
        }
        else if (InAirState.isJumping && CurrentVelocity.y > 0)
        {
            //Higher gravity if jump button released
            SetGravityScale(playerData.gravityScale * playerData.jumpCutGravityMult);
            CurrentVelocity = new Vector2(CurrentVelocity.x, Mathf.Max(CurrentVelocity.y, -playerData.maxFallSpeed));
        }
        else if ((InAirState.isJumping || WallJumpState.isWallJumping || CurrentVelocity.y < 0) && Mathf.Abs(CurrentVelocity.y) < playerData.jumpHangTimeThreshold)
        {
                SetGravityScale(playerData.gravityScale * playerData.jumpHangGravityMult);
        }
        else if (CurrentVelocity.y < 0)
        {
            //Higher gravity if falling
             SetGravityScale(playerData.gravityScale * playerData.fallGravityMult);
            //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
            CurrentVelocity = new Vector2(CurrentVelocity.x, Mathf.Max(CurrentVelocity.y, -playerData.maxFallSpeed));
        }
        else
        {
            //Default gravity if standing on a platform or moving upwards  or wall grab state
            SetGravityScale(playerData.gravityScale);
        }
    }
    #endregion
    #region Other Functions
    // to do : addd feature to manage offset on x axis
    public void SetColliderHeight(float height)
    {
        Vector2 center = MovementCollider.offset;
        workSpace.Set(MovementCollider.size.x, height);

        center.y += (height - MovementCollider.size.y) / 2;

        MovementCollider.size = workSpace;
        MovementCollider.offset = center;
    }
    /// <summary>
    /// Determine the distance between the player and the ledge
    /// </summary>
    /// <returns></returns>
    public Vector2 DetermineCornerPosition()
    {
        RaycastHit2D xHit = Physics2D.Raycast(WallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
        float xDistance = xHit.distance;
        workSpace.Set((xDistance + 0.015f) * FacingDirection, 0f);
        RaycastHit2D yHit = Physics2D.Raycast(ledgeCheck.position + (Vector3)(workSpace), Vector2.down, ledgeCheck.position.y - WallCheck.position.y + 0.015f, playerData.whatIsGround);
        float yDistance = yHit.distance;
        workSpace.Set(WallCheck.position.x + (xDistance * FacingDirection), ledgeCheck.position.y - yDistance);
        return workSpace;
    }

    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();
    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
    /// <summary>
    /// Flips our player
    /// </summary>
    private void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0f, 180f, 0f);
    }

    #endregion
    #region Unity Editor
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        
        Gizmos.color = isTouchingCeiling ? Color.red : Color.green;
        Gizmos.DrawWireSphere(ceilingCheck.position, playerData.ceilingCheckRadius);

        Gizmos.color = isGrounded ? Color.red : Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, playerData.groundCheckRadius);

        Gizmos.color = isTouchingWall ? Color.blue : Color.black;
        Gizmos.DrawRay(WallCheck.position, Vector2.right * FacingDirection * playerData.wallCheckDistance);

        Gizmos.color = isTouchingWallBack ? Color.blue : Color.black;
        Gizmos.DrawRay(WallCheck.position, Vector2.right * -FacingDirection * playerData.wallCheckDistance);




    }
#endif
    #endregion
}
