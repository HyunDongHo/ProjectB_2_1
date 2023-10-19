using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEndFrame : MonoBehaviour
{
    private PlayerControl playerControl;

    public void Init(PlayerControl playerControl)
    {
        this.playerControl = playerControl;
    }

    public void Release() { }


    public void AnimationEnd(float radius)
    {
        GameObject activedPlayer = playerControl.utility.gameObject.transform.parent.gameObject;
        PlayerControl_DefaultStage pd = activedPlayer.GetComponent<PlayerControl_DefaultStage>();
        pd.pRaycast.attackRangeRaycast.radius = radius;

    }



}
