using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler 
{
    Vector3 offset = Vector3.zero;

    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = transform.position - eventData.pressEventCamera.ScreenToWorldPoint(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.pressEventCamera.ScreenToWorldPoint(eventData.position) + offset;
    }
}
