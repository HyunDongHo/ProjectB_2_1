using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectDetail
{
    public string objectTag;
    public GameObject objectModel;
}

public class ObjectsChanger<T> : MonoBehaviour where T: System.Enum
{ 
    public ObjectDetail[] objectDetails;

    public T objectTag;

    /// <summary>
    /// 0번째는 바뀌는 모델 Index / 1번째부터는 바뀌지않는 모델들 Index
    /// </summary>
    public GameObject[] ChangeModel(T objectTag)
    {
        GameObject[] models = new GameObject[objectDetails.Length];

        this.objectTag = objectTag;

        int currentModelIndex = 1;
        for (int i = 0; i < objectDetails.Length; i++)
        {
            bool isMatch = this.objectTag.ToString() == objectDetails[i].objectTag;

            objectDetails[i].objectModel.SetActive(isMatch);

            if (isMatch)
                models[0] = objectDetails[i].objectModel;
            else
                models[currentModelIndex++] = objectDetails[i].objectModel;
        }

        return models;
    }
}
