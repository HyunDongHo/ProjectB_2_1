using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scheduler;

public enum ScreenRectType
{
    Left,
    Right,
    All,
}

public class PlayerInput : MonoBehaviour
{
    private PlayerControl playerControl;

    private Vector2 previousclickedPosition = Vector2.zero;
    private Vector2 currentClickedPosition = Vector2.zero;

    private float clickTime = 0;
    private float unclickTime = 0;

    public void Init(PlayerControl playerControl)
    {
        this.playerControl = playerControl;
    }

    public void Release()
    {

    }

    private void Update()
    {

        if (InputManager.instance.GetMouseButtonDown(0, ScreenRectType.All, isCheckOverlapCanvas: true))
        {
            unclickTime = 0;
        }

        if (InputManager.instance.GetMouseButton(0, ScreenRectType.All, isCheckOverlapCanvas: true))
        {
            clickTime += Time.deltaTime;
        }
        else if (InputManager.instance.GetMouseButtonUp(0, ScreenRectType.All, isCheckOverlapCanvas: true))
        {
            Timer.instance.TimerStart(new TimerBuffer(Time.deltaTime), OnComplete: () => clickTime = 0);
        }
        else
        {
            unclickTime += Time.deltaTime;
        }
    }

    public int GetHorizontalDrag(float sensitivity = 0.05f)
    {
        int value = 0;

        if (Input.GetMouseButtonDown(0) && !InputManager.instance.IsOverlapCanvas())
        {
            previousclickedPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0) && previousclickedPosition != Vector2.zero)
        {
            currentClickedPosition = Input.mousePosition;

            Vector2 dir = currentClickedPosition - previousclickedPosition;

            dir.x /= Screen.width;

            if (dir.x > sensitivity) value = 1;
            else if (dir.x < -sensitivity) value = -1;

            currentClickedPosition = Vector2.zero;
            previousclickedPosition = Vector2.zero;
        }

        return value;
    }

    public float GetClickTime()
    {
        return clickTime;
    }

    public float GetUnclickTime()
    {
        return unclickTime;
    }
}
