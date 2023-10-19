using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scheduler;
using DG.Tweening;

public class DamageShow : MonoBehaviour
{
    public CreatedResource createdResource;

    private Vector3 originScale;

    public float disappearTimeRatio = 1.5f;

    private void Awake()
    {
        originScale = transform.localScale;
    }

    private void OnEnable()
    {
        transform.localScale = originScale;
        Vector3 lookPosition = CameraManager.mainCamera.transform.position;

        transform.LookAt(lookPosition);

        TimerBuffer buffer = new TimerBuffer(createdResource.destroyTime);
        Timer.instance.TimerStart(buffer, 
            OnFrame: () =>
            {
                transform.LookAt(lookPosition);
                transform.forward = -transform.forward;

                if (buffer.timer >= buffer.time / disappearTimeRatio)
                {
                    transform.localScale *= 1.0f - ((buffer.timer / buffer.time) - buffer.time / disappearTimeRatio) * disappearTimeRatio;
                }
            });
    }
}
