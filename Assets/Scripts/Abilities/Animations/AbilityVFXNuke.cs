using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AbilityVFXNuke : AbilityVFX
{
    [Header("Timing")]
    [SerializeField] float nukeDuration;
    [SerializeField] float flashIntensity;
    [SerializeField] float flashFade;
    [SerializeField] float smokeGrowth;
    [SerializeField] float shockwaveGrowth;

    [Header("Refs")]
    [SerializeField] Light2D blastLight;
    [SerializeField] SpriteRenderer blast;
    [SerializeField] SpriteRenderer centerSmoke;
    [SerializeField] SpriteRenderer shockwaveRing;
    [SerializeField] SpriteMask shockwaveRingMask;

    public override void Play()
    {
        StartCoroutine(Blast());
        StartCoroutine(SmokeAndShockwave());
        StartCoroutine(Destroy());
    }
    
    private IEnumerator Blast()
    {
        blastLight.enabled = true;

        float startTime = Time.time;
        float endTime = Time.time + flashFade;
        while (Time.time < endTime)
        {
            float time = (Time.time - startTime) / flashFade;
            blastLight.intensity = Mathf.Lerp(flashIntensity, 0.0f, time);
            blast.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), time);
            yield return null;
        }
    }

    private IEnumerator SmokeAndShockwave()
    {
        while (true)
        {
            centerSmoke.transform.localScale += smokeGrowth * Time.deltaTime * Vector3.one;
            centerSmoke.color -= Time.deltaTime * new Color(0, 0, 0, 0.1f); 

            shockwaveRing.transform.localScale += shockwaveGrowth * Time.deltaTime * Vector3.one;
            shockwaveRingMask.transform.localScale += 1.1f * shockwaveGrowth * Time.deltaTime * Vector3.one;
            yield return null;
        }
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(nukeDuration);
        StopAllCoroutines();
        Destroy(gameObject);
    }
}