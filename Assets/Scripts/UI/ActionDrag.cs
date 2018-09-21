using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    public Canvas canvas;
    public Vector2 startPos;
    public int startingChildrenNumber;
    public int indexInParent;
    public Transform parentObj;

    private void Start() {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        parentObj = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        startPos = transform.localPosition;
        indexInParent = transform.GetSiblingIndex();
        startingChildrenNumber = parentObj.childCount;
        transform.SetParent(parentObj.parent.parent);

        Debug.Log("1st index: " + indexInParent + "/" + (startingChildrenNumber - 1));
    }

    public void OnDrag(PointerEventData eventData) {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
        transform.position = canvas.transform.TransformPoint(pos);
    }

    public void OnEndDrag(PointerEventData eventData) {
        transform.SetParent(parentObj);
        indexInParent -= (startingChildrenNumber - parentObj.childCount);
        transform.SetSiblingIndex(indexInParent);

        Debug.Log(startingChildrenNumber);
        Debug.Log("2nd index: " + indexInParent + "/" + (parentObj.childCount - 1));

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
                            if (Mathf.Abs(action.transform.position.x - transform.position.x) <= Mathf.Abs(childAbove.transform.position.x - transform.position.x)) {
                                if (Mathf.Abs(action.transform.position.y - transform.position.y) < Mathf.Abs(childAbove.transform.position.y - transform.position.y)) {
                                    childAbove = action;
                                }
                            }
                        }
                    } else {
                        if (childBelow == null) {
                            childBelow = action;
                        } else {
                            if (Mathf.Abs(action.transform.position.x - transform.position.x) <= Mathf.Abs(childBelow.transform.position.x - transform.position.x)) {
                                if (Mathf.Abs(action.transform.position.y - transform.position.y) < Mathf.Abs(childBelow.transform.position.y - transform.position.y)) {
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

        SelectionController cont = SelectionController._instance;
        SelectionController.PlannedAction tempAction = cont.plannedActions[startIndex];
        GameObject tempIcon = cont.plannedActionRemovalIcons[startIndex];

        cont.plannedActions.RemoveAt(startIndex);
        cont.plannedActionRemovalIcons.RemoveAt(startIndex);

        if (startIndex < changedIndex) {
            changedIndex--;
        }

        Debug.Log("Setting to index: " + changedIndex);

        cont.plannedActions.Insert(changedIndex, tempAction);
        cont.plannedActionRemovalIcons.Insert(changedIndex, tempIcon);
    }
}
