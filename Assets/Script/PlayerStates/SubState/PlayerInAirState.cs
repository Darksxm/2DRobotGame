using UnityEngine;

public class PlayerInAirState : PlayerState
{
    //Input 
    private int xInput;
    private bool jumpInput;
    private bool jumpInputStop;
    private bool grabInput;
    private bool dashInput;

    //Checks
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingWallBack;
    private bool oldIsTouchingWall;
    private bool oldIsTouchingWallBack;
    private bool isTouchingLedge;

    //Others
    //checks if there is enought time between the moment we are in air and not grounded
    private bool cayoteTime;
    private bool wallJumpCayoteTime;
    public bool isJumping;

    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        oldIsTouchingWall = isTouchingWall;
        oldIsTouchingWallBack = isTouchingWallBack;

        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckIfTouchingWall();
        isTouchingWallBack = player.CheckIfTouchingWallBack();
        isTouchingLedge = player.CheckIfTouchingLedge();
       

        if (isTouchingWall && !isTouchingLedge)
        {
            player.LedgeClimbState.SetDetectedPosition(player.transform.position);
        }
        //if we are currently mid air but in the previous frame we touched the wall then we can start the cayote time for the wall jump
        if (!wallJumpCayoteTime && !isTouchingWallBack && !isTouchingWall && (oldIsTouchingWall || oldIsTouchingWallBack))
        {
            StartWallJumpCayoteTime();
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        oldIsTouchingWall = false;
        oldIsTouchingWallBack = false;
        isTouchingWall = false;
        isTouchingWallBack = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCayoteTime();
        CheckWallJumpCayoteTime();

        xInput = player.inputHandler.NormalizedInputX;
        jumpInput = player.inputHandler.JumpInput;
        jumpInputStop = player.inputHandler.JumpInputStop;
        grabInput = player.inputHandler.GrabInput;
        dashInput = player.inputHandler.DashInput;

        CheckJumpMultiplier();

        if (player.inputHandler.AttackInputs[(int)CombatInputs.primary])
        {
            stateMachine.ChangeState(player.PrimaryAttackState);
        }
        else if (player.inputHandler.AttackInputs[(int)CombatInputs.secondary])
        {
            stateMachine.ChangeState(player.SecondaryAttackState);
        }
        //switch to landState
       else if (isGrounded && player.CurrentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.LandState);
        }
        //switch to Climb Ledge
        else if (isTouchingWall && !isTouchingLedge && !isGrounded)
        {
            stateMachine.ChangeState(player.LedgeClimbState);
        }
        //switch to wall jump
        else if (jumpInput && !isJumping && (isTouchingWall || isTouchingWallBack || wallJumpCayoteTime))
        {
            StopWallJumpCayoteTime();
            isTouchingWall = player.CheckIfTouchingWall();
            player.WallJumpState.DeterminedWallJumpDirection(isTouchingWall);
            stateMachine.ChangeState(player.WallJumpState);
        }
        //switch to JumpState
        else if (jumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        }
        //switch to wall grab state
        else if (isTouchingWall && grabInput && isTouchingLedge)
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
        //switch to wall slide state
        else if (isTouchingWall && xInput == player.FacingDirection && player.CurrentVelocity.y <= 0)
        {
            stateMachine.ChangeState(player.WallSlideState);
        }
        else if (dashInput && player.DashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
        else
        {
            //checks the movement while in the air
            player.CheckIfShouldFlip(xInput);
            player.SetVelocityX(playerData.movementVelocity * xInput);
            //setting up the animation
            player.Anim.SetFloat("yVelocity", player.CurrentVelocity.y);
            player.Anim.SetFloat("xVelocity", Mathf.Abs(player.CurrentVelocity.x));//MathF to return a positive value in the animator always
        }
    }

    /// <summary>
    /// if we keep pressing space we jump heighter
    /// </summary>
    private void CheckJumpMultiplier()
    {
        if (isJumping)
        {
            if (jumpInputStop)
            {
                player.SetVelocityY(player.CurrentVelocity.y * playerData.variableJumpHeightMultiplier);
                isJumping = false;
            }
            else if (player.CurrentVelocity.y <= 0)
            {
                isJumping = false;
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    #region Check Functions

    /// <summary>
    /// checks time-laps when we press the Jump button and the amount of time we are in air so that we still
    /// have a bit of time before falling and we can jump
    /// </summary>
    private void CheckCayoteTime()
    {
        if (cayoteTime == true && Time.time > startTime + playerData.cayoteTime)
        {
            cayoteTime = false;
            player.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    private void CheckWallJumpCayoteTime()
    {
        if (wallJumpCayoteTime == true && Time.time > startTime + playerData.startWallJumpCayoteTime)
        {
            wallJumpCayoteTime = false;
            player.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    #endregion Check Funtions

    public void StartCayoteTime() => cayoteTime = true;

    public void StartWallJumpCayoteTime()
    {
        wallJumpCayoteTime = true;
     /*   playerData.startWallJumpCayoteTime = Time.time;*/
    }

    public void StopWallJumpCayoteTime()
    {
        wallJumpCayoteTime = false;
       
    }

    public void SetIsJumping() => isJumping = true;
}