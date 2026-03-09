using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Sounds")]
    [SerializeField] private AudioClip backgroundSound;
    [SerializeField] private Sound[] sfxSounds;

    [Header("Sources")]
    [SerializeField] private AudioSource backgroundSource;
    [SerializeField] private AudioSource sfxSource;

    private Dictionary<string, Sound> sfxDictionary;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        PlayBackgroundSound();

        DontDestroyOnLoad(gameObject);

        CreateDictionary();                                         //creo il dictionary in awake
    }

    private void CreateDictionary()
    {
        sfxDictionary = new Dictionary<string, Sound>();

        foreach (Sound sound in sfxSounds)                          //aggiungo ogni suono dell'array al dizionario
            sfxDictionary.Add(sound.soundName, sound);
    }

    public void PlaySFXSound(string name)
    {
        if (sfxDictionary == null)
            return;

        if (!sfxDictionary.ContainsKey(name))                       //controllo se il suono e' presente nel dizionario
            return;

        sfxSource.PlayOneShot(sfxDictionary[name].audioClip);       //se lo trovo lo riproduco oneshot
    }

    private void PlayBackgroundSound()
    {
        if (backgroundSound != null)
        {
            backgroundSource.clip = backgroundSound;
            backgroundSource.loop = true;
            backgroundSource.Play();
        }
    }
}