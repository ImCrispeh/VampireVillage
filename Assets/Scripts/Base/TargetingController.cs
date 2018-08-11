using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingController : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Enemy") {
            BaseController._instance.enemiesInRange.Add(other.gameObject);
        }
    }
}
