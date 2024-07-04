using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ActionSystem/ComboAction")]
public class ComboAction : ScriptableObject
{
    public List<ActionData> actions = new List<ActionData>();
    public float comboResetTime = 2.0f;                 //�޺��� ���� �Ǵ� �ð� 

    //���� �׼����� �Ѿ �� �ִ��� üũ 
    public ActionData GetNextAciton(int currentActionIndex)
    {
        if(currentActionIndex < actions.Count - 1)
        {
            return actions[currentActionIndex + 1];
        }

        return null;
    }
}
