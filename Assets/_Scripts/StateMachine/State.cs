using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public class State
{
    protected Core core;

    protected Enity enity;
    protected StateMachine stateMachine;

    protected float startTime;

    protected string animName;

    public State(Enity enity, StateMachine stateMachine, Core core, string animName)
    {
        this.enity = enity;
        this.stateMachine = stateMachine;
        this.core = core;
        this.animName = animName;
    }

    public virtual void Enter()
    {
        enity.animator.SetBool(animName,true);
        startTime = Time.time;
    }

    public virtual void Exit()
    {
        enity.animator.SetBool(animName,false);
    }
    public virtual void LogicUpdate()
    {
        
    }

    public virtual void PhysicsUpdate()
    {
        
    }
}
