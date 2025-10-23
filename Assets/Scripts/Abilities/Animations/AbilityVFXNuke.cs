using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AbilityVFXNuke : AbilityVFX
{
    [Header("Timing")]
    [SerializeField] float nukeDuration;
    [SerializeField] float flashIntensity;
    [SerializeField] float flashFade;
    [SerializeField] float smokeGrowth;

    [Header("Refs")]
    [SerializeField] Light2D blastLight;
    [SerializeField] SpriteRenderer blast;
    [SerializeField] SpriteRenderer centerSmoke;
    [SerializeField] GameObject shockwave;

    public override void Play(Faction attacker)
    {
        base.Play(attacker);
        StartCoroutine(Blast());
        StartCoroutine(Smoke());
        shockwave.SetActive(true);
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

    private IEnumerator Smoke()
    {
        while (true)
        {
            centerSmoke.transform.localScale += smokeGrowth * Time.deltaTime * Vector3.one;
            centerSmoke.color -= Time.deltaTime * new Color(0, 0, 0, 0.1f); 

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