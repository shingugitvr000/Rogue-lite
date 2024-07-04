using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ActionData))]
public class ActionDataEditor : Editor                  //�����Ϳ� �����Ѵ�. 
{
    public override void OnInspectorGUI()                   //���� �ν����� GUI ������ �������� Ŀ���� �� ���� �ڵ� 
    {
        base.OnInspectorGUI();                              //���� ������ �����Ѵ�. 
        ActionData actionData = (ActionData)target; 

        EditorGUI.BeginChangeCheck();                       //��Ʈ���ͺ� ������Ʈ�� ��ȭ�� ���� �� ����

        var newClip = (AnimationClip)EditorGUILayout.ObjectField("Event Clip", actionData.EventClip, typeof(AnimationClip), false);

        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(actionData, "Changed Animation Clip");
            actionData.EventClip = newClip;                             //���� ������ mecanimname �� WaitTime ���� 
            EditorUtility.SetDirty(actionData);
        }

    }
}
