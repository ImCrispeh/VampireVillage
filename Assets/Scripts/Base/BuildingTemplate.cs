using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTemplate : MonoBehaviour {

    [SerializeField]
    private GameObject finalBuilding;

    private Vector3 mousePosition;

    [SerializeField]
    private LayerMask tilesLayer;

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(Mathf.Round(mousePosition.x), -10, Mathf.Round(mousePosition.z));

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseRay = Camera.main.ScreenToWorldPoint(transform.position);
            RaycastHit2D rayHit = Physics2D.Raycast(transform.position, Vector3.zero, Mathf.Infinity, tilesLayer);

            if (rayHit.collider == null)
            {
                Instantiate(finalBuilding, transform.position, Quaternion.identity);
            }
        }
    }
}
