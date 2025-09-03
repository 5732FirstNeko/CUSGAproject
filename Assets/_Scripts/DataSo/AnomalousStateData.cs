using System;
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
    public CompulsionStateData(Func<float,float> xCustomCurve, Func<float,float> yCustomCurve,
        Vector3 startPosition,Vector3 endPosition,
        float height,float time_MidPoint,float time_EndPoint) 
    {
        this.xCustomCurve = xCustomCurve;
        this.yCustomCurve = yCustomCurve;
        this.startPosition = startPosition;
        this.endPosition = endPosition;
        this.height = height;
        this.time_MidPoint = time_MidPoint;
        this.time_EndPoint = time_EndPoint;
    }

    public Func<float, float> xCustomCurve;
    public Func<float, float> yCustomCurve;

    public Vector3 startPosition;
    public Vector3 endPosition;

    public float height;
    public float time_MidPoint;//开始至到达中间值的时间
    public float time_EndPoint;//中间值至到达结束时的时间
}

public class LevitateStateData : AnomalousStateData
{
    public float levitateTime;
}