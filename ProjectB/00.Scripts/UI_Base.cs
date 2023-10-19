using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{
    /* 강의 : UI 자동화 #1*/
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    /* # 강의 : 인벤토리 실습 #2*/
    public abstract void init();

    private void Start()
    {
        init();        
    }
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type); // type 이 만약 Texts라면 PointText와 ScoreText가 
                                              // string 배열로 들어간다.      
                  
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            /* # UI자동화 #2*/
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true);
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true); // drag & drop 사용하지 않고 매핑하는 법
                                                                            // 자식들 찾기 (utils 파일에 util 스크립트에 FindChild 함수가 정의되어 있음)
            if (objects[i] == null)
                Debug.Log($"Fail to Bind({names[i]})");
        }
    }

    /* # UI 자동화 #2 */
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

    /* # UI 자동화 #4*/
    public static void AddUIEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        //UI_EventHandler evt = go.GetComponent<UI_EventHandler>(); // 수동(드래그 드롭)으로 했다면  GetComponent로 UI_EventHandler 얻어오고 
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go); // 자동으로 UI_EventHandler 부착 및 가져옴       

        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action; //구독 신청하고    
                break;
            case Define.UIEvent.PointerDown:
                evt.OnPointerDownHandler -= action;
                evt.OnPointerDownHandler += action; //구독 신청하고 
                break;
            case Define.UIEvent.PointerUp:
                evt.OnPointerUpHandler -= action;
                evt.OnPointerUpHandler += action; //구독 신청하고   
                break;   
        }

        //evt.OnDragHandler += ((PointerEventData data) => { evt.gameObject.transform.position = data.position; }); // 구독 신청 및 람다로 정의 
    }


}
