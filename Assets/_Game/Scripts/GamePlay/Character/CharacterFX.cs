using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFX : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] float flashDuration;
    [SerializeField] Material hitMat;
    Material originalMat;

    [Header("Ailment Color")]
    [SerializeField] Color[] chillColor;
    [SerializeField] Color[] shockColor;
    [SerializeField] Color[] igniteColor;

    void Start()
    {
        originalMat = sr.material;
    }

    public IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;
        sr.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        sr.color = currentColor;
        sr.material = originalMat;
    }

    public void RedColorBlink()
    {
        if(sr.color != Color.white)
        {
            sr.color = Color.white;
        }
        else
            sr.color = Color.red;
    }

    public void CanelChangeColor()
    {
        CancelInvoke();
        sr.color = Color.white;
    } 

    public void ShockFxFor(float second)
    {
        InvokeRepeating(nameof(ShockColorFX), 0, .3f);
        Invoke(nameof(CanelChangeColor), second);
    }

    public void ChillFxFor(float second)
    {
        InvokeRepeating(nameof(ChillColorFX), 0, .3f);
        Invoke(nameof(CanelChangeColor), second);
    }

    public void IgniteFxFor(float second)
    {
        InvokeRepeating(nameof(IgniteColorFX), 0, .3f);
        Invoke(nameof(CanelChangeColor), second);
    }

    private void IgniteColorFX()
    {
        if(sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];
    }

    private void ShockColorFX()
    {
        if(sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }

    private void ChillColorFX()
    {
        if(sr.color != chillColor[0])
            sr.color = chillColor[0];
        else
            sr.color = chillColor[1];
    }


}
