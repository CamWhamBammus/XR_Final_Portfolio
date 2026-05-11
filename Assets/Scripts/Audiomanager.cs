using UnityEngine;

public class Audiomanager : MonoBehaviour
{
    public AudioClip BackgroundMusic;
    public AudioClip chicken_01;
    public AudioClip chicken_02;
    public AudioClip chicken_03;
    public AudioClip fox_01;
    public AudioClip fox_02;
    public AudioClip water_01;
    public AudioClip water_02;
    public AudioClip water_03;
    public AudioClip GameOverSound;

    public float bg_volume = 0.3f;
    public float sfx_volume = 0.5f;

    public static Audiomanager Instance;

    private AudioSource BackgroundMusicSource;
    private AudioSource EffectsMusicSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // play BackgroundMusic
        BackgroundMusicSource = gameObject.AddComponent<AudioSource>();
        BackgroundMusicSource.clip = BackgroundMusic;
        BackgroundMusicSource.loop = true;
        BackgroundMusicSource.volume = bg_volume;
        BackgroundMusicSource.Play();

        EffectsMusicSource = gameObject.AddComponent<AudioSource>();
        EffectsMusicSource.loop = false;
        EffectsMusicSource.volume = sfx_volume;

        Debug.Log("BG is: " + BackgroundMusic);
        Debug.Log("BG volume: " + bg_volume);
        Debug.Log("AudioManager has started");
    }

    public void StartBGmusic()
    {
        StopBGSound();
        BackgroundMusicSource.clip = BackgroundMusic;
        BackgroundMusicSource.Play();
    }

    public void PlayChickenSound()
    {
        AudioClip[] chickenClips = { chicken_01, chicken_02, chicken_03 };
        AudioClip randomClip = chickenClips[Random.Range(0, chickenClips.Length)];
        EffectsMusicSource.clip = randomClip;
        EffectsMusicSource.Play();
    }

    public void PlayWaterSound()
    {
        AudioClip[] waterClips = { water_01, water_02, water_03 };
        AudioClip randomClip = waterClips[Random.Range(0, waterClips.Length)];
        EffectsMusicSource.clip = randomClip;
        EffectsMusicSource.Play();
    }

    public void PlayFoxSound()
    {
        AudioClip[] foxClips = { fox_01, fox_02, };
        AudioClip randomClip = foxClips[Random.Range(0, foxClips.Length)];
        EffectsMusicSource.clip = randomClip;
        EffectsMusicSource.Play();
    }

    public void PlayGameOverSound()
    {
        StopBGSound();
        BackgroundMusicSource.clip = GameOverSound;
        BackgroundMusicSource.Play();
    }


    public void StopBGSound()
    {
        if (BackgroundMusicSource != null && BackgroundMusicSource.isPlaying)
        {
            BackgroundMusicSource.Stop();
        }
    }

}
