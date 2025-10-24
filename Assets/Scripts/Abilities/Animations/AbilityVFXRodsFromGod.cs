using System.Collections;
using UnityEngine;

public class AbilityVFXRodsFromGod : AbilityVFX
{
    [SerializeField] float duration = 10f;

    [SerializeField] GameObject rod;
    [SerializeField] float startScale;
    [SerializeField] float endScale;
    [SerializeField] float dropTime;
    [SerializeField] GameObject shockwave;
    [SerializeField] float shockwaveScale;

    public override void Play(Faction attacker)
    {
        base.Play(attacker);
        rod.transform.localScale = Vector3.one * startScale;
        StartCoroutine(Drop());
        StartCoroutine(Destroy());
    }

    private IEnumerator Drop()
    {
        float startTime = Time.time;
        float endTime = Time.time + dropTime;
        while (Time.time < endTime)
        {
            float time = (Time.time - startTime) / dropTime;
            rod.transform.localScale = Vector3.one * Mathf.Lerp(startScale, endScale, time);
            yield return null;
        }

        shockwave.SetActive(true);
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(duration);
        StopAllCoroutines();
        Destroy(gameObject);
    }
}