using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scheduler
{
    public class Timer : Singleton<Timer>
    {
        private List<TimerBuffer> buffers = new List<TimerBuffer>();

        public void ClearAllTimer()
        {
            for (int i = 0; i < buffers.Count; i++)
                TimerStop(buffers[i]);

            buffers.Clear();
        }

        private void LateUpdate()
        {
            float deltaTime = Time.deltaTime;

            for (int i = buffers.Count - 1; i >= 0; i--)
            {
                if (buffers.Count <= 0 || i >= buffers.Count) return;

                TimerBuffer buffer = buffers[i];

                if (buffer.timer < buffer.time)
                {
                    buffer.timer += deltaTime;

                    // 만약 Timer가 Time을 넘어가면 값을 같도록 설정.
                    if (buffer.timer > buffer.time) buffer.timer = buffer.time;

                    buffer.OnFrame?.Invoke();
                }
                else
                {
                    TimerStop(buffer, isReset: false);

                    buffer.OnComplete?.Invoke();
                }
            }
        }

        public void TimerStart(TimerBuffer buffer, Action OnFrame = null, Action OnComplete = null, bool isReset = true)
        {
            if (isReset)
                Reset(buffer);

            buffer.OnFrame = OnFrame;
            buffer.OnComplete = OnComplete;

            if (!buffers.Exists(data => data == buffer))
                buffers.Add(buffer);

            buffer.isRunningTimer = true;
        }

        public void TimerStop(TimerBuffer buffer, bool isReset = true)
        {
            if (isReset)
                Reset(buffer);

            buffers.Remove(buffer);

            buffer.isRunningTimer = false;
        }

        public void ForceTimerEnd(TimerBuffer buffer, bool isForceTimerBuffer = false)
        {
            if (isForceTimerBuffer) buffer.timer = buffer.time;

            TimerStop(buffer, isReset: false);

            buffer.OnComplete?.Invoke();
        }

        public void Reset(TimerBuffer buffer)
        {
            buffer.timer = 0;
        }
    }

    public class TimerBuffer
    {
        public Action OnComplete = null;
        public Action OnFrame = null;

        public float time = 0;
        public float timer = 0;

        public bool isRunningTimer = false;

        public bool isTimerEnd
        {
            get { return timer >= time; }
        }

        public TimerBuffer(float time, float timer = 0)
        {
            this.time = time;
            this.timer = timer;
        }
    }
}
