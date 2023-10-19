using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance = null;

    public CanvasManager canvasManager;

    [Space]

    public CameraManager cameraManager;

    public PlayerControl playerControl;

    protected virtual void Awake()
    {
        instance = this;

        PlayersControlManager.instance.SetPlayerFromServer();
        playerControl = PlayersControlManager.instance.GetNowActivePlayer();

        AddEvent();
    }

    protected virtual void Start()
    {
        OnStartFadeStart();
        FadeInOut.instance.FadeIn(DefineManager.DEFAULT_FADE_DURATION, OnStartFadeEnd);
    }

    protected virtual void OnDestroy()
    {
        RemoveEvent();
    }

    protected virtual void AddEvent()
    {

    }

    protected virtual void RemoveEvent()
    {

    }

    protected virtual void OnStartFadeStart()
    {

    }

    protected virtual void OnStartFadeEnd()
    {

    }
}
