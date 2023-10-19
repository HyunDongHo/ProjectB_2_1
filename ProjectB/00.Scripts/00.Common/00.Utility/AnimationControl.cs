using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scheduler;

public class AnimationControlState
{
    private AnimationState animationState;

    public AnimationClip currentClip;
    public bool isCurrentClipRepeat;
    public int currentClipWeight;

    public AnimationControlState() => Reset();

    public void SetState(AnimationState animationState, AnimationClip animationClip, bool isCurrentClipRepeat, int currentClipWeight)
    {
        this.currentClip = animationClip;

        this.animationState = animationState;

        this.isCurrentClipRepeat = isCurrentClipRepeat;
        this.currentClipWeight = currentClipWeight;
    }

    public void Reset()
    {
        currentClip = null;

        animationState = null;

        isCurrentClipRepeat = false;
        currentClipWeight = -1;
    }

    public AnimationState GetAnimationState()
    {
        return animationState;
    }
}

public class AnimationControl : MonoBehaviour
{
    public bool isDebugAnimationFrame = false;

    public Animation ani;

    public AnimationControlState controlState = new AnimationControlState();
    private TimerBuffer animationBuffer = new TimerBuffer(float.MaxValue);

    // 매개변수 (clipName, 애니 시작 NormalizedTime, 애니 속도, 같은 애니 실행 가능한지, 애니 반복 여부, 실행 애니 무게, Frame마다 호출, 애니 시작시 호출, 애니 끝나면 호출)
    public void PlayAnimation(string clipName,
        float startNormalizedTime = 0, float speed = 1, bool isSameAniAvailablePlay = true, bool isRepeat = false, int weight = 0,
        Action<int> OnAnimationFrame = null, Action OnAnimationStart = null, Action OnAnimationEnd = null)
    {
        if (!CheckClipExist(clipName)) return;

        StartAnimation(animationPlayAction: () => ani.Play(clipName, PlayMode.StopAll),
            clipName, startNormalizedTime, speed, isSameAniAvailablePlay, isRepeat, weight, OnAnimationFrame, OnAnimationStart, OnAnimationEnd);
    }

    public void PlayAnimation(AnimationClip clip,
        float startNormalizedTime = 0, float speed = 1, bool isSameAniAvailablePlay = true, bool isRepeat = false, int weight = 0,
        Action<int> OnAnimationFrame = null, Action OnAnimationStart = null, Action OnAnimationEnd = null)
    {
        if (ani.GetClip(clip.name) == null)
            ani.AddClip(clip, clip.name);

        StartAnimation(animationPlayAction: () => ani.Play(clip.name, PlayMode.StopAll),
            clip.name, startNormalizedTime, speed, isSameAniAvailablePlay, isRepeat, weight, OnAnimationFrame, OnAnimationStart, OnAnimationEnd);
    }

    // 매개변수 (clipName, 애니 블렌딩, 애니 시작 NormalizedTime, 애니 속도, 같은 애니 실행 가능한지, 애니 반복 여부, 실행 애니 무게, Frame마다 호출, 애니 시작시 호출, 애니 끝나면 호출)
    public void PlayAnimationCrossFade(string clipName, float fadeLength = 0.5f,
        float startNormalizedTime = 0, float speed = 1, bool isSameAniAvailablePlay = true, bool isRepeat = false, int weight = 0,
        Action<int> OnAnimationFrame = null, Action OnAnimationStart = null, Action OnAnimationEnd = null)
    {
        if (!CheckClipExist(clipName)) return;

        StartAnimation(animationPlayAction: () => ani.CrossFade(clipName, fadeLength, PlayMode.StopAll),
            clipName, startNormalizedTime, speed, isSameAniAvailablePlay, isRepeat, weight, OnAnimationFrame, OnAnimationStart, OnAnimationEnd);
    }

