using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public class PlayerEnity : Enity
{
    public PlayerWeapon weapon { get; private set; }

    public PlayerMoveState moveState { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerInAirState inAirState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerLandState landState { get; private set; }
    public PlayerAttackState attackState { get; private set; }
    public PlayerAttackEndState attackEndState { get; private set; }

    protected override void Start()
    {
        base.Start();

        moveState = new PlayerMoveState(this, stateMachine, core, "Move", this);
        idleState = new PlayerIdleState(this, stateMachine, core, "Idle", this);
        inAirState = new PlayerInAirState(this, stateMachine, core, "InAir", this);
        jumpState = new PlayerJumpState(this, stateMachine, core, "Jump", this);
        landState = new PlayerLandState(this, stateMachine, core, "Land", this); 
        attackState = new PlayerAttackState(this, stateMachine, core, "Attack", this);
        attackEndState = new PlayerAttackEndState(this, stateMachine, core,"Idle",this);

        weapon = GetComponent<PlayerWeapon>();

        stateMachine.Initialize(idleState);
    }

    //TODO : 实现player的异常状态进入的代码
    public override void KnockBackStateEnter(KnockBackStateData anomalousData)
    {
        
    }

    public override void CompulsionStateEnter(CompulsionStateData anomalousData)
    {
        
    }
}
