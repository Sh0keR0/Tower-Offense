using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {


    public static SoundManager current;

    public float soundDelay = 0.15f;

    private float cooldown = 0;

    private AudioSource audio;

    public Dictionary<string, AudioClip> Sounds;

    public bool PlayBackgroundMusic = true;
    private AudioSource backgroundMusic;


	void Awake () {

        current = this;

        audio = GetComponent<AudioSource>();
        if(audio == null)
        {
            audio = gameObject.AddComponent<AudioSource>();
        }

        Sounds = new Dictionary<string, AudioClip>();

        var s = Resources.LoadAll<AudioClip>("Sounds/");
        foreach(AudioClip a in s)
        {
            Sounds.Add(a.name, a);
        }

        Debug.Log(Sounds.Count + " sounds loaded");

        if(PlayBackgroundMusic)
        {
            backgroundMusic = gameObject.AddComponent<AudioSource>();
            backgroundMusic.clip = Sounds["background_"];
            backgroundMusic.loop = true;
            backgroundMusic.volume = 0.2f;
            backgroundMusic.Play();
        }


	}
	

    public void PlaySound(string soundName)
    {
        if(cooldown > 0)
        {
            return;
        }
        audio.clip = Sounds[soundName];
        audio.Play();
        cooldown = soundDelay;
    }


    void Update()
    {
        cooldown -= Time.unscaledDeltaTime;
    }

    public void ToggleBackgroundMusic()
    {
        if (backgroundMusic.volume == 0f)
        {
            backgroundMusic.volume = 0.2f;
        }
        else
        {
             backgroundMusic.volume = 0f;
        }
   
    }



}