    public void PlayAnimationCrossFade(AnimationClip clip, float fadeLength = 0.5f,
        float startNormalizedTime = 0, float speed = 1, bool isSameAniAvailablePlay = true, bool isRepeat = false, int weight = 0,
        Action<int> OnAnimationFrame = null, Action OnAnimationStart = null, Action OnAnimationEnd = null)
    {
        if(ani.GetClip(clip.name) == null)
            ani.AddClip(clip, clip.name);

        StartAnimation(animationPlayAction: () => ani.CrossFade(clip.name, fadeLength, PlayMode.StopAll),
            clip.name, startNormalizedTime, speed, isSameAniAvailablePlay, isRepeat, weight, OnAnimationFrame, OnAnimationStart, OnAnimationEnd);
    }

    private bool CheckClipExist(string clipName)
    {
        if (ani?.GetClip(clipName) == null)
        {
            Debug.LogError($"[AnimationControl] {clipName} does not exist.");

            return false;
        }

        return true;
    }

    private void StartAnimation(Action animationPlayAction,
        string clipName, float startNormalizedTime, float speed, bool isSameAniAvailablePlay, bool isRepeat, int weight,  
        Action<int> OnAnimationFrame, Action OnAnimationStart, Action OnAnimationEnd)
    {
        // 현재 Layer 보다 낮으면 작동 되지 않도록 설정.
        if (weight < GetAnimationControlState().currentClipWeight) return;
            
        if (!isSameAniAvailablePlay && IsSameCurrentAnimation(clipName)) return;

        StartAnimationTimer(animationPlayAction, clipName, startNormalizedTime, speed, isRepeat, weight, OnAnimationFrame, OnAnimationStart, OnAnimationEnd);
    }

    private void StartAnimationTimer(Action animationPlayAction, string clipName, float startNormalizedTime, float speed, bool isRepeat, int weight,
        Action<int> OnAnimationFrame, Action OnAnimationStart, Action OnAnimationEnd)
    {
        SetAnimationBeforeStartAnimationTimer(animationPlayAction, clipName, startNormalizedTime, speed, isRepeat, weight, OnAnimationFrame, OnAnimationStart, OnAnimationEnd);

        int currentFrame = GetCurrentFrame(clipName);

        animationBuffer.time = GetTotalTime(clipName) * (1 - startNormalizedTime) / speed;
        Timer.instance.TimerStart(animationBuffer,
            OnFrame: () =>
            {
                OnAnimationTimerFrame(ref currentFrame, animationPlayAction, clipName, startNormalizedTime, speed, isRepeat, weight, OnAnimationFrame, OnAnimationStart, OnAnimationEnd);
            },
            OnComplete: () =>
            {
                OnAnimationTimerComplete(ref currentFrame, animationPlayAction, clipName, startNormalizedTime, speed, isRepeat, weight, OnAnimationFrame, OnAnimationStart, OnAnimationEnd);
            });
    }

    private void SetAnimationBeforeStartAnimationTimer(Action animationPlayAction, string clipName, float startNormalizedTime, float speed, bool isRepeat, int weight,
        Action<int> OnAnimationFrame, Action OnAnimationStart, Action OnAnimationEnd)
    {
        ResetAnimationState();
        GetAnimationControlState().SetState(ani[clipName], ani.GetClip(clipName), isRepeat, weight);

        ani[clipName].speed = speed;
        ani[clipName].normalizedTime = startNormalizedTime;

        animationPlayAction?.Invoke();
        OnAnimationStart?.Invoke();

        if (isDebugAnimationFrame)
            Debug.Log($"{gameObject.name} - {clipName}이 실행 되었습니다.");
    }

    private void OnAnimationTimerFrame(ref int currentFrame, Action animationPlayAction, string clipName, float startNormalizedTime, float speed, bool isRepeat, int weight,
        Action<int> OnAnimationFrame, Action OnAnimationStart, Action OnAnimationEnd)
    {
        // Frame 마다 Timer가 해당 Animation Frame의 시간을 넘겼는지 확인하여 currentFrame을 Action으로 보내줌.
        // (렉이 발생 하였을 때 Animation Frame이 1 이였지만 다음 프레임 때 3이 되는 경우 때문에 반복문 사용.)
        while (currentFrame != GetTotalFrame(clipName) && animationBuffer.timer * speed >= GetFrameToTime(clipName, currentFrame))
        {
            if (isDebugAnimationFrame)
                Debug.Log($"{gameObject.name} - {clipName} : {currentFrame}");

            OnAnimationFrame?.Invoke(currentFrame);
            currentFrame += 1;
        }
    }

