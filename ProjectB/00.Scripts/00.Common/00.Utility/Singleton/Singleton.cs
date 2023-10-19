using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;
    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject singletonObject = FindObjectOfType<T>()?.gameObject ?? new GameObject(typeof(T).FullName);

                if (singletonObject.GetComponent<T>())
                {
                    _instance = singletonObject.GetComponent<T>();
                }
                else
                {
                    _instance = singletonObject.AddComponent<T>();
                }

                DontDestroyOnLoad(singletonObject);
            }

            return _instance;
        }
    }
    void OnApplicationQuit()
    {
        // 게임 종료시 객체 제거
        _instance = null;
        Destroy(gameObject);
    }

    public void Init()
    {
        // Init을 만든 이유는 처음 SettingScene에서 생성이 되도록 하기위해 호출함. 
        // 다른 Scene에서 작동해서 생성되도 되지만 SettingScene의 의미를 잃게 만들기 때문에 제작.

        // Singleton_Create를 만든 이유는 Singleton_DontDestoryOnLoad를 사용해 SettingScene을 거치지 않아도 작동되게 하기 위함.
    }
}
