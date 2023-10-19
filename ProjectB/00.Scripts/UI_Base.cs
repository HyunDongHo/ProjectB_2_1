using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{
    /* ���� : UI �ڵ�ȭ #1*/
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    /* # ���� : �κ��丮 �ǽ� #2*/
    public abstract void init();

    private void Start()
    {
        init();        
    }
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type); // type �� ���� Texts��� PointText�� ScoreText�� 
                                              // string �迭�� ����.      
                  
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            /* # UI�ڵ�ȭ #2*/
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true);
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true); // drag & drop ������� �ʰ� �����ϴ� ��
                                                                            // �ڽĵ� ã�� (utils ���Ͽ� util ��ũ��Ʈ�� FindChild �Լ��� ���ǵǾ� ����)
            if (objects[i] == null)
                Debug.Log($"Fail to Bind({names[i]})");
        }
    }

    /* # UI �ڵ�ȭ #2 */
    // getter
    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    protected Text GetText(int idx)
    {
        return Get<Text>(idx);
    }
    protected Button GetButton(int idx)
    {
        return Get<Button>(idx);
    }
    protected Image GetImage(int idx)
    {
        return Get<Image>(idx);
    }
    protected GameObject GetObject(int idx) 
    { 
        return Get<GameObject>(idx); 
    }

    /* # UI �ڵ�ȭ #4*/
    public static void AddUIEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        //UI_EventHandler evt = go.GetComponent<UI_EventHandler>(); // ����(�巡�� ���)���� �ߴٸ�  GetComponent�� UI_EventHandler ������ 
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go); // �ڵ����� UI_EventHandler ���� �� ������       

        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action; //���� ��û�ϰ�    
                break;
            case Define.UIEvent.PointerDown:
                evt.OnPointerDownHandler -= action;
                evt.OnPointerDownHandler += action; //���� ��û�ϰ� 
                break;
            case Define.UIEvent.PointerUp:
                evt.OnPointerUpHandler -= action;
                evt.OnPointerUpHandler += action; //���� ��û�ϰ�   
                break;   
        }

        //evt.OnDragHandler += ((PointerEventData data) => { evt.gameObject.transform.position = data.position; }); // ���� ��û �� ���ٷ� ���� 
    }


}
