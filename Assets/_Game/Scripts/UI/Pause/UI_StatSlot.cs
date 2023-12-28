using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] StatType statType;
    [SerializeField] TextMeshProUGUI statValueText;

    [SerializeField] UI_StatToolTip statToolTip;

    [TextArea]
    [SerializeField] string statDescription;

    void Start()
    {
        UpdateStatValue();
    }

    internal void UpdateStatValue()
    {
        PlayerStats playerStats = LevelManager.Instance.Player.characterStats as PlayerStats;

        if(playerStats != null)
        {
            statValueText.SetText(playerStats.GetStat(statType).GetValue().ToString());
        }

        if (statType == StatType.health)
                statValueText.text = playerStats.GetMaxHealthValue().ToString();

            if (statType == StatType.damage)
                statValueText.text = (playerStats.damage.GetValue() + playerStats.strength.GetValue()).ToString();

            if (statType == StatType.critPower)
                statValueText.text = (playerStats.critPower.GetValue() + playerStats.strength.GetValue()).ToString();

            if(statType == StatType.critChance)
                statValueText.text = (playerStats.critChance.GetValue() + playerStats.agility.GetValue()).ToString();

            if (statType == StatType.evasion)
                statValueText.text = (playerStats.evasion.GetValue() + playerStats.agility.GetValue()).ToString();

            if (statType == StatType.magicRes)
                statValueText.text = (playerStats.magicResistance.GetValue() + (playerStats.intelligence.GetValue() * 3)).ToString();
        

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        statToolTip?.ShowStatToolTip(statDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        statToolTip?.HideStatToolTip();
    }


}