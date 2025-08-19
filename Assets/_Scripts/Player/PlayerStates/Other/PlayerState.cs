using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public class PlayerState : State
{
    protected PlayerEnity playerEnity;

    protected InputComponent inputComponent;
    protected MoveComponent moveComponent;
    protected CollisionComponent collisionComponent;
    protected StatsComponent statsComponent;

    public PlayerState(Enity enity, StateMachine stateMachine, Core core, string animName, PlayerEnity playerEnity) : base(enity, stateMachine, core, animName)
    {
        this.playerEnity = playerEnity;

        inputComponent = core.GetCoreComponent<InputComponent>();
        moveComponent = core.GetCoreComponent<MoveComponent>();
        collisionComponent = core.GetCoreComponent<CollisionComponent>();
        statsComponent = core.GetCoreComponent<StatsComponent>();
    }
}
