using UnityEngine;

public class MultiAudioSourcePlayer : MonoBehaviour
{
    public static MultiAudioSourcePlayer Instance { get; private set; }

    [SerializeField] private int audioSourceCount = 10; // Кількість джерел звуку
    private AudioSource[] audioSources;

    private void Awake()
    {
        // Сінглтон
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitAudioSources();
    }

    private void InitAudioSources()
    {
        audioSources = new AudioSource[audioSourceCount];
        for (int i = 0; i < audioSourceCount; i++)
        {
            AudioSource src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            audioSources[i] = src;
        }
    }


    public static void PlaySound(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (Instance == null || clip == null) return;

        foreach (var source in Instance.audioSources)
        {
            if (!source.isPlaying)
            {
                source.clip = clip;
                source.volume = Mathf.Clamp01(volume);
                source.pitch = pitch;
                source.Play();
                return;
            }
        }
    }


    public void StopAllSounds()
    {
        foreach (var source in audioSources)
        {
            source.Stop();
        }
    }
}
