using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public class DamageComponent : CoreComponent
{
    private Enity enity;
    private StatsComponent statsComponent;
    private MoveComponent moveComponent;

    private void Start()
    {
        statsComponent = core.GetCoreComponent<StatsComponent>();
        moveComponent = core.GetCoreComponent<MoveComponent>();
        enity = GetComponentInParent<Enity>();
    }

    public void NormallDamage(float Damage, int FacingDirection = 1)
    {
        statsComponent.HPChange(-Damage);
        if (moveComponent.FacingDirection == FacingDirection)
        {
            moveComponent.Flip();
        }
    }

    public void KnockBackDamage(float time, float x = 0, float y = 0)
    {
        enity.KnockBackStateEnter(new KnockBackStateData(time, new Vector2(x, y)));
    }

    public void CompulsionDamage(Vector3 startPosition,Vector3 endfPosition,float height,float time_Heighest,float time_EndPoint)
    {
        CompulsionStateData stateData = new CompulsionStateData(startPosition, endfPosition, height, time_Heighest, time_EndPoint);

        enity.CompulsionStateEnter(stateData);
    }
}
