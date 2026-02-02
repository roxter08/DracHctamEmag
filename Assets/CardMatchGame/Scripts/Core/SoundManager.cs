using UnityEngine;

public class SoundManager
{
    private AudioSource[] audioSources;
    private static SoundManager instance;

    public static SoundManager GetInstance()
    {
        if (instance == null)
        {
            instance = new SoundManager();
        }
        return instance;
    }

    private SoundManager() 
    {
        //Singleton constructor
    }
    public void Initialize(AudioSource[] audioSources) 
    {
        this.audioSources = audioSources;
    }

    public void PlayAudio(AudioClip clip)
    {
        for(int i=0;i<audioSources.Length; i++) 
        {
            if (!audioSources[i].isPlaying)
            {
                audioSources[i].clip = clip;
                audioSources[i].Play();
                break;
            }
        }
    }
}
