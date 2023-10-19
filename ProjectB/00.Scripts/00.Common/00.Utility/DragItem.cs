using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Action OnChanged;

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

    [Space]

    private Vector2 pointerDownPos = Vector2.zero;
    private Vector2 pointerUpPos = Vector2.zero;

    private void Awake()
    {
        originPosition = transform.localPosition;
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
        pointerUpPos = eventData.position;

        Vector2 dir = -(pointerDownPos - pointerUpPos) * (int)onDirection;

        switch (axis)
        {
            case Axis.Horizontal:
                if(dir.x >= onMax || dir.x <= offMax)
                {
                    SetState(dir.x > 0);
                }
                else
                    transform.localPosition = prevPosition;
                break;
            case Axis.Vertical:
                 if (dir.y >= onMax || dir.y <= offMax)
                {
                    SetState(dir.y > 0);
                }
                else
                    transform.localPosition = prevPosition;
                break;
        }

        pointerDownPos = Vector3.zero;
        pointerUpPos = Vector3.zero;
    }

    public void SetState(bool isOn)
    {
        if (isOn != true)
        {
            transform.localPosition = prevPosition;
            return;
        }

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

        OnChanged?.Invoke();
    }
    public bool GetState()
    {
        return isOn;
    }
}
