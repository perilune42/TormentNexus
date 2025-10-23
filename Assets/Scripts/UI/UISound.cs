using UnityEngine;
using UnityEngine.EventSystems;

public class UISound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, ISubmitHandler
{
    [SerializeField] AudioSource sfx;          // drag UIAudio AudioSource here
    [SerializeField] AudioClip hoverClip;      // assign hover .wav
    [SerializeField] AudioClip clickClip;      // assign click .wav
    [SerializeField] float hoverVol = 1f;
    [SerializeField] float clickVol = 1f;

    private const float cooldownTime = 0.6f;     // ⏱ global 1-second cooldown
    private static float lastSoundTime = -10f; // shared across ALL UISound instances

    void Reset()
    {
        if (!sfx)
            sfx = GameObject.Find("UIAudio")?.GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData e)
    {
        // prevent hover spam across all UI elements
        if (Time.unscaledTime - lastSoundTime >= cooldownTime)
        {
            Play(hoverClip, hoverVol);
            lastSoundTime = Time.unscaledTime;
        }
    }

    public void OnPointerClick(PointerEventData e)
    {
        // still respects the same cooldown
        if (Time.unscaledTime - lastSoundTime >= cooldownTime)
        {
            Play(clickClip, clickVol);
            lastSoundTime = Time.unscaledTime;
        }
    }

    public void OnSubmit(BaseEventData e)
    {
        if (Time.unscaledTime - lastSoundTime >= cooldownTime)
        {
            Play(clickClip, clickVol);
            lastSoundTime = Time.unscaledTime;
        }
    }

    void Play(AudioClip clip, float vol)
    {
        if (clip && sfx)
            sfx.PlayOneShot(clip, vol);
    }
}
