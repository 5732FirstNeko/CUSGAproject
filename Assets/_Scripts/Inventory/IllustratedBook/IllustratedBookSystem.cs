using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public class IllustratedBookSystem : MonoBehaviour,IGameSaveAndLoad
{
    public List<IllustratedBookElement> illustratedBookElements = new List<IllustratedBookElement>();

    private GameObject content;

    private void Awake()
    {
        IllustratedBookInit();
    }

    public void IllustratedBookInit()
    {
        content = transform.Find("Viewport").Find("Content").gameObject;
        illustratedBookElements = content.GetComponentsInChildren<IllustratedBookElement>().ToList();
    }

    public void UnLockIllustratedBookElement(IllustratedBookElementDataSo data)
    {
        foreach(IllustratedBookElement element in illustratedBookElements)
        {
            if (element.IllustratedBookElementData == data)
            {
                element.UnLockIllustratedBookElement();
            }
        }
    }

    public string GenerateUniqueID()
    {
        return $"{gameObject.scene.name}/{GameManager.GetPath(transform)}";
    }

    public string CaptureData()
    {
        try
        {
            // 验证列表是否初始化，以及元素数量
            if (illustratedBookElements.Count == 0)
            {
                return "{}";
            }

            // 收集已解锁元素的ID（用SO的elementID或name作为标识）
            var saveData = new IllustratedBookSystemData
            {
                unlockedElements = illustratedBookElements
                    .Where(e => e.isUnLocked && e.IllustratedBookElementData != null)
                    .Select(e => e.IllustratedBookElementData.name)
                    .ToList()
            };

            // 序列化并返回（带日志验证）
            string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
            Debug.Log("图鉴系统数据序列化成功：" + json, this);
            return json;
        }
        catch (System.Exception ex)
        {
            Debug.LogError("图鉴系统序列化失败：" + ex.Message, this);
            return ""; // 出错时返回空（避免影响整体存档）
        }
    }

    public void RestoreData(string jsonData)
    {
        var saveData = JsonConvert.DeserializeObject<IllustratedBookSystemData>(jsonData);
        if (saveData == null) return;

        // 恢复每个元素的解锁状态
        foreach (var element in illustratedBookElements)
        {
            bool isUnlocked = saveData.unlockedElements.Contains(element.IllustratedBookElementData.name);
            if (isUnlocked)
            {
                element.UnLockIllustratedBookElement(); // 执行解锁逻辑（如显示图片、描述）
            }
            else
            {
                element.LockIllustratedBookElement(); // 确保未解锁的元素处于锁定状态
            }
        }
    }

    public class IllustratedBookSystemData
    {
        public List<string> unlockedElements = new List<string>();
    }

    private void OnEnable()
    {
        GameSaveAndLoadSystem.RegisterSaveable(this);
    }

    private void OnDestroy()
    {
        GameSaveAndLoadSystem.UnregisterSaveable(GenerateUniqueID());
    }
}
