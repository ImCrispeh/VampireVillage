using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitCameraController : MonoBehaviour {
    public static PortraitCameraController _instance;
    public Transform target;
    public Vector3 offset;
    public bool following;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        }
        else {
            _instance = this;
        }
    }

    void Start () {
        offset = new Vector3(0, 4, 0);
        following = false;
	}
	
	public void Update () {
        if (following) {
            SetFollow(target);
        }
	}

    //Sets the size value in the inspector, smaller size means more zoomed in, larger size means more zoomed out
    public void SetZoom(float zoom) {
        GetComponent<Camera>().orthographicSize = zoom;
    }

    public void SetPosition(Transform target) {
        transform.position = target.position + offset;
        transform.LookAt(target);        
    }

    public void SetFollow(Transform target) {
        if (target != null) {
            transform.position = target.position + offset;
            transform.LookAt(target);
        }
    }
}
