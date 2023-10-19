using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extensions
{
    /*====================================*/
    // Extension 클래스는 함수를 좀더 간단히 쓰기 위해 정의하는 클래스이다.
    // 사용법은 this를 사용한다.

    /*# 강의 : 코드 정리 */
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(go);
    }


    /* # UI 자동화 #4*/
    public static void AddUIEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.AddUIEvent(go, action, type);    
    }

    /* # 미니RPG Destroy#2*/
    public static bool IsVaild(this GameObject go)
    {
        return go != null && go.activeSelf;
    }
}
