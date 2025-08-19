using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public class PlayerLongSpear : PlayerWeapon
{
    private Vector2 offset;

    #region WaltzsticksValue
    [SerializeField] private Transform Waltzsticks_StartPosition;
    [SerializeField] private Transform Waltzsticks_EndPosition;
    [SerializeField] private float height;
    [SerializeField] private float time_Heighestpoint;
    [SerializeField] private float time_Endpoint;
    #endregion

    public override void EnterAttack(AttackInputTypes attackInput)
    {
        if (attackInput.X_input)
        {
            animator.SetInteger("NormalAttacknumber", normallAttackNumber);
            animator.SetBool("Attack", true);
        }
        else if (attackInput.Y_input)
        {
            animator.SetBool("SpecialAttack_1", true);
            animator.SetBool("Attack", true);
        }
        else
        {
            ExitAttack();
        }
    }

    private void NormalAttack()
    {
        offset.Set(transform.position.x + (weaponData.normalAttackData[normallAttackNumber].Hitbox.center.x *
           moveComponent.FacingDirection),
           transform.position.y + weaponData.normalAttackData[normallAttackNumber].Hitbox.center.y);

        moveComponent.SetVelocity(new Vector2(weaponData.normalAttackData[normallAttackNumber].MoveStrength.x * moveComponent.FacingDirection,
            weaponData.normalAttackData[normallAttackNumber].MoveStrength.y));
        Collider2D[] detected = Physics2D.OverlapBoxAll(offset, weaponData.normalAttackData[normallAttackNumber].Hitbox.size,
            0f, weaponData.normalAttackData[normallAttackNumber].DetectedLayer);

        if (detected != null)
        {
            CameraShakeManager.Instance.TriggerHitStop(0.11f);
        }

        CameraShakeManager.Instance.TriggerDirectionalShake(0.075f,0.1f,true,true,CameraShakeManager.ShakeType.Default);

        foreach (Collider2D collider in detected)
        {
            collider.GetComponent<Enity>().core.GetCoreComponent<DamageComponent>().
                NormallDamage(statsComponent.AttackDamage,moveComponent.FacingDirection);
            collider.GetComponent<Enity>().core.GetCoreComponent<DamageComponent>().KnockBackDamage(0.5f, 
                weaponData.normalAttackData[normallAttackNumber].KnockbackStrength.x * 
                moveComponent.FacingDirection, 
                weaponData.normalAttackData[normallAttackNumber].KnockbackStrength.y);
        }
        AttackAction(detected);

        if (normallAttackNumber < weaponData.normalAttackData.Count - 1)
        {
            normallAttackNumber++;
        }
        else
        {
            ResetNormalAttackNumber();
            GameManager.instance.StopTimer("LongSpearNormalAttack");
        }

        GameManager.instance.StartTimer("LongSpearNormalAttack", weaponData.NormalAttackIntervalTime, ResetNormalAttackNumber);
    }

    #region Waltzsticks

    private void Waltzsticks_NoUseGravity()
    {
        playerEnity.rig2D.gravityScale = 0;
    }

    private void Waltzsticks_UseGravity()
    {
        playerEnity.rig2D.gravityScale = 5;
    }

    private void Waltzsticks_FlipPlayer()
    {
        playerEnity.core.GetCoreComponent<MoveComponent>().Flip();
    }

    private void Waltzsticks_PickUpEnemy()
    {
        offset.Set(transform.position.x + (weaponData.SpecialAttackData[0].Hitbox.center.x *
           moveComponent.FacingDirection),
           transform.position.y + weaponData.SpecialAttackData[0].Hitbox.center.y);

        Collider2D[] detected = Physics2D.OverlapBoxAll(offset, weaponData.normalAttackData[normallAttackNumber].Hitbox.size,
            0f, weaponData.SpecialAttackData[0].DetectedLayer);

        CameraShakeManager.Instance.TriggerDirectionalShake(0.05f, 0.05f, true, true, CameraShakeManager.ShakeType.Default);

        if (detected != null)
        {
            CameraShakeManager.Instance.TriggerHitStop(0.1f);
        }

        foreach (Collider2D collider in detected)
        {
            DamageComponent damageComponentTemp = collider.GetComponent<Enity>().core.GetCoreComponent<DamageComponent>();

            damageComponentTemp.NormallDamage(statsComponent.AttackDamage, moveComponent.FacingDirection);
            damageComponentTemp.CompulsionDamage(Waltzsticks_StartPosition.position,Waltzsticks_EndPosition.position,
                height,time_Heighestpoint,time_Endpoint);
        }
    }

    private void Waltzsticks_LastDamage()
    {
        
    }
    #endregion

    private void OnDrawGizmos()
    {
        if (weaponData.normalAttackData[0].IsShowHitBox)
        {
            Vector2 tempoffset;
            tempoffset.x = transform.position.x + weaponData.normalAttackData[0].Hitbox.x;
            tempoffset.y = transform.position.y + weaponData.normalAttackData[0].Hitbox.y;
            Gizmos.DrawWireCube(tempoffset, weaponData.normalAttackData[0].Hitbox.size);
        }

        if (weaponData.normalAttackData[1].IsShowHitBox)
        {
            Vector2 tempoffset;
            tempoffset.x = transform.position.x + weaponData.normalAttackData[1].Hitbox.x;
            tempoffset.y = transform.position.y + weaponData.normalAttackData[1].Hitbox.y;
            Gizmos.DrawWireCube(tempoffset, weaponData.normalAttackData[1].Hitbox.size);
        }

        if (weaponData.normalAttackData[2].IsShowHitBox)
        {
            Vector2 tempoffset;
            tempoffset.x = transform.position.x + weaponData.normalAttackData[2].Hitbox.x;
            tempoffset.y = transform.position.y + weaponData.normalAttackData[2].Hitbox.y;
            Gizmos.DrawWireCube(tempoffset, weaponData.normalAttackData[2].Hitbox.size);
        }

        if (weaponData.SpecialAttackData[0].IsShowHitBox)
        {
            Vector2 tempoffset;
            tempoffset.x = transform.position.x + weaponData.SpecialAttackData[0].Hitbox.x;
            tempoffset.y = transform.position.y + weaponData.SpecialAttackData[0].Hitbox.y;
            Gizmos.DrawWireCube(tempoffset, weaponData.SpecialAttackData[0].Hitbox.size);
        }
    }
}
