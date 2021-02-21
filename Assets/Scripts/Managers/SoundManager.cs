using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    AudioSource defaultSource;
    [SerializeField] List<Sound> soundsList;
    Dictionary<string, AudioClip[]> soundsDico;

    #region Singleton

    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        soundsDico = new Dictionary<string, AudioClip[]>();
        for (int i = 0; i < soundsList.Count; i++)
        {
            soundsDico.Add(soundsList[i].name, soundsList[i].clips);
        }
        if (GameObject.FindGameObjectWithTag("DefaultSource") != null)
            defaultSource = GameObject.FindGameObjectWithTag("DefaultSource").GetComponent<AudioSource>();
    }

    #endregion

    public void PlaySound(AudioSource source, string soundName)
    {
        AudioClip[] clips;
        if (soundsDico.TryGetValue(soundName, out clips))
        {
            source.clip = clips[UnityEngine.Random.Range(0, clips.Length)];
            source.Play();
        }
        else
            throw new Exception("Le son au nom " + soundName + " n'existe pas dans le manager");
    }

    public void PlaySound(AudioSource source, AudioClip soundClip)
    {
        source.clip = soundClip;
        source.Play();
    }

    public void PlaySound(string soundName)
    {
        if (defaultSource == null)
            throw new MissingReferenceException("Il manque la source par défaut");

        AudioClip[] clips;
        if (soundsDico.TryGetValue(soundName, out clips))
        {
            defaultSource.clip = clips[UnityEngine.Random.Range(0, clips.Length)];
            defaultSource.Play();
        }
        else
            throw new Exception("Le son au nom " + soundName + " n'existe pas dans le manager");
    }

    [Serializable]
    public class Sound
    {
        public string name;
        public AudioClip[] clips;
    }
}
