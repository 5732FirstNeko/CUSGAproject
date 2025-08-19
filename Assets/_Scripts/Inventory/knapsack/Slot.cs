using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Image))]
public class Slot : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    //TODO : PlayerEnity的获取
    public PlayerEnity playerEnity;

    // ========== 槽位状态 ==========
    [Header("槽位状态")]
    public bool IsSelected { get; private set; } = false;

    // ========== 槽位数据 ==========
    [Header("槽位数据")]
    public ItemDataSo ItemData { get; private set; }
    public float UseTime { get; private set; }

    // ========== UI 组件 ==========
    [Header("UI 组件")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private GameObject selectionIndicator;

    // ========== 颜色配置 ==========
    [Header("颜色配置")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = new Color(0.8f, 0.8f, 0.8f, 1f);
    [SerializeField] private Color selectedColor = new Color(0.7f, 0.9f, 1f, 1f);

    // ========== 事件回调 ==========
    public delegate void SlotEvent(Slot slot);
    public static event SlotEvent OnSlotSelected;
    public static event SlotEvent OnSlotDeselected;
    public static event SlotEvent OnItemDropped;
    public static event SlotEvent OnItemUsed;

    private void Awake()
    {
        // 获取组件引用
        backgroundImage = GetComponent<Image>();
        itemIcon = transform.Find("icon").GetComponent<Image>();
        timeText = GetComponentInChildren<TMP_Text>();
        selectionIndicator = transform.Find("SelectIcon").gameObject;
        UseTime = ItemData.Time;

        EventCenter.AddListener(EventType.RoomStart, ItemEffectStart);
        EventCenter.AddListener(EventType.RoomEnd, ItemEffectPause);

        // 初始化UI状态
        UpdateSlotUI();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.RoomStart, ItemEffectStart);
        EventCenter.RemoveListener(EventType.RoomEnd, ItemEffectPause);
    }

    // ========== 槽位操作 ==========

    /// <summary>
    /// 设置槽位中的物品
    /// </summary>
    public void SetItem(ItemDataSo newitemData)
    {

        ItemData = newitemData;

        //==================
        foreach (AttributeBonus attribute in newitemData.attributeBonus)
        {
            attribute.ActiveAttributeBonus(playerEnity);
        }

        foreach (var specialEffect in newitemData.specialEffects)
        {
            specialEffect.ActiveEffect(playerEnity);
        }
        //==================

        GameManager.instance.StartTimer(ItemData.itemName,UseTime,Clear);
        UpdateSlotUI();
    }

    public void ItemEffectPause()
    {
        GameManager.instance.PauseTimer(ItemData.itemName);
    }

    public void ItemEffectStart()
    {
        GameManager.instance.ReStartTimer(ItemData.itemName);
    }

    /// <summary>
    /// 清空槽位
    /// </summary>
    public void Clear()
    {
        foreach (AttributeBonus attribute in ItemData.attributeBonus)
        {
            attribute.ReMoveAttributeBonus(playerEnity);
        }

        foreach (var specialEffect in ItemData.specialEffects)
        {
            specialEffect.ReMoveEffect(playerEnity);
        }

        if (Inventory.instance != null)
        {
            Inventory.instance.slots.Remove(this);
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// 选择槽位
    /// </summary>
    public void Select()
    {
        IsSelected = true;
        selectionIndicator.SetActive(true);
        backgroundImage.color = selectedColor;
        OnSlotSelected?.Invoke(this);
    }

    /// <summary>
    /// 取消选择槽位
    /// </summary>
    public void Deselect()
    {
        IsSelected = false;
        selectionIndicator.SetActive(false);
        backgroundImage.color = normalColor;
        OnSlotDeselected?.Invoke(this);
    }


    // ========== UI 更新 ==========
    private void UpdateSlotUI()
    {
        itemIcon.gameObject.SetActive(true);
    }

    // ========== 指针事件处理 ==========
    public void OnPointerEnter(PointerEventData eventData)
    {
        backgroundImage.color = IsSelected ? selectedColor : hoverColor;

        // 显示物品提示（需要TooltipManager实现）
        GameManager.instance.ShowTooltip(ItemData.itemName, ItemData.description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        backgroundImage.color = IsSelected ? selectedColor : normalColor;

        // 隐藏物品提示
        GameManager.instance.HideTooltip();
    }
}