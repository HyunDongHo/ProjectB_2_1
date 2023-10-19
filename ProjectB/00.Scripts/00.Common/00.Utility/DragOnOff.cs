using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragOnOff : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Action<bool> OnChanged;
    public Action OnCheckCondition;

    private Vector3 originPosition = Vector3.zero;

    public float onMax = 0;
    public float offMax = 0;

    [Space]

    public Direction onDirection = Direction.Plus;
    public enum Direction
    {
        Plus = 1,
        Minus = -1,
    }

    public Axis axis = Axis.Horizontal;
    public enum Axis
    {
        Horizontal,
        Vertical,
    }

    public bool startReset = true;
    public bool isOn = true;
    public bool isChangePossible = true;

    [Space]

    private Vector2 pointerDownPos = Vector2.zero;
    private Vector2 pointerUpPos = Vector2.zero;

    private void Awake()
    {
        originPosition = transform.localPosition;
        SetStateNoEvent(false);
    }

    private void Start()
    {
        if (startReset)
            SetState(isOn);
    }

    private Vector3 prevPosition = Vector3.zero;
    public void OnBeginDrag(PointerEventData eventData)
    {
        prevPosition = transform.localPosition;
        pointerDownPos = eventData.position;
        isChangePossible = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 convertDragPosition = transform.localPosition;

        switch (axis)
        {
            case Axis.Horizontal:
                convertDragPosition = new Vector3(originPosition.x + (eventData.position.x - pointerDownPos.x), transform.localPosition.y, transform.localPosition.z);               
                switch (onDirection)
                {
                    case Direction.Plus:
                        if (convertDragPosition.x > originPosition.x + onMax)
                        {
                            convertDragPosition.x = originPosition.x + onMax;
                        }
                        else if (convertDragPosition.x < originPosition.x + offMax)
                        {
                            convertDragPosition.x = originPosition.x + offMax;
                        }
                        break;
                    case Direction.Minus:
                        if (convertDragPosition.x > originPosition.x + offMax)
                        {
                            convertDragPosition.x = originPosition.x + offMax;
                        }
                        else if (convertDragPosition.x < originPosition.x + onMax)
                        {
                            convertDragPosition.x = originPosition.x + onMax;
                        }
                        break;
                }
                break;
            case Axis.Vertical:
                convertDragPosition = new Vector3(transform.localPosition.x, originPosition.y + (eventData.position.y - pointerDownPos.y), transform.localPosition.z); 
                float xMove = Mathf.Abs(pointerDownPos.x - eventData.position.x);
                if (xMove >= 30)
                {
                    transform.localPosition = prevPosition;
                    return;
                }
                switch (onDirection)
                {
                    case Direction.Plus:
                        if (convertDragPosition.y > originPosition.y + onMax)
                        {
                            convertDragPosition.y = originPosition.y + onMax;
                        }
                        else if (convertDragPosition.y < originPosition.y + offMax)
                        {
                            convertDragPosition.y = originPosition.y + offMax;
                        }
                        break;
                    case Direction.Minus:
                        if (convertDragPosition.y > originPosition.y + offMax)
                        {
                            convertDragPosition.y = originPosition.y + offMax;
                        }
                        else if (convertDragPosition.y < originPosition.y + onMax)
                        {
                            convertDragPosition.y = originPosition.y + onMax;
                        }
                        break;
                }
                break;
        }
        transform.localPosition = convertDragPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnCheckCondition?.Invoke();

        if (isChangePossible == false)
        {
            SetState(isOn);
            return; 
        }

        pointerUpPos = eventData.position;

        Vector2 dir = -(pointerDownPos - pointerUpPos).normalized * (int)onDirection;

        switch (axis)
        {
            case Axis.Horizontal:
                //     if (dir.x != 0)
                //    if(dir.x >= onMax || dir.x <= offMax)
                if (Mathf.Abs(pointerDownPos.y - pointerUpPos.y) < 20)
                {
                    SetState(dir.x > 0);
                }
                else
                    SetStateNoEvent(isOn);
                break;
            case Axis.Vertical:
                //   if (dir.y != 0)
                //     if (dir.y >= onMax || dir.y <= offMax)
                if (Mathf.Abs(pointerDownPos.x - pointerUpPos.x) < 20)
                {
                    SetState(dir.y > 0);
                }
                else
                    SetStateNoEvent(isOn);
                break;
        }

        pointerDownPos = Vector3.zero;
        pointerUpPos = Vector3.zero;
    }

    public void SetState(bool isOn)
    {
        this.isOn = isOn;

        Vector3 convertPosition = Vector3.zero;

        switch (axis)
        {
            case Axis.Horizontal:
                convertPosition = originPosition + Vector3.right * (isOn ? onMax : offMax);
                break;
            case Axis.Vertical:
                convertPosition = originPosition + Vector3.up * (isOn ? onMax : offMax);
                break;
        }

        convertPosition.z = transform.localPosition.z;
        transform.localPosition = convertPosition;


        OnChanged?.Invoke(isOn);
    }

    public void SetStateNoEvent(bool isOn)
    {
        this.isOn = isOn;

        Vector3 convertPosition = Vector3.zero;

        switch (axis)
        {
            case Axis.Horizontal:
                convertPosition = originPosition + Vector3.right * (isOn ? onMax : offMax);
                break;
            case Axis.Vertical:
                convertPosition = originPosition + Vector3.up * (isOn ? onMax : offMax);
                break;
        }

        convertPosition.z = transform.localPosition.z;
        transform.localPosition = convertPosition;

    }

    public bool GetState()
    {
        return isOn;
    }
}
