using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : Singleton<InputManager>
{
    private bool isAvaliableInput = true;

    public void SetIsAvaliableInput(bool isAvaliable)
    {
        isAvaliableInput = isAvaliable;
    }

    public bool GetMouseButtonDown(int button, ScreenRectType screenRectType = ScreenRectType.All, bool isCheckOverlapCanvas = false)
    {
        return Input.GetMouseButtonDown(button) &&
               CheckScreenRect(screenRectType) &&
               (isCheckOverlapCanvas ? !IsOverlapCanvas() : true) &&
               isAvaliableInput;
    }

    public bool GetMouseButton(int button, ScreenRectType screenRectType = ScreenRectType.All, bool isCheckOverlapCanvas = false)
    {
        return Input.GetMouseButton(button) &&
               CheckScreenRect(screenRectType) &&
               (isCheckOverlapCanvas ? !IsOverlapCanvas() : true) &&
               isAvaliableInput;
    }

    public bool GetMouseButtonUp(int button, ScreenRectType screenRectType = ScreenRectType.All, bool isCheckOverlapCanvas = false)
    {
        return Input.GetMouseButtonUp(button) &&
               CheckScreenRect(screenRectType) &&
               (isCheckOverlapCanvas ? !IsOverlapCanvas() : true) &&
               isAvaliableInput;
    }

    public bool IsOverlapCanvas()
    {
        if (EventSystem.current == null) return false;

        if (EventSystem.current.IsPointerOverGameObject())
            return true;

        if (Input.touchCount > 0)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                return true;
        }

        return false;
    }

    private bool CheckScreenRect(ScreenRectType screenRectType)
    {
        switch (screenRectType)
        {
            case ScreenRectType.Left:
                if (Input.mousePosition.x < Screen.width / 2)
                {
                    return true;
                }
                break;
            case ScreenRectType.Right:
                if (Input.mousePosition.x >= Screen.width / 2)
                {
                    return true;
                }
                break;
            case ScreenRectType.All:
                return true;
        }

        return false;
    }
}
