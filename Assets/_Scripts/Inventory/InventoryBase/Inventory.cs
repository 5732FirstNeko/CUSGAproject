using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public class Inventory : MonoBehaviour
{
    private static Inventory Instance;
    public static Inventory instance
    {
        get
        {
            if (Instance == null)
            {
                GameObject singletonObject = new GameObject("SingletonManager");
                Instance = singletonObject.AddComponent<Inventory>();
                DontDestroyOnLoad(singletonObject);
            }
            return Instance;
        }
    }

    [SerializeField] private GameObject slotUIPrefab;
    [SerializeField] private GameObject content;

    public List<Slot> slots = new List<Slot>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        content = transform.Find("Scroll View").Find("Viewport").Find("Content").gameObject;
    }

    // 添加物品到背包
    public void AddItem(ItemDataSo item)
    {
        foreach (Slot slot in slots)
        {
            if (slot.ItemData == item)
            {
                //TODO : 重写技能时长
                return;
            }
        }

        Slot newslot = Instantiate(slotUIPrefab,Vector3.zero,Quaternion.identity,content.transform).GetComponent<Slot>();
        newslot.SetItem(item);
        slots.Add(newslot);
    }

    public void RemoveItem(ItemDataSo itemData)
    {
        foreach (Slot slot in slots)
        {
            if (itemData == slot.ItemData)
            {
                slot.Clear();
            }
        }
    }
}
