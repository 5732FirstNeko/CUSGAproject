using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/IllustratedBookElement Data")]
public class IllustratedBookElementDataSo : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    [TextArea] public string description;
}
