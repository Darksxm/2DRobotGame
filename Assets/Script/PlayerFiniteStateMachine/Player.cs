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


    [SerializeField]
    private PlayerData playerData;
    #endregion
    #region Components
    public Animator Anim { get; private set; }
    public PlayerInputHandler inputHandler { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    #endregion
    #region Check Variables
    [SerializeField]
    private Transform groundCheck;
    #endregion
    #region Other Variables
    public Vector2 CurrentVelocity { get; private set; }  
    public int FacingDirection { get; private set; }    

    private Vector2 workSpace;
    #endregion

    #region Unity Callback Functions
    /// <summary>
    /// Set up all the different states
    /// </summary>
    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
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
        FacingDirection = 1;
        StateMachine.Initialize(IdleState);
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
    #endregion
    #region Check Functions
    /// <summary>
    /// Created a Sphere under the players feet to check if is grounded
    /// Takes a transform from this script, and a float and layer from PlayerDatacs// </summary>
    /// <returns></returns>
    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
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
    #region Other Functions
    /// <summary>
    /// Flips our player
    /// </summary>
    private void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0f, 180f, 0f);
    }
    #endregion
}
