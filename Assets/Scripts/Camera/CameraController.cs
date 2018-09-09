﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float panSpeed = 10f;
    private float panBorderTrigger = 10f;

    public Vector2 panLimit;

    private float scrollSpeed = 20f;
    public float scrollMin;
    public float scrollMax;

    public Vector3 orginalCameraPos;

    private void Start()
    {
        orginalCameraPos = Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update () {

        Vector3 pos = transform.position;

        if(Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y >= Screen.height - panBorderTrigger)
        {
            pos.z += panSpeed * Time.deltaTime;
        }

        else if (Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y <= panBorderTrigger)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }

        else if (Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width - panBorderTrigger)
        {
            pos.x += panSpeed * Time.deltaTime;
        }

        else if (Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x <= panBorderTrigger)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        pos.y -= scroll * scrollSpeed * 50f * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, orginalCameraPos.x - panLimit.x, orginalCameraPos.x + panLimit.x);
        pos.y = Mathf.Clamp(pos.y, scrollMin, scrollMax);
        pos.z = Mathf.Clamp(pos.z, orginalCameraPos.z - panLimit.y, orginalCameraPos.z + panLimit.y);

        transform.position = pos;   

        if (Input.GetKeyDown(KeyCode.F1))
        {
            transform.position = orginalCameraPos;
        }
    }
}
