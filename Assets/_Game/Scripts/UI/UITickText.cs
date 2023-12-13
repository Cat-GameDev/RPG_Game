using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITickText : GameUnit
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] RectTransform rectTransform;

    internal void SetupText(Vector3 textTransform, Vector2 sizeUI)
    {
        text.transform.position = textTransform;
        rectTransform.sizeDelta = sizeUI;
    }
}
