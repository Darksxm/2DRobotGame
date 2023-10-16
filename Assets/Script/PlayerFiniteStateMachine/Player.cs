using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }


    private PlayerData playerData;
    #endregion
    #region Components
    public Animator Anim { get; private set; }
    public PlayerInputHandler inputHandler { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    #endregion
    #region Other Variables
    public Vector2 CurrentVelocity { get; private set; }  
    public int FacingDirection { get; private set; }    
    [SerializeField]

    private Vector2 workSpace;
    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");  
    }

    private void Start()
    {
        Anim = GetComponent<Animator>();
        inputHandler = GetComponent<PlayerInputHandler>();
        Rb = GetComponent<Rigidbody2D>();
        FacingDirection = 1;
        StateMachine.Initialize(IdleState);
    }
    private void Update()
    {
        CurrentVelocity = Rb.velocity;
        StateMachine.CurrentState.LogicUpdate();
       
    }
    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion

    #region Set Functions
    public void SetVelocityX(float velocity)
    {
        workSpace.Set(velocity, CurrentVelocity.y);
        Rb.velocity = workSpace;
        CurrentVelocity = workSpace;
    }
    #endregion
    #region Check Functions
    public void CheckIfShouldFlip(int xInput)
    {
        if(xInput !=0 && xInput != FacingDirection)
        {
            Flip();
        }
    }
    #endregion
    #region Other Functions
    private void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0f, 180f, 0f);
    }
    #endregion
}
