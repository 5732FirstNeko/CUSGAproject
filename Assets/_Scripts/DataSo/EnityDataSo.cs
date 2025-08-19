using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//半山腰太挤，你总得去山顶看看//
[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Enity Data")]
public class EnityDataSo : ScriptableObject
{
    [SerializeField] public float MoveVelocity;

    [SerializeField] public float startForce;
    [SerializeField] public float FlipForce;
    [SerializeField] public float EndDecelerateRate;

    [SerializeField] public float jumpVelocity;
    [SerializeField] public float jumpSpeed;
}
