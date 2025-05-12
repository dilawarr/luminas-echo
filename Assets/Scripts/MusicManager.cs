using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip musicClip;

    [Range(0f, 1f)] public float volume = 0.5f;

    private void Awake()
    {
        // Singleton: ensure only one music manager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = musicClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = volume;
        audioSource.Play();
    }

    public void SetVolume(float newVolume)
    {
        volume = newVolume;
        audioSource.volume = volume;
    }
}
