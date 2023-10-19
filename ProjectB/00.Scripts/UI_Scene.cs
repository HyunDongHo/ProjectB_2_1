using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene : UI_Base
{
    /* # ���� : UI Manager #2*/
    // �����Լ��� ������� 
    public override void init()
    {
        //Managers.UI.SetCanvas(gameObject, false);   
        Canvas canvas = Util.GetOrAddComponent<Canvas>(gameObject);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay; // ȯ�漳�� �κ� 
        canvas.overrideSorting = true; // canvas�ȿ� canvas�� ��ø�ؼ� ���� �� �θ� � ���� ������ ���� �� sorting order�� ���� ���̴�.

        //if (sort) // sorting �� ��û�ϸ� 
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
