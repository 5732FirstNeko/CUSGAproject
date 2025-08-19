using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//半山腰太挤，你总得去山顶看看//
public abstract class PlayerWeapon : MonoBehaviour
{
    protected int normallAttackNumber = 0;

    public PlayerWeaponDataSo weaponData;

    public event Action<Collider2D[]> AttackActionEvent;
    public event Action AttackCanExit_Move;
    public event Action AttackCanExit_Attack;
    public event Action AttackEndEvent;

    protected Animator animator;
    protected PlayerEnity playerEnity;
    protected MoveComponent moveComponent;
    protected StatsComponent statsComponent;

    private void Start()
    {
        playerEnity = GetComponent<PlayerEnity>();
        moveComponent = playerEnity.core.GetCoreComponent<MoveComponent>();
        statsComponent = playerEnity.core.GetCoreComponent<StatsComponent>();
        animator = GetComponent<Animator>();
    }

    public abstract void EnterAttack(AttackInputTypes attackInput);

    public virtual void ExitAttack()
    {
        animator.SetBool("Attack", false);
        animator.SetBool("SpecialAttack_1", false);

        AttackEndEvent?.Invoke();
    }

    protected void SetCanExitAttack_Move()
    {
        AttackCanExit_Move?.Invoke();
    }

    protected void SetCanExitAttack_Attack()
    {
        AttackCanExit_Attack?.Invoke();
    }

    protected virtual void ResetNormalAttackNumber()
    {
        normallAttackNumber = 0;
    }

    protected virtual void AttackAction(Collider2D[] detected)
    {
        AttackActionEvent?.Invoke(detected);
    }
}
