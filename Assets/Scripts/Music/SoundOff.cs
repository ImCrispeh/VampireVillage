using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOff : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("SoundOnTrigger");

        if (other.name == "Moon" || other.name == "Sun")
        {
            SoundManager.instance.StopMusic();
        }
    }
}
