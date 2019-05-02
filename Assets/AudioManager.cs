using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip clickSound;
    public AudioClip GoldSound;
    public AudioClip digResultSound;
    public AudioClip upgradeSound;
    public AudioClip crashSound;

    public static AudioManager INSTANCE = null;
    public AudioSource sfxSource;

    private void Awake()
    {
        if (INSTANCE == null)
        {
            INSTANCE = this;
        }
        else if (INSTANCE != this)
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
    }

    public void Play(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }
}
