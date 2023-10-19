using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Scheduler;

public class SubCamera : MonoBehaviour
{
    [System.Serializable]
    public class DirectCamera
    {
        public Camera subCamera;
        public AnimationControl animationControl;

        private class SaveTransform
        {
            public Vector3 previousPosition;
            public Quaternion previousRotation;
            public float previousFieldView;
        }
        private SaveTransform saveTransform = new SaveTransform();

        public void SaveCurrentSubCameraValue()
        {
            saveTransform.previousPosition = subCamera.gameObject.transform.position;
            saveTransform.previousRotation = subCamera.gameObject.transform.rotation;
            saveTransform.previousFieldView = subCamera.fieldOfView;
        }

        public void SetCurrentSubCameraValue()
        {
            subCamera.gameObject.transform.position = saveTransform.previousPosition;
            subCamera.gameObject.transform.rotation = saveTransform.previousRotation;
            subCamera.fieldOfView = saveTransform.previousFieldView;
        }
    }
    public DirectCamera directCamera;

    private TimerBuffer buffer = new TimerBuffer(0);

    private void Awake()
    {
        directCamera.SaveCurrentSubCameraValue();
    }

    public void PlayCameraAnimation(string animation, float returnTime, bool isReturnToMainCameraAfterEnd = true)
    {
        buffer.time = returnTime;

        StopPlayCameraAnimation();
        directCamera.animationControl.PlayAnimation(animation,
            OnAnimationEnd: () =>
            {
                if (isReturnToMainCameraAfterEnd == true)
                {
                    directCamera.SaveCurrentSubCameraValue();

                    directCamera.subCamera.gameObject.transform.DORotate(CameraManager.mainCamera.transform.eulerAngles, returnTime);
                    directCamera.subCamera.gameObject.transform.DOMove(CameraManager.mainCamera.transform.position, returnTime);
                    directCamera.subCamera.DOFieldOfView(CameraManager.mainCamera.fieldOfView, returnTime);

                    Timer.instance.TimerStart(buffer,
                        OnComplete: () =>
                        {
                            StopPlayCameraAnimation();
                        });
                }
            });

        directCamera.subCamera.gameObject.SetActive(true);
    }

    public void StopPlayCameraAnimation()
    {
        directCamera.subCamera.gameObject.SetActive(false);

        Timer.instance.TimerStop(buffer);

        directCamera.subCamera.gameObject.transform.DOKill();
        directCamera.animationControl.ResetAnimationState();

        directCamera.SetCurrentSubCameraValue();
        directCamera.SaveCurrentSubCameraValue();
    }
}
