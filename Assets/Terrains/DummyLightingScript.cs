using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyLightingScript : MonoBehaviour {

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Rotate Sun and Moon around origin 
        transform.RotateAround(Vector3.zero, new Vector3(0, 0, 1), Mathf.PI * Time.deltaTime);
        transform.LookAt(Vector3.zero);
    }
}
