using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NicknameCamera : MonoBehaviour
{
    public Transform playerLook_Min;
    public Transform playerLook_Max;

    private float playerLook_Amount = 0.0f;
    public NicknameCameraState nicknameCameraState { get; set; }
    void Start()
    {
        nicknameCameraState = NicknameCameraState.Normal;
        transform.DOMove(playerLook_Max.position, 0.25f);
    }

    public void PlayerLookLerp(float amount)
    {
        playerLook_Amount += amount;
        playerLook_Amount = Mathf.Clamp(playerLook_Amount, 0.0f, 1.0f);

        transform.position = Vector3.Lerp(playerLook_Min.position, playerLook_Max.position, playerLook_Amount);
    }

    public void ZoomIn()
    {
        if (nicknameCameraState == NicknameCameraState.Zoom)
            return;

        nicknameCameraState = NicknameCameraState.Zoom;

        float returnTime = 0.25f;

        Sequence sequence = DOTween.Sequence();

        sequence.Join(transform.DOMove(playerLook_Min.position, returnTime));
        sequence.Join(transform.DORotate(playerLook_Min.eulerAngles, returnTime));
      //  sequence.Join(playerControl.transform.DOLocalRotate(new Vector3(0, 180, 0), returnTime));

        sequence.OnComplete(() =>
        {
            InputManager.instance.SetIsAvaliableInput(true);
            
            //(playerControl as PlayerControl_Lobby).EndLobby(OnCompleteLobby: () =>
            //{
            //    FindObjectOfType<EventSystem>().enabled = true;
            //    InputManager.instance.SetIsAvaliableInput(true);

            //    OnCompleteLobbyScene?.Invoke();
            //});
        });
    }
    public void ZoomOut()
    {
        if (nicknameCameraState == NicknameCameraState.Normal)
            return;

        nicknameCameraState = NicknameCameraState.Normal;

        float returnTime = 0.25f;

        Sequence sequence = DOTween.Sequence();

        sequence.Join(transform.DOMove(playerLook_Max.position, returnTime));
        sequence.Join(transform.DORotate(playerLook_Max.eulerAngles, returnTime));
        //  sequence.Join(playerControl.transform.DOLocalRotate(new Vector3(0, 180, 0), returnTime));

        sequence.OnComplete(() =>
        {
            InputManager.instance.SetIsAvaliableInput(true);
            //(playerControl as PlayerControl_Lobby).EndLobby(OnCompleteLobby: () =>
            //{
            //    FindObjectOfType<EventSystem>().enabled = true;
            //    InputManager.instance.SetIsAvaliableInput(true);

            //    OnCompleteLobbyScene?.Invoke();
            //});
        });
    }

}
