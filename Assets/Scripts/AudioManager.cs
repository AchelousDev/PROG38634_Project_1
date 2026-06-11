using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip spawnSound;
    public AudioClip speedBoostSound;
    public AudioClip teleportSound;
    public AudioClip finishSound;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    private void PlayBackgroundMusic()
    {
        if (musicSource == null || backgroundMusic == null)
        {
            return;
        }

        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySpawnSound()
    {
        PlaySFX(spawnSound);
    }

    public void PlaySpeedBoostSound()
    {
        PlaySFX(speedBoostSound);
    }

    public void PlayTeleportSound()
    {
        PlaySFX(teleportSound);
    }

    public void PlayFinishSound()
    {
        PlaySFX(finishSound);
    }

    private void PlaySFX(AudioClip clip)
    {
        if (sfxSource == null || clip == null)
        {
            return;
        }

        sfxSource.PlayOneShot(clip);
    }
}