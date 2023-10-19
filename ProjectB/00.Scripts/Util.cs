using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util 
{
    /* # UI 자동화 #2*/
    // 게임오브젝트 전용의 FindChild 함수 구현 
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;
        return transform.gameObject;
    }
    public static T FindChild<T>(GameObject go, string name = null , bool recursive = false) where T: UnityEngine.Object
    {
        if (go == null) // 최상위 객체가 null아라면 return null
            return null;

        if (recursive == false) // 직속 자식만 찾는 부분 
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
        else // 재귀적으로 착는 버전 (자식의 자식을 찾는 부분)
        {
            foreach(T component in go.GetComponentsInChildren<T>()) 
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }
        return null;
    }


    /* # UI 자동화 #4*/
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>(); // T 타입의 컴포넌트를 얻어옴 
        if (component == null) // 컴포넌트가 없다면 
            component = go.AddComponent<T>(); // 추가하고 
        return component; // component 반환 
    }

}
