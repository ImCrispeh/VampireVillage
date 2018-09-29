using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public static CameraController _instance;

    public int timeSpeed = 1;

    public float panSpeed = 10f;
    private float panBorderTrigger = 10f;

    public Vector2 panLimit;

    private float scrollSpeed = 20f;
    public float scrollMin;
    public float scrollMax;

    public Vector3 orginalCameraPos;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    private void Start()
    {
        orginalCameraPos = Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update () {

        Vector3 pos = transform.position;

        if(Input.GetAxisRaw("Vertical") > 0 || Input.mousePosition.y >= Screen.height - panBorderTrigger)
        {
            pos.z += panSpeed * Time.unscaledDeltaTime * timeSpeed;
        }

        else if (Input.GetAxisRaw("Vertical") < 0 || Input.mousePosition.y <= panBorderTrigger)
        {
            pos.z -= panSpeed * Time.unscaledDeltaTime * timeSpeed;
        }

        else if (Input.GetAxisRaw("Horizontal") > 0 || Input.mousePosition.x >= Screen.width - panBorderTrigger)
        {
            pos.x += panSpeed * Time.unscaledDeltaTime * timeSpeed;
        }

        else if (Input.GetAxisRaw("Horizontal") < 0 || Input.mousePosition.x <= panBorderTrigger)
        {
            pos.x -= panSpeed * Time.unscaledDeltaTime * timeSpeed;
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

    public void PauseCamera() {
        timeSpeed = 0;
    }

    public void UnpauseCamera() {
        timeSpeed = 1;
    }
}
