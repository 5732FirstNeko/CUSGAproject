using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//半山腰太挤，你总得去山顶看看//
public class DialogueSystem : MonoBehaviour
{
    [Header("DialogueUI")]
    [SerializeField] private GameObject dialoguePanel; // 对话面板
    [SerializeField] private TMP_Text speakerNameText; // 说话人名称
    [SerializeField] private TMP_Text contentText; // 对话内容
    [SerializeField] private Image speakerAvatarImage; // 说话人头像

    [Header("DialogueChoiceUI")]
    [SerializeField] private GameObject optionsObject; // 选项面板
    [SerializeField] private List<GameObject> optionsButtonObjects;
    [SerializeField] private List<Button> optionsButtons;
    [SerializeField] private List<TMP_Text> optionsButtonTexts;

    [Header("OtherDialogueUI")]
    [SerializeField] private Button nextButton; // 下一句按钮
    [SerializeField] private Button closeButton; // 关闭按钮

    private DialogueDataSo currentDialogueData;

    private int currentSentenceIndex = 0;

    private bool isinDialogue = false;

    public void StartDialogue(DialogueDataSo dialogueData)
    {
        if(isinDialogue)
            return;

        if (dialogueData == null || dialogueData.data.Count == 0)
        {
            Debug.LogError("对话数据为空或不存在！");
            return;
        }

        //EventCenter.BroadCast(EventType.DialogueStart);

        isinDialogue = true;
        currentDialogueData = dialogueData;
        currentSentenceIndex = 0;
        dialoguePanel.SetActive(true);

        speakerNameText.text = currentDialogueData.data[currentSentenceIndex].speakerName;
        speakerAvatarImage.sprite = currentDialogueData.data[currentSentenceIndex].speakerCharacter;
        contentText.text = currentDialogueData.data[currentSentenceIndex].dialogueContent;

        currentSentenceIndex++;
    }

    public void UpdateDialogueUI()
    {
        if (!isinDialogue)
        {
            Debug.LogError("未开启对话而调用" + GetType().Name + ".UpdateDialogueUI");
            return;
        }

        if (currentDialogueData.data.Count == currentSentenceIndex)
        {
            StopDialogue();
            return;
        }

        if (currentDialogueData.data[currentSentenceIndex].isDialogueChoice)
        {
            ShowChoiceButton(currentDialogueData.data[currentSentenceIndex].Choice.ToArray());
            return;
        }

        speakerNameText.text = currentDialogueData.data[currentSentenceIndex].speakerName;
        speakerAvatarImage.sprite = currentDialogueData.data[currentSentenceIndex].speakerCharacter;
        contentText.text = currentDialogueData.data[currentSentenceIndex].dialogueContent;

        currentSentenceIndex++;
    }

    public void StopDialogue()
    {
        dialoguePanel.SetActive(false);

        EventCenter.BroadCast(EventType.DialogueEnd);

        currentDialogueData.isFinished = true;
        currentDialogueData = null;
        currentSentenceIndex = 0;
    }

    public void ShowChoiceButton(params DialogueChoice[] dialogueChoice)
    {
        optionsObject.SetActive(true);

        for (int i = 0; i < dialogueChoice.Length; i++)
        {
            if (i >= optionsButtonObjects.Count || i >= optionsButtonTexts.Count || i >= optionsButtons.Count)
            {
                Debug.LogWarning($"选项数量超过可用按钮数量，忽略第{i}个选项");
                continue;
            }

            optionsButtonObjects[i].SetActive(true);
            optionsButtonTexts[i].text = dialogueChoice[i].choiceName;
            int index = i;
            optionsButtons[i].onClick.AddListener(() => AddChoiceinButton(dialogueChoice[index].nextDialogueData));
        }
    }

    private void AddChoiceinButton(DialogueDataSo dialogueSelectID)
    {
        isinDialogue = false;

        currentDialogueData.isFinished = true;

        StartDialogue(dialogueSelectID);

        optionsObject.SetActive(false);
        
        foreach(var obj in optionsButtonObjects)
        {
            obj.SetActive(false);
        }

        foreach (var obj in optionsButtons)
        {
            obj.onClick.RemoveAllListeners();
        }
    }

    public DialogueDataSo text;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            StartDialogue(text);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            UpdateDialogueUI();
        }
    }
}
