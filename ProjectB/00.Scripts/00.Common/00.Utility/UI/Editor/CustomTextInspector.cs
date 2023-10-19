using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(CustomText))]
public class CustomTextInspector : TextEditor
{
    private SerializedProperty disableWordWrap;

    protected override void OnEnable()
    {
        base.OnEnable();

        disableWordWrap = serializedObject.FindProperty("disableWordWrap");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.PropertyField(disableWordWrap);

        serializedObject.ApplyModifiedProperties();
    }

}