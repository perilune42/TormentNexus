using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class AbilityVFXLiveWire : AbilityVFX
{
    [SerializeField] float duration = 10f;
    [SerializeField] int amount = 3;
    [SerializeField] float wireDelay = 0.3f;
    [SerializeField] float maxWidth = 3f;
    [SerializeField] float wireExpandTime = 1f;
    [SerializeField] float wireDuration = 3f;

    [SerializeField] GameObject wireSegment;

    public override void Play()
    {
        StartCoroutine(SpawnWires());
        StartCoroutine(Destroy());
    }

    private IEnumerator SpawnWires()
    {
        for (int i = 0; i < amount; i++)
        {
            StartCoroutine(ExpandWire());
            yield return new WaitForSeconds(wireDelay);
        }
    }

    private IEnumerator ExpandWire()
    {
        SpriteRenderer wire = Instantiate(wireSegment, transform).GetComponent<SpriteRenderer>();
        wire.transform.Rotate(Vector3.forward, Random.Range(0, 360));

        float startTime = Time.time;
        float endTime = Time.time + wireExpandTime;
        float randWidth = Random.Range(-maxWidth * 0.1f, maxWidth * 0.1f);

        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / wireExpandTime;
            wire.size = Vector2.Lerp(new Vector2(0, wire.size.y), new Vector2(maxWidth + randWidth, wire.size.y), t);
            yield return null;
        }

        yield return new WaitForSeconds(wireDuration);

        StartCoroutine(CollapseWire(wire));
    }

    private IEnumerator CollapseWire(SpriteRenderer wire)
    {
        float startTime = Time.time;
        float endTime = Time.time + wireExpandTime;

        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / wireExpandTime;
            wire.size = Vector2.Lerp(new Vector2(maxWidth, wire.size.y), new Vector2(0, wire.size.y), t);
            yield return null;
        }

        Destroy(wire);
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(duration);
        StopAllCoroutines();
        Destroy(gameObject);
    }
}