    private void OnAnimationTimerComplete(ref int currentFrame, Action animationPlayAction, string clipName, float startNormalizedTime, float speed, bool isRepeat, int weight,
        Action<int> OnAnimationFrame, Action OnAnimationStart, Action OnAnimationEnd)
    {
        // OnFrame에서 Animation의 마지막 Frame을 체크 하지 않고 넘어가는 경우를 위해 Timer가 끝났을 때는 남은 currentFrame을 Animation의 End Frame과 같게 함.
        while (currentFrame <= GetTotalFrame(clipName))
        {
            OnAnimationFrame?.Invoke(currentFrame);
            currentFrame += 1;
        }

        if (clipName == GetAnimationControlState().currentClip.name)
        {
            ResetAnimationState();

            if (!isRepeat)
            {
                EndAnimationTimer(OnAnimationEnd);
            }
            else
            {
                StartAnimationTimer(animationPlayAction, clipName, startNormalizedTime: 0, speed, isRepeat, weight, OnAnimationFrame, OnAnimationStart, OnAnimationEnd);
            }
        }
    }

    private void EndAnimationTimer(Action OnAnimationEnd)
    {
        ResetAnimationState();

        OnAnimationEnd?.Invoke();
    }

    // 애니메이션은 멈추지 않고 State와 Timer가 작동 되지 않도록 해줌.
    public void ResetAnimationState()
    {
        GetAnimationControlState().Reset();

        Timer.instance.TimerStop(animationBuffer);    
    }

    public bool IsSameCurrentAnimation(string clipName)
    {
        if (GetAnimationControlState().currentClip == null)
            return false;

        return clipName == GetAnimationControlState().currentClip.name;
    }

    public float GetFrameToTime(string clipName, int frame)
    {
        AnimationClip clip = ani[clipName].clip;

        int totalFrame = GetTotalFrame(clipName);

        frame = (frame >= totalFrame) ? totalFrame : frame;

        float execTime = (float)frame / clip.frameRate;
        return execTime;
    }

    public float GetFrameToNomarlizedTime(string clipName, int frame)
    {
        AnimationClip clip = ani[clipName].clip;

        return frame / clip.length / clip.frameRate;
    }

    public int GetCurrentFrame(string clipName)
    {
        AnimationClip clip = ani[clipName].clip;

        int currentFrame = Mathf.CeilToInt(ani[clipName].normalizedTime * clip.length * clip.frameRate);

        return currentFrame;
    }

    public float GetNormarlizedTime(string clipName)
    {
        return ani[clipName].normalizedTime;
    }

    public int GetTotalFrame(string clipName)
    {
        AnimationClip clip = ani[clipName].clip;

        float totalLength = clip.frameRate * clip.length;
        int totalFrame = Mathf.FloorToInt(totalLength);

        return totalFrame;
    }

    public float GetTotalTime(string clipName)
    {
        AnimationClip clip = ani[clipName].clip;

        return clip.length;
    }

    public AnimationControlState GetAnimationControlState()
    {
        return controlState;
    }

    // Animation Key Frame에서 호출 이벤트로 발동.
    public void CreateAnimationEffect(CreateResourceData effectData)
    {
        CreateResourceManager.instance.CreateResource(this.gameObject, effectData.name);
    }

    // Animation Key Frame에서 호출 이벤트로 발동.
    public void PlayAnimationSound(SoundData soundData)
    {
        SoundManager.instance.PlaySound(soundData);
    }

    private void OnDestroy()
    {
        Timer.instance.TimerStop(animationBuffer);
    }
}
