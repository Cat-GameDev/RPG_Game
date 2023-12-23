using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatText_UI : GameUnit
{
    [SerializeField] TextMeshProUGUI hpText;
    public void OnInit(float damage)
    {
        hpText.text = damage.ToString();
        Invoke(nameof(OnDespawn), 1f);
    }
}
