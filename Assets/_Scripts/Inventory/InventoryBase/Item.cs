using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public class Item : MonoBehaviour
{
    [SerializeField]private ItemDataSo currentItemData;

    public void ItemInit(ItemDataSo data)
    {
        currentItemData = data;
        gameObject.name = currentItemData.itemName;
        gameObject.GetComponent<SpriteRenderer>().sprite = currentItemData.icon;
    }

    public ItemDataSo GetItemData()
    {
        return currentItemData;
    }
}
