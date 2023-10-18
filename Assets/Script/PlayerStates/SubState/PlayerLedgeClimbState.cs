using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    private Vector2 detectedPosition;
    private Vector2 cornerPosition;
    private Vector2 startPosition;
    private Vector2 stopPosition;

    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocityZero();
        player.transform.position = detectedPosition;
        cornerPosition = player.DetermineCornerPosition();
        // set the position between the player and the ledge
        // modify the startPos in player data if you want to change the position of the player regarding to the rain
        startPosition.Set(cornerPosition.x - (player.FacingDirection * playerData.startOffSet.x), cornerPosition.y - playerData.startOffSet.y);
        stopPosition.Set(cornerPosition.x + (player.FacingDirection * playerData.stopOffSet.x), cornerPosition.y + playerData.stopOffSet.y);
        player.transform.position = startPosition;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        player.SetVelocityZero();
        player.transform.position = startPosition;
    }

    public void SetDetectedPosition(Vector2 position) => detectedPosition = position;
}