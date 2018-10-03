using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class VolumeSlider : MonoBehaviour {
	public AudioMixer mixer;

    // Use this for initialization
    public void SetMusicVolume(float volume){

		mixer.SetFloat("MasterVolume", volume);
	}

}
