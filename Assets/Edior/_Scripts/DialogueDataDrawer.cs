using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//半山腰太挤，你总得去山顶看看//
[CustomPropertyDrawer(typeof(DialogueData))]
public class DialogueDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 开始绘制属性
        EditorGUI.BeginProperty(position, label, property);

        // 获取所有需要控制的字段
        SerializedProperty isDialogueChoice = property.FindPropertyRelative("isDialogueChoice");
        SerializedProperty speakerName = property.FindPropertyRelative("speakerName");
        SerializedProperty speakerCharacter = property.FindPropertyRelative("speakerCharacter");
        SerializedProperty dialogueContent = property.FindPropertyRelative("dialogueContent");
        SerializedProperty choiceList = property.FindPropertyRelative("Choice");

        // 绘制 "isDialogueChoice" 开关
        float yPos = position.y;
        float lineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        Rect toggleRect = new Rect(position.x, yPos, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(toggleRect, isDialogueChoice);
        yPos += lineHeight;

        // 根据开关状态绘制不同字段
        if (isDialogueChoice.boolValue)
        {
            // 显示选项列表（当 isDialogueChoice 为 true 时）
            Rect choiceRect = new Rect(position.x, yPos, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(choiceRect, choiceList, new GUIContent("ChoiceList"), true);
        }
        else
        {
            // 显示常规对话字段（当 isDialogueChoice 为 false 时）
            Rect nameRect = new Rect(position.x, yPos, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(nameRect, speakerName);
            yPos += lineHeight;

            Rect characterRect = new Rect(position.x, yPos, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(characterRect, speakerCharacter);
            yPos += lineHeight;

            Rect contentRect = new Rect(position.x, yPos, position.width, EditorGUIUtility.singleLineHeight * 3); // 多行文本
            EditorGUI.PropertyField(contentRect, dialogueContent, true);
        }

        EditorGUI.EndProperty();
    }

    // 计算属性所需的高度（根据显示的字段动态调整）
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty isDialogueChoice = property.FindPropertyRelative("isDialogueChoice");
        SerializedProperty choiceList = property.FindPropertyRelative("Choice");
        SerializedProperty dialogueContent = property.FindPropertyRelative("dialogueContent");

        float baseHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // 开关的高度

        if (isDialogueChoice.boolValue)
        {
            // 选项列表的高度（包括折叠/展开的高度）
            return baseHeight + EditorGUI.GetPropertyHeight(choiceList, true);
        }
        else
        {
            // 常规字段的高度：3行（名称、头像、内容）+ 间距
            return baseHeight +
                   (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 2 +
                   EditorGUI.GetPropertyHeight(dialogueContent, true);
        }
    }
}
