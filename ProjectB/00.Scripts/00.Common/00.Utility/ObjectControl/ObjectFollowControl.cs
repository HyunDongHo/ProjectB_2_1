using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectFollowControlData
{
    public bool isRelativeOffset = false;

    [Space]

    public bool isLocalPosition = false;

    public Vector3 positionOffset = Vector3.zero;

    public bool isfirstPositionToTarget = false;
    public bool isFollowTargetPosition = false;

    [Space]

    public bool isLocalRotation = false;

    public Vector3 rotationOffset = Vector3.zero;

    public bool isfirstRotationToTarget = false;
    public bool isFollowTargetRotation = false;

    public bool isRotationLerp = false;
    [ConditionalHide("isRotationLerp", true)] public float rotationLerp = 1;
}

public class ObjectFollowControl : MonoBehaviour
{
    public Transform target;
    public ObjectFollowControlData objectFollowControlData = new ObjectFollowControlData();

    private void Awake()
    {
        Init(target, objectFollowControlData);
    }

    private void LateUpdate()
    {
        ObjectFollowControlData data = objectFollowControlData;

        if (data.isFollowTargetPosition)
            SetPosition(target.transform, data.isLocalPosition, data.isRelativeOffset, data.positionOffset);

        if (data.isFollowTargetRotation)
            SetRotation(target.transform, data.isLocalRotation, data.rotationOffset);
    }

    public void Init(Transform target, ObjectFollowControlData data)
    {
        this.target = target;

        if (data.isfirstPositionToTarget)
            SetPosition(target, data.isLocalPosition, data.isRelativeOffset, data.positionOffset);

        if (data.isfirstRotationToTarget)
            SetRotation(target, data.isLocalRotation, data.rotationOffset);
    }

    public void SetPosition(Transform followTransform, bool isLocal, bool isRelativeOffset, Vector3 offset)
    {
        Vector3 convertOffset = offset;

        if (isRelativeOffset)
            convertOffset = transform.right * offset.x + transform.up * offset.y + transform.forward * offset.z;

        if (isLocal)
        {
            transform.localPosition = convertOffset;
        }
        else
        {
            transform.position = followTransform.position + convertOffset;
        }
    }

    public void SetRotation(Transform followTransform, bool isLocal, Vector3 offset)
    {
        Vector3 convertOffset = offset;

        if (isLocal)
        {
            Quaternion changedRotation = Quaternion.Euler(convertOffset);

            if (objectFollowControlData.isRotationLerp)
                transform.localRotation = Quaternion.Lerp(transform.localRotation, changedRotation, objectFollowControlData.rotationLerp);
            else
                transform.localRotation = changedRotation;
        }
        else
        {
            Quaternion changedRotation = Quaternion.Euler(followTransform.eulerAngles + convertOffset);

            if (objectFollowControlData.isRotationLerp)
                transform.rotation = Quaternion.Lerp(transform.rotation, changedRotation, objectFollowControlData.rotationLerp);
            else
                transform.rotation = changedRotation;
        }
    }
}
