using UnityEngine;
using UnityEditor;

//Original version of the ConditionalHideAttribute created by Brecht Lecluyse (www.brechtos.com)
//Modified by: -

[CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
public class ConditionalHidePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var condHAtt = (ConditionalHideAttribute)attribute;
        var enabled = GetConditionalHideAttributeResult(condHAtt, property);

        GUI.enabled = enabled;
        if (!condHAtt.hideInInspector || enabled)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var condHAtt = (ConditionalHideAttribute)attribute;
        var enabled = GetConditionalHideAttributeResult(condHAtt, property);

        if (!condHAtt.hideInInspector || enabled)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
        else
        {
            //The property is not being drawn
            //We want to undo the spacing added before and after the property
            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }

    private bool GetConditionalHideAttributeResult(ConditionalHideAttribute condHAtt, SerializedProperty property)
    {
        var enabled = true;

        var propertyPath = property.propertyPath; //returns the property path of the property we want to apply the attribute to
        string conditionPath;

        if (!string.IsNullOrEmpty(condHAtt.conditionalSourceField))
        {
            //Get the full relative property path of the sourcefield so we can have nested hiding
            conditionPath = propertyPath.Replace(property.name, condHAtt.conditionalSourceField); //changes the path to the conditionalsource property path
            var sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

            if (sourcePropertyValue != null)
            {
                enabled = CheckPropertyType(condHAtt, sourcePropertyValue);
            }
        }

        return enabled;
    }

    private bool CheckPropertyType(ConditionalHideAttribute condHAtt, SerializedProperty sourcePropertyValue)
    {
        switch (sourcePropertyValue.propertyType)
        {
            case SerializedPropertyType.Boolean:
                return sourcePropertyValue.boolValue == (bool)condHAtt.stateValue;
            case SerializedPropertyType.Enum:
                return sourcePropertyValue.enumValueIndex == (int)condHAtt.stateValue;
            case SerializedPropertyType.ObjectReference:
                return sourcePropertyValue.objectReferenceValue != null;
            default:
                Debug.LogError("Data type of the property used for conditional hiding [" +
                                sourcePropertyValue.propertyType + "] is currently not supported");
                return true;
        }
    }
}