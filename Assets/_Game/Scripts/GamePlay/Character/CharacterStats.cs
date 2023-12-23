using UnityEngine;
using Random = UnityEngine.Random;

public abstract class CharacterStats : MonoBehaviour, IHit
{
    public const int PLUS_POINT_SHOCKED = 20;
    public const float REDUCE_ARMOR_CHILL = .2f; // 20%
    public const float IGNITE_DAMAGE_OVER_TIME = .2f; // 20%
    public const float CHILL_DAMAGE_OVER_TIME = .3f;
    public const float TIME_AILMENT = 4f;

    [SerializeField] protected CharacterFX characterFX;
    protected HealthBar_UI healthBar_UI;

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

    public bool isIgnited; // bi dot chay // damage over time
    public bool isChilled; // bi dong cuc // reduce armor 20%
    public bool isShocked; // diet dat // reduce accuracy(Do chinh xac) by 20%

    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;


    private float igniteDamageCoodlown = .3f;
    private float igniteDamageTimer;
    private int igniteDamage;
    private int shockDamage;


    public bool IsDead => currrentHp <= 0;

    public CharacterFX CharacterFX { get => characterFX; }

    [SerializeField] float currrentHp;

    void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;


        if (ignitedTimer < 0)
            isIgnited = false;

        if (chilledTimer < 0)
            isChilled = false;

        if (shockedTimer < 0)
            isShocked = false;

        if(isIgnited && !IsDead)
            ApplyIgniteDamage();
    }

    public virtual void OnInit()
    {
        currrentHp = hp.GetValue();
        critPower.SetDefoutlValue(150);
        isShocked = isChilled = isIgnited = false;
    }

    protected abstract void CreateHealthBar();
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

        // if have a magic
        DoMagicalDamage(characterStats);

    }

    public virtual void OnHit(float damage)
    {
        if(!IsDead)
        {
            characterFX.StartCoroutine(nameof(characterFX.FlashFX));
            DecreaseHealth(damage);
        }
    }

    private void DecreaseHealth(float damage)
    {
        currrentHp -= damage;
        if (IsDead)
        {
            OnDeath();
        }
        healthBar_UI.SetNewHp(currrentHp);
    }

    #region Magic and Ailment
    public virtual void DoMagicalDamage(CharacterStats characterStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        int totalMagicDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();
        totalMagicDamage = CheckTargetResistance(characterStats, totalMagicDamage);

        characterStats.OnHit(totalMagicDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
            return;

        AttempyToApplyAilment(characterStats, _fireDamage, _iceDamage, _lightingDamage);

    }
    private void AttempyToApplyAilment(CharacterStats characterStats, int _fireDamage, int _iceDamage, int _lightingDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _iceDamage && _lightingDamage > _fireDamage;

        while (!canApplyChill && !canApplyIgnite && !canApplyShock)
        {
            if (Random.value < .3f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                characterStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < .5f && _iceDamage > 0)
            {
                canApplyChill = true;
                characterStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < .5f && _lightingDamage > 0)
            {
                canApplyShock = true;
                characterStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }

        if (canApplyIgnite)
            characterStats.SetupIgniteDamange(Mathf.RoundToInt(_fireDamage * IGNITE_DAMAGE_OVER_TIME));

        if (canApplyShock)
            characterStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightingDamage * .1f));

        characterStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    public void ApplyAilments(bool isIgnited, bool isChilled, bool isShocked)
    {
        bool canApplyIgnite = !this.isIgnited && !this.isChilled && !this.isShocked;
        bool canApplyChill = !this.isIgnited && !this.isChilled && !this.isShocked;
        bool canApplyShock = !this.isIgnited && !this.isChilled;

        if(isIgnited && canApplyIgnite)
        {
            this.isIgnited = isIgnited;
            ignitedTimer = TIME_AILMENT;
            characterFX.IgniteFxFor(TIME_AILMENT);
        }
        
        if(isChilled && canApplyChill)
        {
            this.isChilled = isChilled;
            ignitedTimer = TIME_AILMENT;

            GetComponent<Character>().SlowCharacterBy(CHILL_DAMAGE_OVER_TIME, TIME_AILMENT);
            characterFX.ChillFxFor(TIME_AILMENT);
        }

        if(isShocked && canApplyShock)
        {
            if(!this.isShocked)
            {
                ApplyShock(isShocked);
            }
            else
            {
                if (GetComponent<Player>() != null)
                    return;

                HitNearestTargetWithShockStrike();
            }
            
        }
    }

    public void ApplyShock(bool isShocked)
    {
        if(this.isShocked)
            return;

        this.isShocked = isShocked;
        shockedTimer = TIME_AILMENT;
        characterFX.ShockFxFor(TIME_AILMENT);
    }
    private void ApplyIgniteDamage()
    {
        if(igniteDamageTimer < 0)
        {
            DecreaseHealth(igniteDamage);
            igniteDamageTimer = igniteDamageCoodlown;
        }
        
    }
    private void HitNearestTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (Collider2D hit in colliders)
        {
            Enemy enemy = Cache.GetEnemy(hit);
            if (enemy != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

            if (closestEnemy == null)            // delete if you don't want shocked target to be hit by shock strike
                closestEnemy = transform;
        }


        if (closestEnemy != null)
        {
            ShockStrike_Controller newShockStrike = SimplePool.Spawn<ShockStrike_Controller>(PoolType.Shock_Strike, transform.position, Quaternion.identity);
            newShockStrike.Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
            Debug.Log(closestEnemy.GetComponent<CharacterStats>().name);
        }
    }

    public void SetupIgniteDamange(int damage) => igniteDamage = damage;
    public void SetupShockStrikeDamage(int damage) => shockDamage = damage;

    #endregion

    #region Calculate Damage
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

        if(isShocked)
            totalEvasion += PLUS_POINT_SHOCKED;

        if(Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }

        return false;
    }

    private int CheckTargetArmor(CharacterStats characterStats, int totalDamage)
    {
        if(isChilled)
            totalDamage -= Mathf.RoundToInt(characterStats.armor.GetValue() * (1-REDUCE_ARMOR_CHILL));
        else
            totalDamage -= characterStats.armor.GetValue();

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    #endregion
}
