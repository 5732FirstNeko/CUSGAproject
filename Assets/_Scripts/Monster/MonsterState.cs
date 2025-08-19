using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public class MonsterState : State
{
    protected MonsterEnity monsterEnity;

    protected MoveComponent moveComponent;
    protected CollisionComponent collisionComponent;
    protected StatsComponent statsComponent;

    public MonsterState(Enity enity, StateMachine stateMachine, Core core, string animName, MonsterEnity monsterEnity) : base(enity, stateMachine, core, animName)
    {
        this.monsterEnity = monsterEnity;

        moveComponent = core.GetCoreComponent<MoveComponent>();
        collisionComponent = core.GetCoreComponent<CollisionComponent>();
        statsComponent = core.GetCoreComponent<StatsComponent>();
    }
}
