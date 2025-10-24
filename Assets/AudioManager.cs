using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSource;
    public AudioClip hardClick;
    public AudioClip softClick;
    public AudioClip UIAccept;
    public AudioClip UIDecline;
    public AudioClip ExplosionHeavyThud;
    public AudioClip ExplosionLaser;
    public AudioClip ExplosionDisintegrate;
    public AudioClip ExplosionChargeBlast;
    public AudioClip CoinsSFX;
    public AudioClip MapNodeClick;

    public void Awake()
    {
        instance = this;
    }
    public void Play(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
