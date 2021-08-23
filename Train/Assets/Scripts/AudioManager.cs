using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager AudioInstance;
    public List<AudioClip> Songs;
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
    }
    private void Update()
    {
    }

    public void Play()
    {
        source.Play();
        source.clip = Songs[i];
        source.Play();
        i = i == 1 ? 0 : 1;
    }
}
