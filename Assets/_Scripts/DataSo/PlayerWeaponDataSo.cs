using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
[CreateAssetMenu(fileName = "newWeaponData", menuName = "Data/Player Weapon Data")]
public class PlayerWeaponDataSo : ScriptableObject
{
    [SerializeField] public RuntimeAnimatorController WeaponAnimatorcontroller;
    [field: SerializeField] public float NormalAttackIntervalTime { get; private set; }

    public List<Attack> normalAttackData;
    public List<Attack> SpecialAttackData;
}

[Serializable]
public class Attack
{
    [field: SerializeField] public string SkillName {  get; private set; }
    [field: SerializeField] public float CoolDown {  get; private set; }
    [field: SerializeField] public Rect Hitbox { get; private set; }
    [field: SerializeField] public Vector2 KnockbackStrength { get; private set; }
    [field: SerializeField] public Vector2 MoveStrength { get; private set; }
    [field: SerializeField] public LayerMask DetectedLayer { get; private set; }

    [field: SerializeField] public bool IsShowHitBox { get; private set; }
}
