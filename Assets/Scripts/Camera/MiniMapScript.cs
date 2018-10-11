using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapScript : MonoBehaviour {

    public Camera miniMapCamera;
    public Camera mainCamera;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = miniMapCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                
            }

            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, hit.point, 0.1f);
        }
	}
}
