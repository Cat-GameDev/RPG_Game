using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class CharacterStats : MonoBehaviour, IHit
{
    [SerializeField] protected CharacterFX characterFX;

    [Header("Major stats")]
    public Stats strength; // 1 point increase damage by 1 and crit.power by 1%
    public Stats agility; // 1 point increase evasion by 1 % and crit.change by 1% // Nhanh nhwen
    public Stats intelligence; // 1 point increase magic damage by 1 and magic resistance by 3
    public Stats vitality; // 1 point increase health by 3 or 5 point // anh huong den suc kheo cua nhan vat

    [Header("Offensive stats")]
    public Stats damage;
    public Stats critChance;
    public Stats critPower;      // default value 150%


    [Header("Defensive stats")]
    public Stats hp;
    public Stats armor; // giap
    public Stats evasion; // le tranh
    public Stats magicResistance;

    [Header("Magic stats")]
    public Stats fireDamage;
    public Stats iceDamage;
    public Stats lightingDamage;

    public bool isIgnited; // bi dot chay
    public bool isChilled; // bi dong cuc
    public bool isShocked; // diet dat



    public bool IsDead => currrentHp <= 0;

    public CharacterFX CharacterFX { get => characterFX; }

    [SerializeField] float currrentHp;

    public void OnInit()
    {
        currrentHp = hp.GetValue();
        critPower.SetDefoutlValue(150);
    }

    
    public abstract void OnDeath();

    public virtual void DoDamge(CharacterStats characterStats)
    {
        if (TargetCanVoidAttack(characterStats))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        if(CanCrit())
        {
            totalDamage = CalculateCritialDamage(totalDamage);
        }

        
        totalDamage = CheckTargetArmor(characterStats, totalDamage);

        characterStats.OnHit(totalDamage);

    }

    public virtual void OnHit(float damage)
    {
        if(!IsDead)
        {
            currrentHp -= damage;
            characterFX.StartCoroutine(nameof(characterFX.FlashFX));
            if(IsDead)
            {
                OnDeath();
            }
        }
    }

    public virtual void DoMagicalDamage(CharacterStats characterStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        int totalMagicDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();
        totalMagicDamage = CheckTargetResistance(characterStats, totalMagicDamage);

        characterStats.OnHit(totalMagicDamage);
    }

    public void ApplyAilments(bool isIgnited, bool isChilled, bool isShocked )
    {
        if(this.isChilled || this.isIgnited || this.isShocked)
            return;

        this.isChilled = isChilled;
        this.isIgnited = isIgnited;
        this.isShocked = isShocked;
    }


    private int CheckTargetResistance(CharacterStats characterStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= characterStats.magicResistance.GetValue() + (characterStats.intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    private int CalculateCritialDamage(int damage)
    {
        float totalCriticalDamage = (critPower.GetValue() + strength.GetValue()) * 0.01f;

        float critalDamage = damage * totalCriticalDamage;

        return Mathf.RoundToInt(critalDamage);
    }
    private bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if(Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }

        return false;
    }
    private bool TargetCanVoidAttack(CharacterStats characterStats)
    {
        int totalEvasion = characterStats.evasion.GetValue() + characterStats.intelligence.GetValue();

        if(Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }

        return false;
    }

    private int CheckTargetArmor(CharacterStats characterStats, int totalDamage)
    {
        totalDamage -= characterStats.armor.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }


}
