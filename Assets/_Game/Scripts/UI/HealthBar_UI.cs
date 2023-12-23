using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : GameUnit
{
    [SerializeField] Slider slider;
    float hp;
    float maxHp;
    Transform target;
    Vector3 offset;

    void Update()
    {
        slider.value = Mathf.Lerp(slider.value, hp / maxHp, Time.deltaTime*5f);
        transform.position = target.position + offset;
    }

    public void OnInit(float maxHp, Transform target,Vector3 offset)
    {
        this.offset = offset;
        this.target = target;
        this.maxHp = maxHp;
        hp = maxHp;
        slider.value = 1;
    }

    public void SetNewHp(float hp)
    {
        this.hp = hp;
    }
}
