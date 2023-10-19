using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util 
{
    /* # UI �ڵ�ȭ #2*/
    // ���ӿ�����Ʈ ������ FindChild �Լ� ���� 
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;
        return transform.gameObject;
    }
    public static T FindChild<T>(GameObject go, string name = null , bool recursive = false) where T: UnityEngine.Object
    {
        if (go == null) // �ֻ��� ��ü�� null�ƶ�� return null
            return null;

        if (recursive == false) // ���� �ڽĸ� ã�� �κ� 
        {
            for(int i=0; i<go.transform.childCount; i++)
            {
                Transform transform =  go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)  
                        return component;
                }  
            }
            
        }
        else // ��������� ���� ���� (�ڽ��� �ڽ��� ã�� �κ�)
        {
            foreach(T component in go.GetComponentsInChildren<T>()) 
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }
        return null;
    }


    /* # UI �ڵ�ȭ #4*/
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>(); // T Ÿ���� ������Ʈ�� ���� 
        if (component == null) // ������Ʈ�� ���ٸ� 
            component = go.AddComponent<T>(); // �߰��ϰ� 
        return component; // component ��ȯ 
    }

}
