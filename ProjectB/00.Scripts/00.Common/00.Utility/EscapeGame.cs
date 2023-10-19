using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scheduler;

public class EscapeGame : MonoBehaviour
{
    private static EscapeGame instance = null;

    public const float ESCAPE_CANCLE_TIME = 5.0f;
    private TimerBuffer escapeSchedulerBuffer = new TimerBuffer(ESCAPE_CANCLE_TIME);

    private bool isCheckEscape = false;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        if (!isCheckEscape)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isCheckEscape = true;
                Timer.instance.TimerStart(escapeSchedulerBuffer,
                    OnFrame: () =>
                    {
                        if (Input.GetKeyDown(KeyCode.Escape))
                            Application.Quit();
                    },
                    OnComplete: () =>
                    {
                        isCheckEscape = false;
                    });
            }
        }
    }
}
