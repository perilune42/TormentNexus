using UnityEngine;
using UnityEngine.EventSystems;

public class UISound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, ISubmitHandler
{
    [SerializeField] AudioSource sfx;          // drag UIAudio AudioSource here
    [SerializeField] AudioClip hoverClip;      // assign hover .wav
    [SerializeField] AudioClip clickClip;      // assign click .wav
    [SerializeField] float hoverVol = 1f;
    [SerializeField] float clickVol = 1f;

    void Reset() {
        // convenience: try to find a UIAudio in scene
        if (!sfx) sfx = GameObject.Find("UIAudio")?.GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData e) { Play(hoverClip, hoverVol); }
    public void OnPointerClick(PointerEventData e) { Play(clickClip, clickVol); }
    public void OnSubmit(BaseEventData e)          { Play(clickClip, clickVol); } // keyboard/Pad "submit"

    void Play(AudioClip clip, float vol) {
        if (clip && sfx) sfx.PlayOneShot(clip, vol);
    }
}
