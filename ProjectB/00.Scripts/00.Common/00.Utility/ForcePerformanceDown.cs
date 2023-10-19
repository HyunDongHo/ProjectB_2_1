using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePerformanceDown : Singleton<ForcePerformanceDown>
{
    public bool isActive = true;
    public float performanceDown = 100;

    private void Update()
    {
        if(isActive)
        {
            for (int i = 0; i < 10000 * performanceDown; i++)
            {
                int down = i;
            }
        }
    }
}
