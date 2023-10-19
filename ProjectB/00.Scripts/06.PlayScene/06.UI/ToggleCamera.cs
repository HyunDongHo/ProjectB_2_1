using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleCamera : MonoBehaviour
{
    public Button cameraButton;
    private bool isMainCamera = true;

    private void Start()
    {
        cameraButton.onClick.AddListener(
            () =>
            {
                isMainCamera = !isMainCamera;
                StageManager.instance.cameraManager.SetCamera(isMainCamera ? CameraChangeTag.Main : CameraChangeTag.Sub_1);
            });
    }
}
