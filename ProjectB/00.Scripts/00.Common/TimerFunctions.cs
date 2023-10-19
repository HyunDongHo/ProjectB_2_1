using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scheduler;

public class TimerFunctions : Singleton<TimerFunctions>
{
    // 고정된 한 프레임이 지나면 OnFrame에 currentFrame을 반환해주는 기능.
    public void TimerFrameToFixedOneTime(TimerBuffer buffer, float oneFrameTime, Action<int> OnFrame = null, Action OnComplete = null)
    {
        int currentFrame = 0;

        Timer.instance.TimerStart(buffer,
            OnFrame: () =>
            {
                while (buffer.timer > oneFrameTime * currentFrame)
                {
                    OnFrame?.Invoke(currentFrame);
                    currentFrame += 1;
                }
            },
            OnComplete: () => OnComplete?.Invoke());
    }
}
