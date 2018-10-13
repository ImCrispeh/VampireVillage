using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapScript : MonoBehaviour {

    public Camera miniMapCamera;
    public Camera mainCamera;
    public LayerMask layer;
    public bool mouseEntered { get; set; }
	
	// Update is called once per frame
	void Update () {
        if (mouseEntered)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = miniMapCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
                {
                    Vector3 miniMapPosition = hit.point;
                    Vector3 camViewCenter;
                    RaycastHit cameraView;
                    Vector3 camDestPos;

                    Ray cameraCenter = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
                    Physics.Raycast(cameraCenter, out cameraView, Mathf.Infinity, layer);
                    camViewCenter = cameraView.point;

                    camDestPos = miniMapPosition - camViewCenter;
                    camDestPos.y = 0;
                    mainCamera.transform.position += camDestPos;
                }
            }
        }
	}
}
