using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AttackData))]
public class AttackDataEditor : Editor
{
    private AttackData attackData;

    private void OnEnable()
    {
        attackData = target as AttackData;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AnimationClip clip = attackData.clip;

        if (clip != null)
        {
            EditorGUI.BeginChangeCheck();

            ShowClipDetail(clip);

            for (int i = 0; i < attackData.attackEvents.Count; i++)
            {
                ShowDefaultClipFrameEvent(clip, attackData.attackEvents[i]);
            }
            AddCheckClipFrameEvent();

            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(attackData);
        }
    }

    #region Show Clip Detail

    private void ShowClipDetail(AnimationClip clip)
    {
        EditorGUILayout.Space();
        GUILayout.Label($"애니메이션 클립 정보 -\n총 프레임 : {attackData.GetTotalFrame()}\n총 시간 : {clip.length}");
        EditorGUILayout.Space();
    }

    #endregion

    #region Show Clip Frame Event 관리

    private void ShowDefaultClipFrameEvent(AnimationClip clip, AttackData.AttackEvent animationEvent)
    {
        EditorGUILayout.BeginVertical(GUI.skin.GetStyle("HelpBox"));
        {
            string foldContent = $"프레임 이벤트 {(!string.IsNullOrEmpty(animationEvent.eventName) ? $"({animationEvent.eventName})" : string.Empty)}";

            animationEvent.isOpen = EditorGUILayout.Foldout(animationEvent.isOpen, foldContent);
            if (animationEvent.isOpen)
            {
                animationEvent.eventName = EditorGUILayout.TextField(animationEvent.eventName);

                AddEventControl(animationEvent);

                EditorGUILayout.BeginHorizontal(GUI.skin.GetStyle("HelpBox"));
                {
                    EditorGUILayout.BeginVertical();
                    {
                        AddCheckClipFrameEvent(animationEvent);
                        ShowClipFrameEvents(animationEvent);
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndVertical();
    }

    private void AddEventControl(AttackData.AttackEvent animationEvent)
    {
        EditorGUILayout.BeginHorizontal();
        {
            int currentOrder = attackData.attackEvents.IndexOf(animationEvent);

            if (GUILayout.Button("위로"))
                ChangeClipFrameEventOrder(currentOrder, changeOrder: currentOrder - 1);

            if (GUILayout.Button("아래로"))
                ChangeClipFrameEventOrder(currentOrder, changeOrder: currentOrder + 1);

            if (GUILayout.Button("삭제"))
                RemoveClipFrameEvent(animationEvent);
        }
        EditorGUILayout.EndHorizontal();
    }

    private void AddCheckClipFrameEvent(AttackData.AttackEvent animationEvent)
    {
        if (GUILayout.Button("프레임 추가"))
            animationEvent.eventFrameSettings.Add(new AttackData.AttackEvent.EventFrameSetting());
    }

    private void ShowClipFrameEvents(AttackData.AttackEvent animationEvent)
    {
        for (int i = 0; i < animationEvent.eventFrameSettings.Count; i++)
        {
            AttackData.AttackEvent.EventFrameSetting eventFrameSetting = animationEvent.eventFrameSettings[i];
            EditorGUILayout.BeginHorizontal();
            {
                if (eventFrameSetting.isShowMinMaxFrame)
                {
                    int minFrame = EditorGUILayout.IntField(eventFrameSetting.minFrame, GUILayout.MaxWidth(50));
                    GUILayout.Label("~", GUILayout.MaxWidth(15));
                    int maxFrame = EditorGUILayout.IntField(eventFrameSetting.maxFrame, GUILayout.MaxWidth(50));

                    eventFrameSetting.minFrame = minFrame;
                    eventFrameSetting.maxFrame = maxFrame;
                }
                else
                {
                    int frame = EditorGUILayout.IntField(eventFrameSetting.minFrame, GUILayout.MaxWidth(50));

                    eventFrameSetting.minFrame = frame;
                    eventFrameSetting.maxFrame = frame;
                }
                AddClipFrameEventControl(animationEvent, eventFrameSetting);
            }
            EditorGUILayout.EndHorizontal();

            ShowEventMethods(eventFrameSetting);
        }
    }

    private void ShowEventMethods(AttackData.AttackEvent.EventFrameSetting eventFrameSetting)
    {
        for (int i = 0; i < eventFrameSetting.eventMethods.Count; i++)
        {
            AttackData.EventMethod eventMethod = eventFrameSetting.eventMethods[i];

            if (eventMethod == null) continue;

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("Method :");

                if (attackData.paramterRef != null)
                {
                    string[] methodDefines = new string[attackData.paramterRef.methodDefines.Count + 1]; attackData.paramterRef.methodDefines.ToArray();

                    int setMethod = 0;
                    while (setMethod < attackData.paramterRef.methodDefines.Count)
                    {
                        methodDefines[setMethod] = attackData.paramterRef.methodDefines[setMethod];
                        setMethod += 1;
                    }
                    methodDefines[setMethod] = "Custom";

                    if (methodDefines.Length > 0)
                    {
                        if(eventMethod.selectedIndex == attackData.paramterRef.methodDefines.Count)
                        {
                            for (int findMethod = 0; findMethod < methodDefines.Length; findMethod++)
                            {
                                if(methodDefines[findMethod] == eventMethod.callMethod)
                                    eventMethod.selectedIndex = findMethod;
                            }

                            eventMethod.callMethod = EditorGUILayout.TextField(eventMethod.callMethod);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(eventMethod.callMethod))
                                eventMethod.callMethod = methodDefines[0];

                            int selectedIndex = attackData.paramterRef.methodDefines.IndexOf(eventMethod.callMethod);

                            eventMethod.selectedIndex = EditorGUILayout.Popup(selectedIndex, displayedOptions: methodDefines);
                            eventMethod.callMethod = methodDefines[eventMethod.selectedIndex];
                        }
                    }
                }
                else
                    eventMethod.callMethod = EditorGUILayout.TextField(eventMethod.callMethod);

                AttackData.EventFrameParameter parameter = eventMethod.eventFrameParameter;

                if(parameter != null)
                {
                    GUILayout.Label("Int :");
                    parameter.intValue = EditorGUILayout.IntField(parameter.intValue, GUILayout.MaxWidth(50));

                    GUILayout.Label("Float :");
                    parameter.floatValue = EditorGUILayout.FloatField(parameter.floatValue, GUILayout.MaxWidth(25));
                    GUILayout.Label("Float1 :");
                    parameter.floatValue1 = EditorGUILayout.FloatField(parameter.floatValue1, GUILayout.MaxWidth(25));
                    GUILayout.Label("Float2 :");
                    parameter.floatValue2 = EditorGUILayout.FloatField(parameter.floatValue2, GUILayout.MaxWidth(25));
                    GUILayout.Label("Float3 :");
                    parameter.floatValue3 = EditorGUILayout.FloatField(parameter.floatValue3, GUILayout.MaxWidth(25));

                    GUILayout.Label("String :");
                    parameter.stringValue = EditorGUILayout.TextField(parameter.stringValue);

                    GUILayout.Label("Bool :");
                    parameter.boolValue = EditorGUILayout.Toggle(parameter.boolValue, GUILayout.Width(15));

                    GUILayout.Label("Object :");
                    parameter.objectValue = EditorGUILayout.ObjectField(parameter.objectValue, typeof(Object), allowSceneObjects: true, GUILayout.MaxWidth(150));
                }

                if (GUILayout.Button("삭제"))
                    eventFrameSetting.eventMethods.Remove(eventMethod);
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    private void AddClipFrameEventControl(AttackData.AttackEvent animationEvent, AttackData.AttackEvent.EventFrameSetting eventFrame)
    {
        if (GUILayout.Button("개별 이벤트 추가"))
            eventFrame.eventMethods.Add(new AttackData.EventMethod());

        if (GUILayout.Button("Frame Show 변경"))
            eventFrame.isShowMinMaxFrame = !eventFrame.isShowMinMaxFrame;

        if (GUILayout.Button("삭제"))
            animationEvent.eventFrameSettings.Remove(eventFrame);
    }

    #endregion

    #region Control Clip Frame Event

    private void AddCheckClipFrameEvent()
    {
        if (GUILayout.Button("프레임 컨트롤 추가"))
        {
            attackData.attackEvents.Add(new AttackData.AttackEvent());
        }
    }

    private void ChangeClipFrameEventOrder(int currentOrder, int changeOrder)
    {
        if (changeOrder < 0 || changeOrder >= attackData.attackEvents.Count) return;

        AttackData.AttackEvent temp = attackData.attackEvents[currentOrder];

        attackData.attackEvents[currentOrder] = attackData.attackEvents[changeOrder];
        attackData.attackEvents[changeOrder] = temp;
    }

    private void RemoveClipFrameEvent(AttackData.AttackEvent animationEvent)
    {
        attackData.attackEvents.Remove(animationEvent);
    }

    #endregion
}
