using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RNG;

public enum DamageType
{
    Default,
    Critical,
    Miss
}

public abstract class Control : MonoBehaviour
{
    [SerializeField] private Model model;
    [SerializeField] private Move move;
    [SerializeField] private Attack attack;

    [SerializeField] private Stats stats;

    public bool isEnabledControl = true;
    public bool isAvailableControl = true;

    protected bool isAvailablePlayHitMotion = false;
    private int hitMotionSkipStartFrame = -1;

    protected virtual void Awake()
    {
      //  AddEvent();

        InitHitMotionSkipStartFrame();

        stats.SetBaseStats();
    }

    private void OnEnable()
    {
        AddEvent();
    }

    private void OnDisable()
    {
        RemoveEvent();
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
    }

    protected virtual void OnDestroy()
    {
       // RemoveEvent();
    }

    protected virtual void AddEvent() { }

    protected virtual void RemoveEvent() { }

    private void InitHitMotionSkipStartFrame()
    {
        hitMotionSkipStartFrame = GetInitHitMotionSkipStartFrame();
    }

    protected virtual int GetInitHitMotionSkipStartFrame()
    {
        return -1;
    }

    public void SetFrameIsPlayHitMotion(int currentFrame, int startFrame, int endFrame)
    {
        SetIsPlayHitMotion(!(currentFrame >= startFrame && currentFrame <= endFrame));
    }

    public void SetIsPlayHitMotion(bool isPlayHitMotion)
    {
        this.isAvailablePlayHitMotion = isPlayHitMotion;
    }

    // 공식에 맞춰서 HP 줄어들도록 제작한 함수.
    public DamageType ReduceHp(Control affect, float damage, float criticalRatio, int divideDamage)
    {
        // Miss 일 때도 Attack Motion 발동함으로 맨처음에 실행 시켜줌.

        if(this is PlayerControl_DefaultStage == false)
            GotAttacked(isAvailablePlayHitMotion, hitMotionSkipStartFrame);

        DamageType returnDamageType = DamageType.Miss;

        float attackerDamage = damage;
        int attackerLevel = affect.stats.level.GetCurrentLevel();

        float defenderDefense = stats.defense.GetCurrentDefense();
        int defenderLevel = stats.level.GetCurrentLevel();
        // 공격자의 명중률 체크. 
        // (공격자의 Attack쪽에서 계산하지 않고 Control에서 작업하는 이유는 공격에 대한 예외처리 부분을 한번에 처리및 관리해야 편리하기 때문임.)
        if (affect.CheckAvailableHit(attackerDamage, defenderDefense, attackerLevel, defenderLevel))
        {
            // 회피 체크.
            if (CheckAvailableEvasion(attackerLevel, defenderLevel))
            {
                // 공격자의 대미지 감소 계산 후 값을 가져옴.
                float reduceHp = affect.GetConvertDamageReduce(divideDamage, attackerDamage, defenderDefense, attackerLevel, defenderLevel);

                if (criticalRatio > 0)
                {
                    reduceHp *= criticalRatio;

                    returnDamageType = DamageType.Critical;
                }
                else
                {
                    DamageTextManager.instance.CreateNormalDamage(reduceHp, transform.position + Vector3.up);
                    returnDamageType = DamageType.Default;
                }

                stats.hp.ReduceHp(affect.gameObject, reduceHp);
            }
        }

        return returnDamageType;
    }

