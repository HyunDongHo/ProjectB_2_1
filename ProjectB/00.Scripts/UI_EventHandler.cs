using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour,IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    /* # UI �ڵ�ȭ #3*/
    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnPointerDownHandler = null;
    public Action<PointerEventData> OnPointerUpHandler = null;



    /* # UI �ڵ�ȭ #4*/
    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClickHandler != null) // ���� ��û ������ 
        {
            OnClickHandler.Invoke(eventData); // �������� Invoke�� event �ѷ��ֱ�  

        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (OnPointerDownHandler != null) // ���� ��û ������ 
        {
            //Debug.Log("OnPointerDown");
            OnPointerDownHandler.Invoke(eventData); // �������� Invoke�� event �ѷ��ֱ�  

        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (OnPointerUpHandler != null) // ���� ��û ������ 
        {
            //Debug.Log("OnPointerUp");
            OnPointerUpHandler.Invoke(eventData); // �������� Invoke�� event �ѷ��ֱ�  
        }
    }
}
