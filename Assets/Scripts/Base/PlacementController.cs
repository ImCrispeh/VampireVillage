using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementController : MonoBehaviour {

    [SerializeField]
    private GameObject placeableObject;

    [SerializeField]
    private KeyCode hotKey = KeyCode.E;

    private GameObject currentObject;

    private bool isPlaceable;

    // Update is called once per frame
    void Update()
    {
        HandleObject();

        if (currentObject != null)
        {
            MovePlaceableObject();
            PlaceObject();
        }
    }

    private void PlaceObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentObject = null;
        }
    }

    private void MovePlaceableObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            currentObject.transform.position = hitInfo.point;
            currentObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }

    }

    private void HandleObject()
    {
        if (Input.GetKeyDown(hotKey))
        {
            if (currentObject == null)
            {
                currentObject = Instantiate(placeableObject);
            }
            else
            {
                Destroy(currentObject);
            }
        }
    }
}