    // 공격에 맞게 되면 호출되는 함수.
    protected virtual void GotAttacked(bool isPlayHitMotion, int hitMotionSkipStartFrame) { }
    protected virtual void GotAttackedKnockBack(bool isPlayHitMotion, int hitMotionSkipStartFrame, float mag) { }
    // 명중률 검사.
    protected bool CheckAvailableHit(float attackerDamage, float defenderDefense, int attackerLevel, int defenderLevel)
    {
        float chanceToHitPercentage = CalculateHitPercentage(attackerDamage, defenderDefense, attackerLevel, defenderLevel);

        RandomSetting randomSetting = new RandomSetting(chanceToHitPercentage);
        RandomSetting result = RNGManager.instance.GetRandom(randomSetting);

        if (randomSetting == result)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // 명중률 Percentage 가져오기.
    protected virtual float CalculateHitPercentage(float attackerDamage, float defenderDefense, int attackerLevel, int defenderLevel)
    {
        if (attackerDamage <= 0)
            attackerDamage = 1;

        if (defenderDefense <= 0)
            defenderDefense = 1;

        if (attackerLevel <= 0)
            attackerLevel = 1;

        if (defenderLevel <= 0)
            defenderLevel = 1;

        // 공격 대상의 명중률 (Percentage) 계산.
        // 200(기본 값) * ((받을 대미지 / (받을 대미지 + 나의 방어력)) * (공격하는 사람의 레벨 / (공격하는 사람의 레벨 + 나의 레벨))
        float chanceToHitPercentage = 200 * (attackerDamage / (attackerDamage + defenderDefense)) * ((float)attackerLevel / (attackerLevel + defenderLevel));

        int levelDiff = (defenderLevel - attackerLevel);
        float additionalDbuffChanceToHitByLevelDiff = 0;

        if (levelDiff == 1)
            additionalDbuffChanceToHitByLevelDiff = -2.5f;
        else if (levelDiff == 2)
            additionalDbuffChanceToHitByLevelDiff = -5f;
        else if (levelDiff == 3)
            additionalDbuffChanceToHitByLevelDiff = -10f;
        else if (levelDiff == 4)
            additionalDbuffChanceToHitByLevelDiff = -20f;
        else if (levelDiff >= 5)
            additionalDbuffChanceToHitByLevelDiff = -30f;

        chanceToHitPercentage += additionalDbuffChanceToHitByLevelDiff;

        return chanceToHitPercentage;
    }

    // 회피 검사.
    private bool CheckAvailableEvasion(int attackerLevel, int defenderLevel)
    {
        float evasionPercentage = CalculateEvasionPercentage(attackerLevel, defenderLevel);

        if (!stats.defense.IsAvailableEvasion(evasionPercentage))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // 회피 확률 값 가져오기.
    protected virtual float CalculateEvasionPercentage(int attackerLevel, int defenderLevel)
    {
        // 나의 회피률 (Perecentage) 계산.
        // 나의 회피률 + (나의 방어력 * 0.3f)
        float evasionPercentage = stats.defense.GetBaseEvasionPercentage() + stats.defense.GetCurrentDefense() * 0.3f;

        // 회피율 디버프
        int levelDiff = (attackerLevel - defenderLevel);
        float additionalDbuffEvasionByLevelDiff = 0;

        if (levelDiff == 1)
            additionalDbuffEvasionByLevelDiff = -2.5f;
        else if (levelDiff == 2)
            additionalDbuffEvasionByLevelDiff = -5f;
        else if (levelDiff == 3)
            additionalDbuffEvasionByLevelDiff = -10f;
        else if (levelDiff == 4)
            additionalDbuffEvasionByLevelDiff = -20f;
        else if (levelDiff >= 5)
            additionalDbuffEvasionByLevelDiff = -30f;

        evasionPercentage += additionalDbuffEvasionByLevelDiff;

        return evasionPercentage;
    }

    // 추가 대미지 감소 변환.
    private float GetConvertDamageReduce(int divideDamage, float attackerDamage, float defenderDefense, int attackerLevel, int defenderLevel)
    {
        float damageReduceRatio = CalculateDamageReduceRatio(divideDamage, attackerDamage, defenderDefense, attackerLevel, defenderLevel);

        //방어율 디버프 추가
        int levelDiff = (attackerLevel - defenderLevel);
        float additionalDamage = 0;

        if (levelDiff == 1)
            additionalDamage = 2.5f;
        else if (levelDiff == 2)
            additionalDamage = 5f;
        else if (levelDiff == 3)
            additionalDamage = 10f;
        else if (levelDiff == 4)
            additionalDamage = 20f;
        else if (levelDiff >= 5)
            additionalDamage = 30f;

        damageReduceRatio += additionalDamage;

        if (damageReduceRatio > 1)
            damageReduceRatio = 1;

        // (받을 피해량 / (여러번 공격하는 경우 나눠지는 damage량) * 피해 감소량
        float convertDamage = (attackerDamage / divideDamage) * damageReduceRatio;

        return convertDamage;
    }

    // 대미지 감소 값 가져오기
    protected virtual float CalculateDamageReduceRatio(int divideDamage, float attackerDamage, float defenderDefense, int attackerLevel, int defenderLevel)
    {
        // 피해 감소량 계산.
        // (100 / (100 + 나의 방어력))
        return 100 / (100 + defenderDefense);
    }

    #region Base Control 

    public void ChangeModel(Model model)
    {
        this.model = model;
    }

    public void ChangeMove(Move move)
    {
        this.move = move;
    }

    public void ChangeAttack(Attack attack)
    {
        this.attack = attack;
    }

    public void ChangeStats(Stats stats)
    {
        this.stats = stats;
    }

    public virtual T GetModel<T>() where T : Model
    {
        return model as T;
    }

    public T GetMove<T>() where T : Move
    {
        return move as T;
    }

    public T GetAttack<T>() where T : Attack
    {
        return attack as T;
    }

    public T GetStats<T>() where T : Stats
    {
        return stats as T;
    }

    #endregion
}
