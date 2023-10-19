using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scheduler;
using DG.Tweening;

public class CreatedResource : MonoBehaviour, ObjectPoolInterface
{
    public bool isAfterTimeToDestroy = true;
    [ConditionalHide(nameof(isAfterTimeToDestroy), true)] public float destroyTime = 0.0f;

    private TimerBuffer followObjectPosition = new TimerBuffer(Mathf.Infinity);
    private TimerBuffer followObjectRotation = new TimerBuffer(Mathf.Infinity);

    private ObjectFollowControl objectFollowControl;

    public void Init(GameObject creator, CreateResourceData createResourceData)
    {
        AddObjectFollowControlComponent();

        InitCreateResourceData(creator, createResourceData);
    }

    private void AddObjectFollowControlComponent()
    {
        objectFollowControl = gameObject.GetComponent<ObjectFollowControl>();

        if (objectFollowControl == null)
            objectFollowControl = gameObject.AddComponent<ObjectFollowControl>();        
    }

    private void InitCreateResourceData(GameObject creator, CreateResourceData data)
    {
        if (data.isSetCreatorParent) transform.SetParent(creator.transform);

        objectFollowControl.Init(creator?.transform, TestCreate(data));
    }

    private ObjectFollowControlData TestCreate(CreateResourceData resourceData)
    {
        ObjectFollowControlData followControlData = new ObjectFollowControlData();

        followControlData.isRelativeOffset = resourceData.isRelativeOffset;

        followControlData.isLocalPosition = resourceData.isLocalPosition;
        followControlData.positionOffset = resourceData.positionOffset;
        followControlData.isfirstPositionToTarget = resourceData.isfirstPositionToTarget;
        followControlData.isFollowTargetPosition = resourceData.isFollowTargetPosition;

        followControlData.isLocalRotation = resourceData.isLocalRotation;
        followControlData.rotationOffset = resourceData.rotationOffset;
        followControlData.isfirstRotationToTarget = resourceData.isfirstRotationToTarget;
        followControlData.isFollowTargetRotation = resourceData.isFollowTargetRotation;

        return followControlData;
    }

    public CreatedResource MoveToPosition(float speed, Vector3 position, Action OnComplete = null)
    {
        float duration = Vector3.Distance(transform.position, position) / speed;
        transform.DOMove(position, duration).OnComplete(() => OnComplete?.Invoke());

        return this;
    }

    public void DestroyCreatedResource(float destroyTime = 0.0f)
    {
        Timer.instance.TimerStop(followObjectPosition);
        Timer.instance.TimerStop(followObjectRotation);

        ObjectPoolManager.instance.RemoveObject(gameObject, destroyTime);
    }

    public void Respawned()
    {
        if (isAfterTimeToDestroy)
            DestroyCreatedResource(destroyTime);
    }
}
