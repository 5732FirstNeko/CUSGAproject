using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(DialogueDataSo))]
public class DialogueDataSoEditor : Editor
{
    private SerializedProperty dialogueID;
    private SerializedProperty isFinished;
    private SerializedProperty dataList;

    // 存储每个元素的折叠状态（key：元素索引，value：是否展开）
    private Dictionary<int, bool> foldoutStates = new Dictionary<int, bool>();

    private void OnEnable()
    {
        // 绑定字段
        dialogueID = serializedObject.FindProperty("dialogueID");
        isFinished = serializedObject.FindProperty("isFinished");
        dataList = serializedObject.FindProperty("data");

        // 初始化折叠状态（默认全部展开）
        for (int i = 0; i < dataList.arraySize; i++)
        {
            if (!foldoutStates.ContainsKey(i))
                foldoutStates[i] = true;
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // 绘制基础字段
        EditorGUILayout.PropertyField(dialogueID);
        EditorGUILayout.PropertyField(isFinished);
        EditorGUILayout.Space();

        // 绘制对话数据列表
        EditorGUILayout.LabelField("DialogueDataList", EditorStyles.boldLabel);

        // 处理列表长度变化（新增元素时初始化折叠状态）
        while (foldoutStates.Count < dataList.arraySize)
        {
            foldoutStates[foldoutStates.Count] = true; // 新元素默认展开
        }
        while (foldoutStates.Count > dataList.arraySize)
        {
            foldoutStates.Remove(foldoutStates.Count - 1); // 移除超出的索引
        }

        // 遍历所有元素
        for (int i = 0; i < dataList.arraySize; i++)
        {
            SerializedProperty dataElement = dataList.GetArrayElementAtIndex(i);
            bool isLastElement = (i == dataList.arraySize - 1);

            // 获取当前元素的折叠状态
            bool isExpanded = foldoutStates[i];

            // 绘制折叠按钮（左侧箭头）+ 元素标题
            isExpanded = EditorGUILayout.Foldout(isExpanded, $"Element {i + 1}/{dataList.arraySize}", true);
            foldoutStates[i] = isExpanded; // 更新折叠状态

            // 折叠状态下不绘制内部字段
            if (!isExpanded)
            {
                EditorGUILayout.Space(); // 留空分隔
                continue;
            }

            // 展开状态：绘制内部字段（用缩进框包裹，模拟原生列表样式）
            using (new EditorGUI.IndentLevelScope()) // 增加缩进，区分层级
            using (var scope = new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                // 获取当前元素的所有字段
                var isDialogueChoice = dataElement.FindPropertyRelative("isDialogueChoice");
                var speakerName = dataElement.FindPropertyRelative("speakerName");
                var speakerCharacter = dataElement.FindPropertyRelative("speakerCharacter");
                var dialogueContent = dataElement.FindPropertyRelative("dialogueContent");
                var choiceList = dataElement.FindPropertyRelative("Choice");

                // 仅最后一个元素显示 isDialogueChoice 开关
                if (isLastElement)
                {
                    EditorGUILayout.PropertyField(isDialogueChoice);
                }

                // 根据状态显示字段
                if (isLastElement && isDialogueChoice.boolValue)
                {
                    // 最后一个元素且是选项：只显示选项列表
                    EditorGUILayout.PropertyField(choiceList, new GUIContent("DialogueChoiceList"), true);
                }
                else
                {
                    // 其他情况：显示常规字段
                    EditorGUILayout.PropertyField(speakerName);
                    EditorGUILayout.PropertyField(speakerCharacter);
                    EditorGUILayout.PropertyField(dialogueContent);
                }
            }
        }

        // 列表操作按钮
        using (var scope = new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("AddNewDialogue"))
            {
                dataList.arraySize++;
            }
            if (GUILayout.Button("DeletDialogue") && dataList.arraySize > 0)
            {
                dataList.arraySize--;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}