using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    #region State Variables
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;
    #endregion

    protected float startTime;
    protected bool isAnimationFinished;
    protected bool isExitingState;

    private string animBoolName;

    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
    }
    public virtual void Enter()
    {
        DoChecks();
        player.Anim.SetBool(animBoolName,true);
        // check the time when it enters a state
        startTime = Time.time;
        Debug.Log(animBoolName);
        isAnimationFinished = false;
        isExitingState = false;
    }
    public virtual void Exit()
    {
        player.Anim.SetBool(animBoolName,false);
        isExitingState = true;

    }
    public virtual void LogicUpdate()
    {

    }
    public virtual void PhysicsUpdate()
    {
        DoChecks();
        player.CheckGravity();
    }
    public virtual void DoChecks()  {  }
    public virtual void AnimationTrigger() { }
    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
}
