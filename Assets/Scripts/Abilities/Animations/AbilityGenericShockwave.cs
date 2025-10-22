using System.Collections;
using UnityEngine;

public class AbilityGenericShockwave : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] SpriteMask shockwaveRingMask;
    [SerializeField] SpriteRenderer shockwaveRing;
    [SerializeField] float shockwaveGrowth;
    [SerializeField] float startRingScale;
    [SerializeField] float startMaskScale;
    [SerializeField] float maskGrowthFactor = 1.1f;

    private void Start()
    {
        shockwaveRingMask.transform.localScale = startMaskScale * Vector3.one;
        shockwaveRing.transform.localScale = startRingScale * Vector3.one;
        StartCoroutine(Destroy());
    }

    void Update()
    {
        shockwaveRing.transform.localScale += shockwaveGrowth * Time.deltaTime * Vector3.one;
        shockwaveRingMask.transform.localScale += maskGrowthFactor * shockwaveGrowth * Time.deltaTime * Vector3.one;
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(duration);
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
