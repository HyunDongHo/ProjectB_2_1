using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateResourceManager : Singleton<CreateResourceManager>
{
    public CreatedResource CreateResource(GameObject creator, string resourceName, Vector3? position = null, Quaternion? rotation = null)
    {
        CreateResourceData createResourceData = ResourceManager.instance.Load<CreateResourceData>(resourceName, isCopy: true);

        return SetCreateResource(creator, createResourceData, position, rotation);
    }

    public CreatedResource CreateResource(GameObject creator, CreateResourceData createResourceData, Vector3? position = null, Quaternion? rotation = null)
    {
        return SetCreateResource(creator, createResourceData, position, rotation);
    }

    private CreatedResource SetCreateResource(GameObject creator, CreateResourceData createResourceData, Vector3? position, Quaternion? rotation)
    {
        CreatedResource createdResource = ObjectPoolManager.instance.CreateObject(createResourceData.createdObject,
                                                                 position ?? createResourceData.createdObject.transform.position,
                                                                 rotation ?? createResourceData.createdObject.transform.rotation)
                                                                 .GetComponent<CreatedResource>();

        if (createdResource == null) Debug.LogError($"[CreateResourceManager] {createResourceData.createdObject.name} has not 'CreatedResourceManager' Component");

        SetCreatedResourceData(creator, createResourceData, createdResource);

        return createdResource;
    }

    private void SetCreatedResourceData(GameObject creator, CreateResourceData createResourceData, CreatedResource createdResource)
    {
        createdResource.Init(creator, createResourceData);
    }
}
