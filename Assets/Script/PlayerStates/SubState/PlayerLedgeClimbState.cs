using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    private Vector2 detectedPosition;
    private Vector2 cornerPosition;
    private Vector2 startPosition;
    private Vector2 stopPosition;
    private bool isClimbing;
    private bool jumpInput;
    private int xInput;
    private int yInput;
    public bool isHanging;

    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        // now telling the animator that the ledge has been climb and  an event in the animation ledgeclimb calls AnimationFinishedTrigger();
        player.Anim.SetBool("climbLedge", false);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        // now we know when it is trigger because we put an animation event on the Edge Grab animation refering to animationTrigger()
        isHanging = true;

    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocityZero();
        //we check our player position
        player.transform.position = detectedPosition;
        //we check the corner position
        cornerPosition = player.DetermineCornerPosition();
        // set the position between the player and the ledge
        // modify the startPos in player data if you want to change the position of the player regarding to the ledge
        startPosition.Set(cornerPosition.x - (player.FacingDirection * playerData.startOffSet.x), cornerPosition.y - playerData.startOffSet.y);
        //modify stop Pos in Player Data if you want to change the position when the player finishes climbing the ledge.
        stopPosition.Set(cornerPosition.x + (player.FacingDirection * playerData.stopOffSet.x), cornerPosition.y + playerData.stopOffSet.y);
        //finaly place the player at the calculated position at the start of our ledge grab
        player.transform.position = startPosition;
    }

    public override void Exit()
    {
        base.Exit();
        isHanging = false;
        //once we finish the climb we change our player position to the stop position
        if (isClimbing)
        {
            player.transform.position = stopPosition;
            isClimbing = false;
        }
        
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!isExitingState)
        {
            if (isAnimationFinished)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else
            {
                xInput = player.inputHandler.NormalizedInputX;
                yInput = player.inputHandler.NormalizedInputY;
                jumpInput = player.inputHandler.JumpInput;
                player.SetVelocityZero();
                player.transform.position = startPosition;

                if (yInput > 0 && isHanging && !isClimbing)
                {
                    isClimbing = true;
                    player.Anim.SetBool("climbLedge", true);
                }
               
                else if (yInput == -1 && isHanging && !isClimbing)
                {
                    stateMachine.ChangeState(player.InAirState);
                }
                else if (jumpInput && !isClimbing)
                {
                    //we know there is a wall in front of us because we are on a ledge so we are automaticly jumping opposite to the wall
                    player.WallJumpState.DeterminedWallJumpDirection(true);
                    stateMachine.ChangeState(player.WallJumpState);
                }
            }
        } 

    }

    public void SetDetectedPosition(Vector2 position) => detectedPosition = position;
}