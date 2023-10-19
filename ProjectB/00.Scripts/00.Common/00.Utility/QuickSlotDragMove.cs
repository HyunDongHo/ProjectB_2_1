using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class QuickSlotDragMove : MonoBehaviour,IPointerDownHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler
{
    public Action<bool> OnChanged;
    public Action OnSelectedItem;

    private Vector3 originPosition = Vector3.zero;
    private Vector3 originParentPosition = Vector3.zero;

    public DragOnOff dragOnOff = null;

    public Image Highlight;
    public Button Release;

    public float rightMax = 20;
    public float leftMax = -20;
    public int SlotNum = 0;

    private bool isSelected = false;

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
        Horizontal
    }

    public bool startReset = true;
    public bool isOn = true;
    public bool isEquipping = false;
    public bool isPointDown = false;

    [Space]

    private Vector2 pointerDownPos = Vector2.zero;
    private Vector2 pointerUpPos = Vector2.zero;
    private Vector2 pointerClickPos = Vector2.zero;

    [Space]
    public RectTransform parent;
    //public RectTransform LeftSlotTransform;
    //public RectTransform RightSlotTransform;

    private void Awake()
    {
        originPosition = transform.localPosition;
        originParentPosition = parent.transform.localPosition;
        dragOnOff = GetComponent<DragOnOff>();

      

        Release.onClick.AddListener(() =>
        {
            ReleaseSlotItem();
        });

        OnSelectedItem = () =>
        {
         
        };
    }

    private void Start()
    {
        if (startReset)
            SetState(isOn);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {        
        //if (isEquipping)
        //{
        //    return;
        //}

        pointerDownPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //if (isEquipping)
        //{
        //    return;
        //}

        pointerUpPos = eventData.position;
        float diffPoint = pointerDownPos.x - pointerUpPos.x;

        if (diffPoint >= 50 || diffPoint <= -50)
            dragOnOff.isChangePossible = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //if (isEquipping)
        //{
        //    return;
        //}

        pointerUpPos = eventData.position;
        float diffPoint = pointerDownPos.x - pointerUpPos.x;

        if(diffPoint >= rightMax)
        {
            SetState(true);
        }
        else if(diffPoint <= leftMax)
        {
            SetState(false);
        }

        pointerDownPos = Vector3.zero;
        pointerUpPos = Vector3.zero;
    }

    public void SetState(bool isRight)
    {
        this.isOn = isRight;

        if (isRight == true)
        {
            parent.DOLocalMoveX(-513, 0.2f);
        }
        else if (isRight == false)
        {
            parent.DOLocalMoveX(0, 0.2f);
        }

        OnChanged?.Invoke(isOn);
    }

    public bool GetState()
    {
        return isOn;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //if (isEquipping)
        //{
        //    OnSelected?.Invoke(consumableUI);
        //}
        pointerClickPos = eventData.position;
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if((pointerClickPos - eventData.position).magnitude > 3)
                return;

        pointerClickPos = Vector2.zero;

        if (isEquipping)
        {
        }
        else
        {
          
        }
    }

    public void ReleaseSlotItem()
    {        
       
    }

    public void SelectQuickSlotItem()
    {
    }

    public void UnSelectSlot()
    {
     
    }

    public void UseQuickSlotItem()
    {

    }
}
