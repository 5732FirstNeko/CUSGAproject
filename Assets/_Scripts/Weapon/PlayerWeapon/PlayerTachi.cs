using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public class PlayerTachi : PlayerWeapon
{
    private Vector2 offset;
    

    public override void EnterAttack(AttackInputTypes attackInput)
    {
        if (attackInput.X_input)
        {
            animator.SetInteger("NormalAttacknumber", normallAttackNumber);
            animator.SetBool("Attack", true);
        }
    }

    private void NormalAttack()
    {
        offset.Set(transform.position.x + (weaponData.normalAttackData[0].Hitbox.center.x * 
            playerEnity.core.GetCoreComponent<MoveComponent>().FacingDirection), 
            transform.position.y + weaponData.normalAttackData[0].Hitbox.center.y);

        playerEnity.core.GetCoreComponent<MoveComponent>().SetVelocityZero();
        Collider2D[] detected = Physics2D.OverlapBoxAll(offset, weaponData.normalAttackData[0].Hitbox.size, 0f, weaponData.normalAttackData[0].DetectedLayer);

        foreach (Collider2D collider in detected)
        {
            collider.GetComponent<DamageComponent>().NormallDamage(playerEnity.core.GetCoreComponent<StatsComponent>().AttackDamage);
            AttackAction(detected);
        }

        if (normallAttackNumber < weaponData.normalAttackData.Count)
        {
            normallAttackNumber++;
        }
        else
        {
            ResetNormalAttackNumber();
        }

        GameManager.instance.StartTimer("TachiNormalAttack",weaponData.NormalAttackIntervalTime,ResetNormalAttackNumber);
    }

    

    /// <summary>
    /// 上挑攻击
    /// </summary>
    private void ElectroAttack()
    {
        offset.Set(transform.position.x + (weaponData.normalAttackData[0].Hitbox.center.x *
            playerEnity.core.GetCoreComponent<MoveComponent>().FacingDirection),
            transform.position.y + weaponData.normalAttackData[0].Hitbox.center.y);

        playerEnity.core.GetCoreComponent<MoveComponent>().SetVelocityZero();
        Collider2D[] detected = Physics2D.OverlapBoxAll(offset, weaponData.normalAttackData[1].Hitbox.size, 0f, weaponData.normalAttackData[1].DetectedLayer);

        foreach (Collider2D collider in detected)
        {
            collider.GetComponent<DamageComponent>().NormallDamage(playerEnity.core.GetCoreComponent<StatsComponent>().AttackDamage);
            collider.GetComponent<DamageComponent>().KnockBackDamage(1.5f,1f,5f);
            AttackAction(detected);
        }
    }

    private void OnDrawGizmos()
    {
        if (weaponData.normalAttackData[0].IsShowHitBox)
        {
            Vector2 tempoffset;
            tempoffset.x = transform.position.x + weaponData.normalAttackData[0].Hitbox.x;
            tempoffset.y = transform.position.y + weaponData.normalAttackData[0].Hitbox.y;
            Gizmos.DrawWireCube(tempoffset, weaponData.normalAttackData[0].Hitbox.size);
        }
    }
}
