using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public class MoveComponent : CoreComponent
{
    public Rigidbody2D Rig2D { get; private set; }
    public int FacingDirection { get;private set; }
    public Vector2 CurrentVelocity { get; private set; } = Vector2.zero;

    protected override void Awake()
    {
        base.Awake();

        Rig2D = GetComponentInParent<Rigidbody2D>();
        FacingDirection = 1;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CurrentVelocity = Rig2D.velocity;
    }

    public void SetVelocityZero()
    {
        Rig2D.velocityX = 0;
        Rig2D.velocityY = 0;
    }

    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        Rig2D.velocityX = angle.x * velocity * direction;
        Rig2D.velocityY = angle.y * velocity;
    }

    public void SetVelocity(Vector2 velocity)
    {
        Rig2D.velocityX = velocity.x;
        Rig2D.velocityY = velocity.y;
    }

    public void SetVelocity(float velocity, Vector2 direction)
    {
        Rig2D.velocity = velocity * direction;
    }

    public void SetVelocityX(float velocity)
    {
        Rig2D.velocityX = velocity * FacingDirection;
    }

    public void SetVelocityY(float velocity)
    {
        Rig2D.velocityY = velocity;
    }

    public void SetForeceX(float force,bool isImpuse = true)
    {
        if (isImpuse)
        {
            Rig2D.AddForceX(force, ForceMode2D.Impulse);
        }
        else
        {
            Rig2D.AddForceX(force,ForceMode2D.Force);
        }
    }

    public void SetForeceY(float force, bool isImpuse = true)
    {
        if (isImpuse)
        {
            Rig2D.AddForceY(force, ForceMode2D.Impulse);
        }
        else
        {
            Rig2D.AddForceY(force, ForceMode2D.Force);
        }
    }

    public void SetForce(Vector2 force, bool isImpuse = true)
    {
        if (isImpuse)
        {
            Rig2D.AddForce(force, ForceMode2D.Impulse);
        }
        else
        {
            Rig2D.AddForce(force, ForceMode2D.Impulse);
        }
    }

    public void Flip()
    {
        FacingDirection *= -1;
        SetVelocityX(0f);
        Rig2D.transform.Rotate(0.0f, 180.0f, 0.0f);
    }
}
