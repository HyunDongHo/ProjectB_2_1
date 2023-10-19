using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDamageManager : Singleton<ShowDamageManager>
{
    public void ShowCritical(Vector3 createPosition)
    {
        CreateResourceManager.instance.CreateResource(gameObject, "Critical", createPosition);
    }

    public void ShowMiss(Vector3 createPosition)
    {
        CreateResourceManager.instance.CreateResource(gameObject, "Miss", createPosition);
    }
}
