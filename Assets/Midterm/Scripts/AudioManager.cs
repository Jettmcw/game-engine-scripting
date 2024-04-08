using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip[] sounds;

    private static AudioClip[] staticSounds;
    private static AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        staticSounds = sounds;
    }

    public static void Play(int soundIndex)
    {
        if (soundIndex >= staticSounds.Length || soundIndex < 0) return;
        audioSource.PlayOneShot(staticSounds[soundIndex]);
    }
}
