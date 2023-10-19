using UnityEngine;
using System;

//Original version of the ConditionalHideAttribute created by Brecht Lecluyse (www.brechtos.com)
//Modified by: -

// 상태 값에 따라서 변수가 비활성화 될 수 있도록 하는 기능. (Inspector에서 헷갈리지 않고 작업 할 수 있도록 도움 줌.)
[AttributeUsage(AttributeTargets.Field)]
public class ConditionalHideAttribute : PropertyAttribute
{
    public readonly string conditionalSourceField;
    public readonly object stateValue;

    public bool hideInInspector = false;

    // Use this for initialization
    public ConditionalHideAttribute(string conditionalSourceField, object stateValue, bool hideInInspector = false)
    {
        this.conditionalSourceField = conditionalSourceField;
        this.stateValue = stateValue;

        this.hideInInspector = hideInInspector;
    }
}