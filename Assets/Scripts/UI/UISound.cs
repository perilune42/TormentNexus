using UnityEngine;
using UnityEngine.EventSystems;

public class UISound : MonoBehaviour, IPointerClickHandler, ISubmitHandler
{
    [SerializeField] AudioSource sfx;          // drag your shared UIAudio AudioSource here
    [SerializeField] AudioClip hoverClip;      // hover SFX
    [SerializeField] AudioClip clickClip;      // click/submit SFX
    [SerializeField] float hoverVol = 1f;
    [SerializeField] float clickVol = 1f;

    // ---- global cooldown state (shared across ALL nodes) ----
    private const float GlobalCooldown = 0.5f;            // 1 second global cooldown
    private static float s_lastSoundTime = -10f;           // when any UI sound last played
    private static int   s_lastNodeId = int.MinValue;      // which node played it
    private static bool  s_lastWasHover = false;           // was the last sound from hover?

    int _id;

    void Awake()  { _id = GetInstanceID(); }
    void Reset()  { if (!sfx) sfx = GameObject.Find("UIAudio")?.GetComponent<AudioSource>(); }

    // ---------------- events ----------------

    public void OnPointerClick(PointerEventData e)
    {
        TryPlayClick();
    }

    public void OnSubmit(BaseEventData e)
    {
        TryPlayClick();
    }

    // ---------------- helpers ----------------
    void TryPlayClick()
    {
        float dt = Time.unscaledTime - s_lastSoundTime;

        // case A: outside cooldown → always play
        if (dt >= GlobalCooldown)
        {
            Play(clickClip, clickVol);
            s_lastSoundTime = Time.unscaledTime;
            s_lastNodeId    = _id;
            s_lastWasHover  = false;
            return;
        }

        // case B: inside cooldown, but the last sound was a HOVER from THIS SAME NODE
        // → allow the click to play (the exception you asked for)
        if (s_lastWasHover && s_lastNodeId == _id)
        {
            Play(clickClip, clickVol);
            s_lastSoundTime = Time.unscaledTime;  // reset cooldown from the click
            s_lastNodeId    = _id;
            s_lastWasHover  = false;
            return;
        }

        // else: inside cooldown triggered by another node or by a previous click → suppress
    }

    void Play(AudioClip clip, float vol)
    {
        if (clip && sfx)
            sfx.PlayOneShot(clip, vol);
    }
}
