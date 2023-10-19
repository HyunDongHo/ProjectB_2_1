using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Create Attack Data", menuName = "Custom/Attack Data")]
public class AttackData : ScriptableObject
{
    public AnimationClip clip;
    private Dictionary<string, Action<EventFrameParameter>> actions = new Dictionary<string, Action<EventFrameParameter>>();

    public AttackDataParamterRef paramterRef;

    [System.Serializable]
    public class AttackEvent
    {
        public string eventName = string.Empty;

        public bool isOpen = false;

        [System.Serializable]
        public class EventFrameSetting
        {
            public bool isShowMinMaxFrame = false;

            public int minFrame = 0;
            public int maxFrame = 0;

            public List<EventMethod> eventMethods = new List<EventMethod>();
        }
        public List<EventFrameSetting> eventFrameSettings = new List<EventFrameSetting>();
    }
    [HideInInspector] public List<AttackEvent> attackEvents = new List<AttackEvent>();

    [System.Serializable]
    public class EventMethod
    {
        public int selectedIndex;

        public string callMethod;
        public EventFrameParameter eventFrameParameter;
    }

    [System.Serializable]
    public class EventFrameParameter
    {
        public UnityEngine.Object objectValue = null;

        public bool boolValue = false;
        public float floatValue = 0;
        public float floatValue1 = 0;
        public float floatValue2 = 0;
        public float floatValue3 = 0;
        public int intValue = 0;
        public string stringValue = string.Empty;
    }

    private void Awake()
    {
        RepeatAttackEvent((eventFrameSetting) =>
        {
            foreach (var eventMethod in eventFrameSetting.eventMethods)
            {
                if(!actions.ContainsKey(eventMethod.callMethod))
                {
                    actions.Add(eventMethod.callMethod, null);
                }
            }
        });
    }

    public void AddHandleEvent(string eventMethod, Action<EventFrameParameter> handleEvent)
    {
        if(actions.ContainsKey(eventMethod))
            actions[eventMethod] += handleEvent;
    }
    public void RemoveHandleEvent(string eventMethod, Action<EventFrameParameter> handleEvent)
    {
        if (actions.ContainsKey(eventMethod))
            actions[eventMethod] = null;
    }
    public void ResetAllHandleEvent()
    {
        RepeatAttackEvent((eventFrameSetting) =>
        {
            foreach (var eventMethod in eventFrameSetting.eventMethods)
                actions[eventMethod.callMethod] = null;
        });
    }

    public void CheckCurrentAnimationFrame(int frame)
    {
        RepeatAttackEvent((eventFrameSetting) =>
        {
            if (eventFrameSetting.minFrame <= frame && frame <= eventFrameSetting.maxFrame)
            {
                foreach (var eventMethod in eventFrameSetting.eventMethods)
                {
                    actions[eventMethod.callMethod]?.Invoke(eventMethod.eventFrameParameter);
                }
            }
        });
    }

    public int GetAttackMethodsCount(string method)
    {
        int methodCount = 0;

        RepeatAttackEvent((eventFrameSetting) =>
        {
            foreach (var eventMethod in eventFrameSetting.eventMethods)
                if(eventMethod.callMethod == method)
                    methodCount += 1;
        });

        return methodCount;
    }

    public float GetTotalLength()
    {
        return clip.length;
    }

    public int GetTotalFrame()
    {
        return Mathf.FloorToInt(clip.frameRate * clip.length);
    }

    public float GetFrameToNomarlizedTime(int frame)
    {
        return frame / clip.length / clip.frameRate;
    }

    private void RepeatAttackEvent(Action<AttackEvent.EventFrameSetting> action)
    {
        foreach (var attackEvent in attackEvents)
            foreach (var eventFrameSetting in attackEvent.eventFrameSettings)
                action?.Invoke(eventFrameSetting);
    }
}
