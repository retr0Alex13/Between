using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource _soundSource;
    [SerializeField] private AudioSource _musicSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(AudioClip audioClip, float volume = 1f)
    {
        _soundSource.PlayOneShot(audioClip, volume);
    }

    public void PlaySoundAtPoint(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    public void PlayMusic(AudioClip audioClip, float volume = 1f)
    {
        _musicSource.clip = audioClip;
        _musicSource.volume = volume;
        _musicSource.Play();
    }
}