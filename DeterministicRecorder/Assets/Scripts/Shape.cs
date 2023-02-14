using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shape : MonoBehaviour, IDragHandler
{


    //Handle what happens OnDrag event
    public void OnDrag(PointerEventData eventData)
    {
        //reposition this transform to the new drag position
        transform.position = eventData.position;
    }
}
