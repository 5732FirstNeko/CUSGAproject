using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//半山腰太挤，你总得去山顶看看//
public class MonsterCompulsionState : MonsterAnomalousState<CompulsionStateData>
{
    private bool isPassHeigestPoint;
    private float previousGravity;

    public MonsterCompulsionState(Enity enity, StateMachine stateMachine, Core core, string animName, MonsterEnity monsterEnity) : base(enity, stateMachine, core, animName, monsterEnity)
    {
    }

    public override void Enter()
    {
        base.Enter();

        isPassHeigestPoint = false;
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

        if (!isPassHeigestPoint)
        {
            progress = Mathf.Clamp01((Time.time - startTime) / anomalousData.trowTime_Heigestpoint) * 0.5f;
        }
        else
        {
            progress = Mathf.Clamp01((Time.time - startTime - anomalousData.trowTime_Heigestpoint) / anomalousData.trowTime_Endpoint) * 0.5f + 0.5f;
        }

        if (Mathf.Abs(progress - 0.5f) < 0.01f)
        {
            isPassHeigestPoint = true;
        }

        // 计算抛物线高度
        float height = Mathf.Sin(progress * Mathf.PI) * anomalousData.height;

        if (Mathf.Abs(height - anomalousData.height) < 0.1f)
        {
            isPassHeigestPoint = true;
        }

        // 水平移动
        Vector2 newPos = Vector2.Lerp(anomalousData.startPostion, anomalousData.endPostion, progress);
        // 添加垂直高度
        newPos.y += height;

        // 应用新位置
        enity.transform.position = newPos;

        if (Vector3.Distance(enity.transform.position, anomalousData.endPostion) < 0.1f)
        {
            stateMachine.ChangState(monsterEnity.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
