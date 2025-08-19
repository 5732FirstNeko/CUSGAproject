using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
[CreateAssetMenu(fileName = "newItemData", menuName = "Data/ItemData Data")]
public class ItemDataSo : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public float Time = 0;
    [TextArea] public string description;

    public List<AttributeBonus> attributeBonus;
    public List<SpecialEffectData> specialEffects;
}

[Serializable]
public class AttributeBonus
{
    public AttributeBonusType type;
    public float amount;
    public ModifierType modifierType; // 加法或乘法

    //TODO : 完善属性加成的代码

    public void ActiveAttributeBonus(PlayerEnity playerEnity)
    {
        switch (type)
        {
            case AttributeBonusType.HPBonus:
                playerEnity.core.GetCoreComponent<StatsComponent>().HPRateChange(amount);
                break;
        }
    }

    public void ReMoveAttributeBonus(PlayerEnity playerEnity)
    {
        switch (type)
        {
            case AttributeBonusType.HPBonus:
                playerEnity.core.GetCoreComponent<StatsComponent>().HPRateChange(amount);
                break;
        }
    }
}

[Serializable]
public class SpecialEffectData
{
    public SpecialEffect effectType;
    public float value; // 效果值（如毒伤害、延长时间等）
    public ISpecialEffect effect;

    //TODO : 完善特殊效果的代码

    public void ActiveEffect(PlayerEnity playerEnity)
    {
        switch (effectType)
        {
            case SpecialEffect.PoisonAttack:
                PoisonSpecialEffect temp = effect as PoisonSpecialEffect;
                playerEnity.weapon.AttackActionEvent -= temp.SpecialEffect;
                break;
        }
    }

    public void ReMoveEffect(PlayerEnity playerEnity)
    {
        switch (effectType)
        {
            case SpecialEffect.PoisonAttack:
                PoisonSpecialEffect temp = effect as PoisonSpecialEffect;
                playerEnity.weapon.AttackActionEvent -= temp.SpecialEffect;
                break;
        }
    }
}

public enum AttributeBonusType
{
    AttackBonus,
    HPBonus,
    FireHurt
}


public enum ModifierType
{
    Additive,
    Multiplicative
}

