using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AbilityVFXBlackHole : AbilityVFX
{
    [Header("Timing")]
    [SerializeField] float duration = 3f;
    [SerializeField] float startSize = 0.75f;
    [SerializeField] float waveFactor = 0.25f;
    [SerializeField] float waveSpeed = 1;
    [SerializeField] SpriteRenderer blast;

    public override void Play(Faction attacker)
    {
        base.Play(attacker);
        StartCoroutine(Blast());
        StartCoroutine(Destroy());
    }
    
    private IEnumerator Blast()
    {
        Vector3 startingScale = Vector3.one * startSize;
        while (true)
        {
            blast.transform.localScale = startingScale + Mathf.Sin(Time.time * waveSpeed) * waveFactor * Vector3.one;
            yield return null;
        }
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(duration);
        StopAllCoroutines();
        Destroy(gameObject);
    }
}