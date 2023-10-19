using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

public class ResourceManager : Singleton<ResourceManager>
{
    private Dictionary<ObscuredString, Object> loadedResources = new Dictionary<ObscuredString, Object>();

    public T Load<T>(string path, bool isCopy = false) where T : Object
    {
        System.Type type = typeof(T);

        T loadedResource = GetResource<T>(path);

        if (loadedResource == null)
        {
            Debug.LogError($"[ResoucreManager] {path} 가 존재하지 않습니다.");
        }
        else
        {
            if (isCopy)
            {
                if (type != typeof(GameObject) && type != typeof(Sprite))
                {
                    loadedResource = Instantiate(Resources.Load<T>(path));
                };
            }
        }

        return loadedResource;
    }

    private T GetResource<T>(string path) where T : Object
    {
        if (loadedResources.ContainsKey(path))
        {
            return loadedResources[path] as T;
        }
        else
        {
            T loadedResource = Resources.Load<T>(path);
            loadedResources.Add(path, loadedResource);
            return loadedResource;
        }
    }
}
