using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene : UI_Base
{
    /* # 강의 : UI Manager #2*/
    // 가상함수로 만들어줌 
    public override void init()
    {
        //Managers.UI.SetCanvas(gameObject, false);   
        Canvas canvas = Util.GetOrAddComponent<Canvas>(gameObject);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay; // 환경설정 부분 
        canvas.overrideSorting = true; // canvas안에 canvas가 중첩해서 있을 때 부모가 어떤 값을 가지던 나는 내 sorting order를 가질 것이다.

        //if (sort) // sorting 을 요청하면 
        //{
        //    canvas.sortingOrder = _order;
        //    _order++;
        //}
        //else
        //{
        //    canvas.sortingOrder = 0;
        //}
    }
}
