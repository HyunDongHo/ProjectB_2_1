using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagers : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    //public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    //{
    //    if (string.IsNullOrEmpty(name))
    //        name = typeof(T).Name;

    //    GameObject go = Instantiate<GameObject>(gameObject);

    //    if (parent != null)
    //        go.transform.SetParent(parent); // 부모 지정 

    //    return Util.GetOrAddComponent<T>(go);
    //}
    public void SetCanvasGroupActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void SetCanvasGroupAlpha(float alpha)
    {
        canvasGroup.alpha = alpha;
    }

    public void ShowUI()
    {
        SetCanvasGroupActive(true);
    }

    public void HideUI()
    {
        SetCanvasGroupActive(false);
    }

    public virtual void SetText()
    {

    }

    public virtual void RefreshUI()
    {

    }
}
