using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource dayMusic;
    public AudioSource nightMusic;
    public AudioSource taskCompleted;
    public AudioSource hunger;

    public float speed;
    public float maxVolume;

    public AudioSource sfxSource;
    public static SoundManager instance = null;
    public float lowPitchRange = .95f;              
    public float highPitchRange = 1.05f;
    
    public AudioClip firstNight;
    public AudioClip techComplete;
    //public GameObject sun;
    //public GameObject moon;
    //public List<AudioClip> nightTracks;
    //public List<AudioClip> dayTracks;
    void Start(){
        RandomMusic(true, firstNight);
    }

    void Awake()
    {
        if (instance == null) {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlaySingle(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    public void RandomMusic(bool isNight, params AudioClip[] clips)
    {
        //Generate a random number between 0 and the length of our array of clips passed in.
        int randomIndex = Random.Range(0, clips.Length);

        //Set the clip to the clip at our randomly chosen index.
        if (isNight)
        {
            nightMusic.clip = clips[randomIndex];
            //Play the clip.
            //nightMusic.Play();
            StartCoroutine(FadeIn(nightMusic, 1.0f));
        }
        else if (!isNight) {
            dayMusic.clip = clips[randomIndex];

            //Play the clip.
            //dayMusic.Play();
            StartCoroutine(FadeIn(dayMusic, 1.0f));
        }

    }
    public void RandomizeSfx(params AudioClip[] clips)
    {
        //Generate a random number between 0 and the length of our array of clips passed in.
        int randomIndex = Random.Range(0, clips.Length);

        //Choose a random pitch to play back our clip at between our high and low pitch ranges.
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        //Set the pitch of the audio source to the randomly chosen pitch.
        sfxSource.pitch = randomPitch;

        sfxSource.clip = clips[randomIndex];

        //Play the clip.
        sfxSource.Play();
    }
    public void HungerBeat(params AudioClip[] clips)
    {
        //Generate a random number between 0 and the length of our array of clips passed in.
        int randomIndex = Random.Range(0, clips.Length);

        //Choose a random pitch to play back our clip at between our high and low pitch ranges.
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        //Set the pitch of the audio source to the randomly chosen pitch.
        hunger.pitch = randomPitch;

        hunger.clip = clips[randomIndex];

        //Play the clip.
        hunger.Play();
    }

    public void StopHunger(AudioClip clip){
        hunger.clip = clip;
        hunger.Stop();
    }

    public void TaskCompleted(params AudioClip[] clips)
    {
        //Generate a random number between 0 and the length of our array of clips passed in.
        int randomIndex = Random.Range(0, clips.Length);

        //Choose a random pitch to play back our clip at between our high and low pitch ranges.
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        //Set the pitch of the audio source to the randomly chosen pitch.
        taskCompleted.pitch = randomPitch;

        //Set the clip to the clip at our randomly chosen index.
        taskCompleted.clip = clips[randomIndex];

        //Play the clip.
        taskCompleted.Play();
    }

    public void StopMusic(bool isNight) {
        if (isNight)
        {
            StartCoroutine(FadeOut(dayMusic, 2.0f));
            //dayMusic.Stop();
        } else if (!isNight)
        {
            StartCoroutine(FadeOut(nightMusic, 2.0f));
            //nightMusic.Stop();
        }
        
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        float startVolume = 0.2f;

        audioSource.volume = 0;
        audioSource.Play();

        while (audioSource.volume < 1.0f)
        {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.volume = 1f;
    }


    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    public void TechComplete(){
        TaskCompleted(techComplete);
    }
}