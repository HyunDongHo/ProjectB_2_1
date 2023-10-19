using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SafeArea : MonoBehaviour
{
    protected virtual void Awake()
    {
        AddEvent();
    }

    private void Start()
    {
        SafeAreaObserver.instance.UpdateSafeArea();
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }

    private void AddEvent()
    {
        SafeAreaObserver.instance.OnSafeAreaChaged += HandleOnSafeAreaChaged;
    }

    private void RemoveEvent()
    {
        SafeAreaObserver.instance.OnSafeAreaChaged -= HandleOnSafeAreaChaged;
    }

    protected abstract void HandleOnSafeAreaChaged(Rect rect);
}
