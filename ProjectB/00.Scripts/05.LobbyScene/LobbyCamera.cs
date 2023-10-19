using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCamera : MonoBehaviour
{
    public Transform playerLook_Min;
    public Transform playerLook_Max;

    Animation anim;
    public Action AnimEndAction = null;

    private float playerLook_Amount = 0.0f;
    public LobbyState NowLobbyState { get; set; }
    void Start()
    {
        anim = GetComponent<Animation>();
        NowLobbyState = LobbyState.Center;
    }

    public void PlayerLookLerp(float amount)
    {
        playerLook_Amount += amount;
        playerLook_Amount = Mathf.Clamp(playerLook_Amount, 0.0f, 1.0f);

        transform.position = Vector3.Lerp(playerLook_Min.position, playerLook_Max.position, playerLook_Amount);
    }

    public void AnimPlay(LobbyState nextState, Action endAction = null)
    {
        if (NowLobbyState == nextState)
        {
            endAction?.Invoke();
            return;
        }

        if (endAction != null)
            AnimEndAction = endAction;

        string animName = "";

        if (NowLobbyState == LobbyState.Center)
        {
            if (nextState == LobbyState.Left)
                animName = "LobbyCenterToLeftAnimation";
            else
                animName = "LobbyCenterToRightAnimation";
        }
        else if (NowLobbyState == LobbyState.Left)
        {
            if (nextState == LobbyState.Center)
                animName = "LobbyLeftToCenterAnimation";
        }
        else if (NowLobbyState == LobbyState.Right)
        {
            if (nextState == LobbyState.Center)
                animName = "LobbyRightToCenterAnimation";
        }

        NowLobbyState = nextState;
        anim.Play(animName);
    }

    public void AnimationEnd()
    {
        AnimEndAction?.Invoke();
    }
}
