using UnityEngine;

public class BgMusicController : MonoBehaviour
{
    public static BgMusicController Instance { get; private set; }

    private AudioSource audioSource;
    private AudioClip currentClip;
    private AudioClip previousClip;

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

        // Ініціалізація AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    public void PlayMusic(AudioClip newClip)
    {
        if (newClip == null || newClip == currentClip)
            return;

        // Зберігаємо попередню музику
        previousClip = currentClip;
        currentClip = newClip;

        audioSource.clip = currentClip;
        audioSource.Play();
    }

    public void PlayPrevious()
    {
        if (previousClip != null)
        {
            // Поміняти місцями current <-> previous
            AudioClip temp = currentClip;
            currentClip = previousClip;
            previousClip = temp;

            audioSource.clip = currentClip;
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        audioSource.Stop();
        currentClip = null;
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp01(volume);
    }

    public AudioClip GetCurrentClip() => currentClip;
    public AudioClip GetPreviousClip() => previousClip;
}
