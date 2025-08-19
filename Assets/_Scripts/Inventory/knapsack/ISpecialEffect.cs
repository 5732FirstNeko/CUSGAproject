using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public interface ISpecialEffect
{
    public abstract void SpecialEffect(Collider2D[] detected);
}

public enum SpecialEffect
{
    PoisonAttack,      // 攻击附带毒属性
    ExtendedDodge,     // 延长闪避时间
    LifeSteal,         // 吸血效果
    CriticalMultiplier // 暴击伤害提升
}