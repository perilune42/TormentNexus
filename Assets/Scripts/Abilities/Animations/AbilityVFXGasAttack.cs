using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class AbilityVFXGasAttack : AbilityVFX
{
    [SerializeField] float duration = 10f;
    [SerializeField] float maxRadius = 1f;
    [SerializeField] float growthDecayTime = 1f;
    [SerializeField] float holdTime = 2f;
    [SerializeField] Color startColor;
    [SerializeField] Color endColor;
    [SerializeField] float noiseIntensity = 0.1f;
    [SerializeField] float noiseSpeed = 0.25f;
    [SerializeField] float noiseScalingPosition = 2f;
    [SerializeField] float noiseScalingScale = 1f;
    [SerializeField] int amount = 3;

    [SerializeField] GameObject gasCloud;

    private Vector3 startPos;

    public override void Play()
    {
        startPos = gasCloud.transform.position;

        for (int i = 0; i < amount; i++)
        {
            GameObject cloud = Instantiate(gasCloud, transform);
            cloud.transform.localScale = Vector3.one * maxRadius;
            StartCoroutine(FadeIn(cloud));
            StartCoroutine(Noiseify(cloud));
            StartCoroutine(Destroy());
        }

        Destroy(gasCloud);
    }

    private IEnumerator Noiseify(GameObject cloud)
    {
        float[] rand = { Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100) };

        while (true)
        {
            float noiseX = (1 - Mathf.PerlinNoise(Time.time * noiseSpeed + rand[0], Time.time * noiseSpeed + rand[1]) * 2) * noiseIntensity;
            float noiseY = (1 - Mathf.PerlinNoise(Time.time * noiseSpeed + rand[2], Time.time * noiseSpeed + rand[3]) * 2) * noiseIntensity;
            cloud.transform.position = startPos + new Vector3(noiseX, noiseY, 0) * noiseScalingPosition;
            cloud.transform.localScale = maxRadius * Vector3.one + noiseScalingScale * noiseX * Vector3.one;
            yield return null;
        }
    }

    private IEnumerator FadeIn(GameObject cloud)
    {
        float startTime = Time.time;
        float endTime = Time.time + growthDecayTime;

        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / growthDecayTime;
            cloud.GetComponent<SpriteRenderer>().color = Color.Lerp(endColor, startColor, t);
            yield return null;
        }

        yield return new WaitForSeconds(holdTime);

        StartCoroutine(FadeOut(cloud));
    }

    private IEnumerator FadeOut(GameObject cloud)
    {
        float startTime = Time.time;
        float endTime = Time.time + growthDecayTime;

        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / growthDecayTime;
            cloud.GetComponent<SpriteRenderer>().color = Color.Lerp(startColor, endColor, t);
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