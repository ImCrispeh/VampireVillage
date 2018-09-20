using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    public Canvas canvas;
    public Vector2 startPos;

    private void Start() {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        startPos = transform.localPosition;
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
        bool isChanged = false;

        foreach (GameObject action in actions) {
            if (action != null) {
                if (action != gameObject) {
                    if (action.transform.localPosition.y > transform.localPosition.y) {
                        if (childAbove == null) {
                            childAbove = action;
                        } else {
                            if (Mathf.Abs(action.transform.localPosition.x - transform.localPosition.x) <= Mathf.Abs(childAbove.transform.localPosition.x - transform.localPosition.x)) {
                                if (Mathf.Abs(action.transform.localPosition.y - transform.localPosition.y) < Mathf.Abs(childAbove.transform.localPosition.y - transform.localPosition.y)) {
                                    childAbove = action;
                                }
                            }
                        }
                    } else {
                        if (childBelow == null) {
                            childBelow = action;
                        } else {
                            if (Mathf.Abs(action.transform.localPosition.x - transform.localPosition.x) <= Mathf.Abs(childBelow.transform.localPosition.x - transform.localPosition.x)) {
                                if (Mathf.Abs(action.transform.localPosition.y - transform.localPosition.y) < Mathf.Abs(childBelow.transform.localPosition.y - transform.localPosition.y)) {
                                    childBelow = action;
                                }
                            }
                        }
                    }
                }
            }
        }

        int startIndex = transform.GetSiblingIndex();
        int changedIndex = transform.GetSiblingIndex();

        if (childBelow != null) {
            if (childBelow == transform.parent.GetChild(0).gameObject) {
                Debug.Log("set to first");
                transform.SetAsFirstSibling();
                changedIndex = transform.GetSiblingIndex();
                isChanged = true;
            }
        }

        if (childAbove != null && !isChanged) {
            if (childAbove.transform.GetSiblingIndex() == transform.parent.childCount - 1) {
                Debug.Log("set to last");
                transform.SetAsLastSibling();
                changedIndex = transform.GetSiblingIndex();
                isChanged = true;
            }
        }

        if (childBelow != null && childAbove != null && !isChanged) {
            Debug.Log("set between 2");
            transform.SetSiblingIndex(childAbove.transform.GetSiblingIndex() + 1);
            changedIndex = transform.GetSiblingIndex();
        }

        if (startIndex == changedIndex) {
            transform.localPosition = startPos;
        }

        Debug.Log(transform.parent.childCount);
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
