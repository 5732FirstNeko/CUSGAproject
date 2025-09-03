using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.EventSystems;

//半山腰太挤，你总得去山顶看看//
public class MonsterCompulsionState : MonsterAnomalousState<CompulsionStateData>
{
    private bool isPassMidPoint;
    private float previousGravity;

    public MonsterCompulsionState(Enity enity, StateMachine stateMachine, Core core, string animName, MonsterEnity monsterEnity) : base(enity, stateMachine, core, animName, monsterEnity)
    {
    }

    public override void Enter()
    {
        base.Enter();

        isPassMidPoint = false;
        previousGravity = enity.rig2D.gravityScale;
        enity.rig2D.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();
        enity.rig2D.gravityScale = previousGravity;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        float progress;

        if (!isPassMidPoint)
        {
            progress = Mathf.Clamp01((Time.time - startTime) / anomalousData.time_MidPoint) * 0.5f;
        }
        else
        {
            progress = Mathf.Clamp01((Time.time - startTime - anomalousData.time_MidPoint) / anomalousData.time_EndPoint) * 0.5f + 0.5f;
        }

        if (Mathf.Abs(progress - 0.5f) < 0.01f)
        {
            isPassMidPoint = true;
        }

        // 计算抛物线高度
        float height = anomalousData.yCustomCurve(progress);

        if (Mathf.Abs(height - anomalousData.height) < 0.1f)
        {
            isPassMidPoint = true;
        }

        float velocity = anomalousData.xCustomCurve(progress);

        // 水平移动
        Vector2 newPos = new Vector2(velocity,height);

        // 应用新位置
        enity.transform.position = newPos;

        if (Vector3.Distance(enity.transform.position, anomalousData.endPosition) < 0.1f)
        {
            stateMachine.ChangState(monsterEnity.idleState);
        }

        if (progress == 1)
        {
            stateMachine.ChangState(monsterEnity.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
