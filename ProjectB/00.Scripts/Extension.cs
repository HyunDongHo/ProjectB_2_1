using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extensions
{
    /*====================================*/
    // Extension Ŭ������ �Լ��� ���� ������ ���� ���� �����ϴ� Ŭ�����̴�.
    // ������ this�� ����Ѵ�.

    /*# ���� : �ڵ� ���� */
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(go);
    }


    /* # UI �ڵ�ȭ #4*/
    public static void AddUIEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.AddUIEvent(go, action, type);    
    }

    /* # �̴�RPG Destroy#2*/
    public static bool IsVaild(this GameObject go)
    {
        return go != null && go.activeSelf;
    }
}
