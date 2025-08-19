using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
[CreateAssetMenu(fileName = "newDialogueData", menuName = "Data/Dialogue Data")]
public class DialogueDataSo : ScriptableObject
{
    public string dialogueID;
    public bool isFinished;
    public List<DialogueData> data = new List<DialogueData>();
}

[System.Serializable]
public class DialogueData
{
    public bool isDialogueChoice;
    public string speakerName = string.Empty;
    public Sprite speakerCharacter;
    [TextArea] public string dialogueContent = string.Empty;
    public List<DialogueChoice> Choice = new List<DialogueChoice>();
}

[System.Serializable]
public class DialogueChoice
{
    public string choiceName;
    public DialogueDataSo nextDialogueData;
}