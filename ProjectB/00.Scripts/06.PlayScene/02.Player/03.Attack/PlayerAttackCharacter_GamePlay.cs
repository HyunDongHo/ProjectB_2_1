using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCharacter_GamePlay : PlayerAttackCharacter
{
    public PlayerAttackCharacter_GamePlay(PlayerAttack playerAttack) : base(playerAttack)
    {
    }

    #region DashCombo

    public void Attack_DashLeft(int frame, float addSp)
    {
        if (frame == 9)
        {
            (attack as PlayerAttack_GamePlay).AttackTargets(PlayerAttackType.None, cameraShakePower: 5, cameraShakeTime: 0.1f, divideDamage: 1, addSp);
        }

        if (frame == 22)
        {
            (attack as PlayerAttack_GamePlay).EndDashCombo();
        }

        attack.control.SetFrameIsPlayHitMotion(frame, 7, 22);
    }

    public void Attack_DashRight(int frame, float addSp)
    {
        if (frame == 15)
        {
            (attack as PlayerAttack_GamePlay).AttackTargets(PlayerAttackType.None, cameraShakePower: 5, cameraShakeTime: 0.1f, divideDamage: 1, addSp);
        }

        if (frame == 25)
        {
            (attack as PlayerAttack_GamePlay).EndDashCombo();
        }

        attack.control.SetFrameIsPlayHitMotion(frame, 5, 30);
    }

    #endregion

    #region Combo

    public void Attack_LeftCombo(int frame, float addSp, int leftCombo)
    {
        switch (leftCombo)
        {
            case 0:
                if (frame == 5)
                {
                    (attack as PlayerAttack_GamePlay).AttackTargets(PlayerAttackType.None, cameraShakePower: 5, cameraShakeTime: 0.1f, divideDamage: 1, addSp);
                    (attack as PlayerAttack_GamePlay).leftCombo = 1;
                }
                if (frame == 12)
                {
                    (attack as PlayerAttack_GamePlay).EndCombo();
                }
                attack.control.SetFrameIsPlayHitMotion(frame, 0, 10);
                break;
            case 1:
                if (frame == 9)
                {
                    (attack as PlayerAttack_GamePlay).AttackTargets(PlayerAttackType.None, cameraShakePower: 10, cameraShakeTime: 0.1f, divideDamage: 1, addSp);
                    (attack as PlayerAttack_GamePlay).leftCombo = 2;
                }
                if (frame == 16)
                {
                    (attack as PlayerAttack_GamePlay).EndCombo();
                }
                attack.control.SetFrameIsPlayHitMotion(frame, 0, 21);
                break;
            case 2:
                if (frame == 3)
                {
                    (attack as PlayerAttack_GamePlay).AttackTargets(PlayerAttackType.None, cameraShakePower: 15, cameraShakeTime: 0.1f, divideDamage: 1, addSp);
                    (attack as PlayerAttack_GamePlay).leftCombo = 3;
                }
                if (frame == 17)
                {
                    (attack as PlayerAttack_GamePlay).EndCombo();
                }
                attack.control.SetFrameIsPlayHitMotion(frame, 0, 19);
                break;
            case 3:
                if (frame == 21)
                {
                    (attack as PlayerAttack_GamePlay).AttackTargets(PlayerAttackType.None, cameraShakePower: 20, cameraShakeTime: 0.1f, divideDamage: 1, addSp);
                    (attack as PlayerAttack_GamePlay).leftCombo = 0;
                }
                if (frame == 37)
                {
                    (attack as PlayerAttack_GamePlay).EndCombo();
                }
                attack.control.SetFrameIsPlayHitMotion(frame, 0, 26);
                break;
        }
    }

    public void Attack_AnotherLeftToRightCombo(int frame, float addSp, int leftCombo, int rightCombo)
    {
        switch (rightCombo)
        {
            case 0:
                switch (leftCombo)
                {
                    case 1:
                        if (frame == 11)
                        {
                            (attack as PlayerAttack_GamePlay).AttackTargets(PlayerAttackType.None, cameraShakePower: 5, cameraShakeTime: 0.1f, divideDamage: 1, addSp);
                            (attack as PlayerAttack_GamePlay).leftCombo = 0;
                        }
                        if (frame == 26)
                        {
                            (attack as PlayerAttack_GamePlay).EndCombo();
                        }
                        attack.control.SetFrameIsPlayHitMotion(frame, 0, 28);
                        break;
                    case 2:
                        if (frame == 3 || frame == 7 || frame == 13)
                        {
                            (attack as PlayerAttack_GamePlay).AttackTargets(PlayerAttackType.None, cameraShakePower: 5, cameraShakeTime: 0.1f, divideDamage: 3, addSp);
                            (attack as PlayerAttack_GamePlay).leftCombo = 0;
                        }
                        if (frame == 25)
                        {
                            (attack as PlayerAttack_GamePlay).EndCombo();
                        }
                        attack.control.SetFrameIsPlayHitMotion(frame, 0, 25);
                        break;
                    case 3:
                        if (frame == 5 || frame == 13)
                        {
                            (attack as PlayerAttack_GamePlay).AttackTargets(PlayerAttackType.None, cameraShakePower: 5, cameraShakeTime: 0.1f, divideDamage: 2, addSp);
                            (attack as PlayerAttack_GamePlay).leftCombo = 0;
                        }
                        if (frame == 29)
                        {
                            (attack as PlayerAttack_GamePlay).EndCombo();
                        }
                        attack.control.SetFrameIsPlayHitMotion(frame, 0, 32);
                        break;
                }
                break;
        }
    }

    private void Attack_AnotherRightToLeftCombo(int frame, float addSp, int leftCombo, int rightCombo)
    {
    }

    #endregion

    #region BaseAttack

    public void Attack_Auto(int frame, float addSp)
    {
        if (frame == 13 || frame == 27)
        {
            (attack as PlayerAttack_GamePlay).AttackTargets(PlayerAttackType.Auto, cameraShakePower: 5, cameraShakeTime: 0.1f, divideDamage: 2, addSp);
        }
    }

    #endregion
}
