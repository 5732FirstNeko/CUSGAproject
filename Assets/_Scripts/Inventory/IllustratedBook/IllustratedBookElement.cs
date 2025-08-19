using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//半山腰太挤，你总得去山顶看看//
public class IllustratedBookElement : MonoBehaviour
{
    [SerializeField] private IllustratedBookElementDataSo _illustratedBookElementData;
    public IllustratedBookElementDataSo IllustratedBookElementData => _illustratedBookElementData;

    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private GameObject lockIndicator;

    public bool isUnLocked = true;

    private void Awake()
    {
        itemIcon = transform.Find("icon").GetComponent<Image>();
        descriptionText = GetComponentInChildren<TMP_Text>();
        lockIndicator = transform.Find("LockIndicator").gameObject;
    }

    public void UnLockIllustratedBookElement()
    {
        isUnLocked = true;

        itemIcon.sprite = IllustratedBookElementData.icon;
        descriptionText.text = IllustratedBookElementData.description;
        lockIndicator.SetActive(false);
    }

    public void LockIllustratedBookElement()
    {
        isUnLocked = false;

        itemIcon.sprite = null;
        descriptionText.text = string.Empty;
        lockIndicator.SetActive(true);
    }
}
