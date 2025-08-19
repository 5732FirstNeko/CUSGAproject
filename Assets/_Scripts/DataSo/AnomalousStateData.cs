using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public class AnomalousStateData { }

public class KnockBackStateData : AnomalousStateData
{
    public KnockBackStateData(float knockBackTime,Vector2 knockBackVelocity)
    {
        this.knockBackTime = knockBackTime;
        this.knockBackVelocity = knockBackVelocity;
    }

    public float knockBackTime;

    public Vector2 knockBackVelocity;
}


public class CompulsionStateData : AnomalousStateData
{
    public CompulsionStateData(Vector3 startPosition,Vector3 endPosition,float height,float trowTime_Heigestpoint,float trowTime_Endpoint) 
    {
        this.startPostion = startPosition;
        this.endPostion = endPosition;
        this.height = height;
        this.trowTime_Heigestpoint = trowTime_Heigestpoint;
        this.trowTime_Endpoint = trowTime_Endpoint;
    }

    public Vector3 startPostion;
    public Vector3 endPostion;

    public float height;
    public float trowTime_Heigestpoint;
    public float trowTime_Endpoint;
}