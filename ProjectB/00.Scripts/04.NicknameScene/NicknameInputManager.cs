using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Scheduler;

public class NicknameInputManager : MonoBehaviour
{
    public GameObject nicknameModel;

    private Vector3 nextMovePos = Vector3.zero;
    private Vector3 previousMovePos = Vector3.zero;

    private TimerBuffer modelRotationBuffer = new TimerBuffer(0.25f);
    public float playerRotateSpeed = 0.5f;
    public float playerZoomSpeed = 0.1f;

    private int currentTouchCount = -1;

    private void Update()
    {
       PlayerInput();    
    }

    private void PlayerInput()
    {
        bool isClickedButton = InputManager.instance.GetMouseButton(0, isCheckOverlapCanvas: true);
        int touchCount = Input.touchCount;

#if UNITY_EDITOR
        touchCount = isClickedButton ? 1 : 0;
#endif

        if (currentTouchCount != -1)
        {
            if (currentTouchCount != touchCount)
            {
                ChangedTouchCount(currentTouchCount);
                currentTouchCount = -1;
            }
        }
        else if (touchCount > 0)
        {
            currentTouchCount = touchCount;
        }

        if (isClickedButton)
        {
            if (touchCount <= 1)
            {
                if (previousMovePos == Vector3.zero)
                {
                    previousMovePos = Input.mousePosition;
                }
                else
                {
                    nextMovePos = Input.mousePosition;

                    Vector3 rotateDir = (previousMovePos - nextMovePos);

                    Vector3 modelEulerAngles = nicknameModel.transform.eulerAngles + new Vector3(0, rotateDir.x * playerRotateSpeed, 0);

                    if (rotateDir.normalized.x > 0 || rotateDir.normalized.x < 0)
                    {
                        float distance = Vector3.Distance(nicknameModel.transform.eulerAngles, modelEulerAngles);
                        Timer.instance.TimerStart(modelRotationBuffer,
                            OnFrame: () =>
                            {
                                nicknameModel.transform.Rotate(new Vector3(0, rotateDir.normalized.x, 0) * distance * Time.deltaTime / modelRotationBuffer.time);
                            },
                            OnComplete: () =>
                            {
                                nicknameModel.transform.eulerAngles = modelEulerAngles;
                            });
                    }

                    previousMovePos = Vector3.zero;
                }
            }
            else if (touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                float deltaMagnitudeDiff = -(prevTouchDeltaMag - touchDeltaMag);

                CameraManager.mainCamera.GetComponent<NicknameCamera>().PlayerLookLerp(deltaMagnitudeDiff * Time.deltaTime * playerZoomSpeed);
            }
        }
    }

    private void ChangedTouchCount(int previousTouchCount)
    {
        if (previousTouchCount <= 1)
        {
            previousMovePos = Vector3.zero;
        }
        else if (previousTouchCount == 2)
        {

        }
    }
}
