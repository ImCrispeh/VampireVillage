using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class SoundOn : MonoBehaviour
{
    public AudioClip nightTrack1;
    public AudioClip nightTrack2;
    public AudioClip nightTrack3;
    public AudioClip dayTrack1;
    public AudioClip dayTrack2;
    public AudioClip dayTrack3;
    public AudioMixer mixer;
    public AudioMixerSnapshot[] snapshots;
    public float[] weights;
    public bool isNight = true;

    // public static  bool isItNight(){
    //     return isNight;
    // }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("SoundOnTrigger");

        if (other.name == "Moon")
        {
            isNight = true;
            SoundManager.instance.RandomMusic(isNight, nightTrack1, nightTrack2);
            weights[0] = 1.0f;
            weights[1] = 0.0f;
            mixer.TransitionToSnapshots(snapshots, weights, 0.5f);
        }
        if (other.name == "Sun")
        {
            isNight = false;
            //add any tracks as a parameter (ie RandomMusic(isNight, dayTrack1, dayTrack2))
            SoundManager.instance.RandomMusic(isNight, dayTrack1, dayTrack2);
            weights[0] = 0.0f;
            weights[1] = 1.0f;
            mixer.TransitionToSnapshots(snapshots, weights, 0.5f);

        }
    }
}
