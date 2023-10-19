using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCheck : MonoBehaviour
{
    private bool isShowFPS = false;

    public int fontSize = 10;
    public Color fontColor = new Color(0.0f, 1.0f, 0.0f);

    private float deltaTime = 0.0f;

    public void ShowFPS()
    {
        isShowFPS = true;

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    private void OnGUI()
    {
        if (isShowFPS)
        {
            Rect rect = new Rect(0, 0, Screen.width, Screen.height);

            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.UpperCenter;
            style.fontSize = fontSize;
            style.normal.textColor = fontColor;

            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);

            GUI.Label(rect, text, style);
        }
    }
}

