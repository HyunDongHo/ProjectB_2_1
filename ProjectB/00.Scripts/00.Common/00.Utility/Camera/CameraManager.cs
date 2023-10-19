using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scheduler;
using DG.Tweening;
using Cinemachine;  

public class CameraManager : MonoBehaviour
{
    public static Camera mainCamera = null;

    public CinemachineVirtualCamera cinemachineVirtualCamera;

    private class CurrentCameraData
    {
        public CinemachineVirtualCamera currentVirtualCamera;
        public CinemachineBasicMultiChannelPerlin currentVirtualCameraNoise;

        public void Reset()
        {
            SetNoiseAmplitude(value: 0);

            currentVirtualCamera = null;
            currentVirtualCameraNoise = null;
        }

        public void SetCinemachine(GameObject camera)
        {
            currentVirtualCamera = camera.GetComponent<CinemachineVirtualCamera>();

            if(currentVirtualCamera != null)
                currentVirtualCameraNoise = currentVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public void SetNoiseAmplitude(float value)
        {
            if (currentVirtualCamera != null && currentVirtualCameraNoise != null)
            {
                currentVirtualCameraNoise.m_AmplitudeGain = value;
            }
        }
    }
    private CurrentCameraData currentCameraData = new CurrentCameraData();

    private const string UNTAGGED = "Untagged";
    private const string MAINCAMERA = "MainCamera";

    public CameraObjectsChanger objectsChanger;

    private void Awake()
    {
        mainCamera = Camera.main;

        SetCamera(objectsChanger.objectTag);
    }

    public void SetCamera(CameraChangeTag cameraTag)
    {
        GameObject[] cameras = objectsChanger.ChangeModel(cameraTag);

        for (int i = 0; i < cameras.Length; i++)
        {
            if (i == DefineManager.OBJECT_CHANGER_SELECTED)
            {
                currentCameraData.SetNoiseAmplitude(value: 0);

                currentCameraData.SetCinemachine(cameras[i]);
            }
        }
    }

    public void StartShakeAllCamera_Position(float power, float time)
    {
        Timer.instance.TimerStart(new TimerBuffer(time),
            OnFrame: () =>
            {
                currentCameraData.SetNoiseAmplitude(power);
            },
            OnComplete: () =>
            {
                currentCameraData.SetNoiseAmplitude(0);
            });
    }

    public void SetNowPlayer(PlayerControl player)
    {
        cinemachineVirtualCamera.Follow = player.transform;
        cinemachineVirtualCamera.LookAt = player.transform;
    }
}
