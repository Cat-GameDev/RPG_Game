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

    void Start()
    {
        originalMat = sr.material;
    }

    public IEnumerator FlashFX()
    {
        sr.material = hitMat;

        yield return new WaitForSeconds(flashDuration);

        sr.material = originalMat;
    }
}
