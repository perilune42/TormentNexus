using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AbilityGenericBomb : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] float lightIntensity;
    [SerializeField] Color startColor;
    [SerializeField] Color endColor;

    [SerializeField] SpriteRenderer explosion;
    [SerializeField] Light2D flash;

    private float startTime;
    private float endTime;

    void Start()
    {
        startTime = Time.time;
        endTime = Time.time + duration;
    }

    void Update()
    {
        float t = (Time.time - startTime) / duration;

        explosion.color = Color.Lerp(startColor, endColor, t);
        flash.intensity = Mathf.Lerp(lightIntensity, 0, t);

        if (t > endTime)
        {
            Destroy(gameObject);
        }
    }
}
