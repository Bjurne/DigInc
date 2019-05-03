using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;

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

    public void MuteUnmuteSounds(Image buttonImage)
    {
        if (sfxSource.volume > 0.5f)
        {
            sfxSource.volume = 0f;
            buttonImage.sprite = Resources.Load<Sprite>("Sprites/SoundMutedSprite01");
        }
        else
        {
            sfxSource.volume = 1f;
            buttonImage.sprite = Resources.Load<Sprite>("Sprites/SoundOnSprite01");
        }
    }
}
