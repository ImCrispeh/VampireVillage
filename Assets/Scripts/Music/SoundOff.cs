using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOff : MonoBehaviour {

    private bool isNight = true;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("SoundOnTrigger");

        if (other.name == "Moon")
        {
            isNight = false;
            SoundManager.instance.StopMusic(isNight);
        }
        if (other.name == "Sun")
        {
            isNight = true;
            SoundManager.instance.StopMusic(isNight);
        }
    }
}
