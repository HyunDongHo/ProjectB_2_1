using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraFollow : MonoBehaviour
{
    public GameObject followTarget;
    public Vector3 relativeOffset;
    public Vector3 rotationOffset;

    public float moveDuration = 0.1f;
    public float rotateDuration = 1.0f;

    private void Start()
    {
        ResetCamera();
    }

    private void OnEnable()
    {
        ResetCamera();
    }

    private void ResetCamera()
    {
        Vector3 convertOffset = followTarget.transform.right * relativeOffset.x + followTarget.transform.up * relativeOffset.y + followTarget.transform.forward * relativeOffset.z;

        transform.position = followTarget.transform.position + convertOffset;
        transform.eulerAngles = followTarget.transform.eulerAngles + rotationOffset;
    }

    private void LateUpdate()
    {
        Vector3 positionConvertOffset = followTarget.transform.right * relativeOffset.x + followTarget.transform.up * relativeOffset.y + followTarget.transform.forward * relativeOffset.z;
        transform.position = Vector3.Lerp(transform.position, (followTarget.transform.position + positionConvertOffset), moveDuration);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(followTarget.transform.eulerAngles + rotationOffset), rotateDuration);
    }
}
