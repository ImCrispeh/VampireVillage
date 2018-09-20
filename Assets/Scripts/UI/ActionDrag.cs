using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionDrag : MonoBehaviour, IDragHandler, IEndDragHandler {
    public Canvas canvas;

    private void Start() {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    public void OnDrag(PointerEventData eventData) {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
        transform.position = canvas.transform.TransformPoint(pos);
    }

    public void OnEndDrag(PointerEventData eventData) {
        GameObject[] actions = GameObject.FindGameObjectsWithTag("Action");

        GameObject childAbove = null;
        GameObject childBelow = null;

        foreach (GameObject action in actions) {
            if (action != null) {
                if (action != gameObject) {
                    if (action.transform.localPosition.y > transform.localPosition.y) {
                        if (childAbove == null) {
                            childAbove = action;
                        } else {
                            if (Mathf.Abs(action.transform.localPosition.x - transform.localPosition.x) < Mathf.Abs(childAbove.transform.localPosition.x - transform.localPosition.x)) {
                                childAbove = action;
                            }
                        }
                    } else {
                        if (childBelow == null) {
                            childBelow = action;
                        } else {
                            if (Mathf.Abs(action.transform.localPosition.x - transform.localPosition.x) < Mathf.Abs(childBelow.transform.localPosition.x - transform.localPosition.x)) {
                                childBelow = action;
                            }
                        }
                    }
                }
            }
        }

        if (childBelow != null) {
            if (childBelow.transform.GetSiblingIndex() == 0) {
                transform.SetSiblingIndex(0);
            }
        } else if (childAbove != null) {
            if (childAbove.transform.GetSiblingIndex() == transform.parent.childCount - 1) {
                transform.SetSiblingIndex(transform.parent.childCount - 1);
            }
        } else if (childBelow != null && childAbove != null) {
            transform.SetSiblingIndex(childAbove.transform.GetSiblingIndex() + 1);
        }

        name = "changed";

        if (childBelow != null) {
            Debug.Log("Placed above " + childBelow.transform.GetSiblingIndex());
        }

        if (childAbove != null) {
            Debug.Log("Placed below " + childAbove.transform.GetSiblingIndex());
        }

        transform.parent.GetComponent<GridLayoutGroup>().CalculateLayoutInputVertical();
        transform.parent.GetComponent<GridLayoutGroup>().CalculateLayoutInputHorizontal();
    }
}
