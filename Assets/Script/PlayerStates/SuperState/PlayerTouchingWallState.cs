public class PlayerTouchingWallState : PlayerState
{
    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool grabInput;
    protected bool jumpInput;
    protected int xInput;
    protected int yInput;


    public PlayerTouchingWallState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckIfTouchingWall();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        xInput = player.inputHandler.NormalizedInputX;
        yInput = player.inputHandler.NormalizedInputY;
        grabInput = player.inputHandler.GrabInput;
        jumpInput = player.inputHandler.JumpInput;
        //transitioning to idle state / ground state
        if (!isExitingState)
        {
            if (jumpInput)
            {
                player.WallJumpState.DeterminedWallJumpDirection(isTouchingWall);
                stateMachine.ChangeState(player.WallJumpState);
            }
            else if (isGrounded && !grabInput)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            //transition to air state
            else if (!isTouchingWall || (xInput != player.FacingDirection && !grabInput))
            {
                stateMachine.ChangeState(player.InAirState);
            }

        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}