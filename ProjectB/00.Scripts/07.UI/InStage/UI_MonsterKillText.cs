using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterKillTextState
{
    IDLE,
    KILL
}

public class UI_MonsterKillText : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    private void Awake()
    {
        PlayAnim(MonsterKillTextState.IDLE);
    }

    public void PlayAnim(MonsterKillTextState monsterKillTextState)
    {
        animator.Play(monsterKillTextState.ToString());
    }

}
