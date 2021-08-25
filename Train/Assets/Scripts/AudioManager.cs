using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager AudioInstance;
    private AudioSource source;
    int i = 1;
    private void Awake()
    {
        if (AudioInstance != null && AudioInstance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        AudioInstance = this;
        DontDestroyOnLoad(this);
        source = GetComponent<AudioSource>();
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.Loop;

        }
    }

    private void Start()
    {
        Play("Theme");
    }

    public void Play(string name)
    {
        Sound s =  Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning($"Sound: {name} not found!");
            return;
        }
        s.source.Play();
    }
    public void Stop(string name)
    {
        Sound s =  Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning($"Sound: {name} not found!");
            return;
        }
        s.source.Stop();
    }
}
