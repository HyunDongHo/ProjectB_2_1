using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private UIManagers[] uiManagers;

    protected List<UIManagers> disableControlUIManagers = new List<UIManagers>();

    public T GetUIManager<T>() where T : UIManagers
    {
        foreach (UIManagers uiManager in uiManagers)
        {
            if (uiManager is T)
                return (T)uiManager;
        }

     //   Debug.LogError($"[CanvasManager] {typeof(T)} 의 형식으로 변경이 실패하였습니다.");

        return null;
    }

    public bool IsAvaliableConvertUIManager<T>() where T : UIManagers
    {
        foreach (UIManagers uiManager in uiManagers)
        {
            if (uiManager is T)
                return true;
        }

        return false;
    }

    public void SetAllUIManagersCanvasActive(bool isActive)
    {
        foreach (UIManagers uiManager in uiManagers)
        {
            bool convertIsActive = isActive;
            foreach (UIManagers disableControlUIManager in disableControlUIManagers)
            {
                if(uiManager == disableControlUIManager)
                {
                    convertIsActive = false;
                    break;
                }
            }
            uiManager.SetCanvasGroupActive(convertIsActive);
        }
    }
}
