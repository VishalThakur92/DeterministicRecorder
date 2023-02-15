using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseInteractionManager : MonoBehaviour
{


    #region Parameters
    //Time when Pointer enters an interactable object
    float timestamp = 0;


    //If timer gets past this many seconds then It will considered as an extended hover
    [SerializeField]
    float extendedHoverTime = 3;

    //Was there a drag ?
    bool isDragging = false;

    //The current mouse interactable obj
    IMouseInteractable currentInteractableObject;


    //Used to determine if a drag was performed
    Vector3 lastMousePosition;
    #endregion




    #region Core



    void Update()
    {
        //Start of a potential Drag
        if (Input.GetMouseButtonDown(0)) {
            lastMousePosition = Input.mousePosition;
        }

        //Left Click
        if (Input.GetMouseButtonUp(0))
        {
            //Drag was performed
            if (!Vector3.Equals(lastMousePosition, Input.mousePosition))
            {
                currentInteractableObject?.OnMousePointerExit();
            }
            else {
                currentInteractableObject?.OnLeftMouseClick();
            }

            isDragging = false;
        }

        //Right Click
        if (Input.GetMouseButtonUp(1))
        {
            currentInteractableObject?.OnRightMouseClick();
        }

        //Drag
        if (Input.GetMouseButton(0))
        {
            //Drag was performed
            if (!Vector3.Equals(lastMousePosition, Input.mousePosition))
            {
                isDragging = true;
                currentInteractableObject?.OnMouseDrag(Input.mousePosition);
            }
        }
    }

    //Mouse Pointer entered an IInteractable object
    public void SetInteractableObject(GameObject obj)
    {
        if (isDragging)
            return;

        //Save the time stamp for extended hover detection
        timestamp = Time.time;


        //save the obj if is of type IInteractable 
        if (obj.GetComponent<IMouseInteractable>() != null)
            currentInteractableObject = obj.GetComponent<IMouseInteractable>();
        else
            Debug.LogError("Invalid IInteractable obj");
    }

    //Pointer exited an IInteractable  object
    public void ResetInteractableObject()
    {
        if(!isDragging)
            currentInteractableObject = null;
    }

   

    #endregion







    ////Handle what happens OnDrag event
    //public void OnDrag(PointerEventData eventData)
    //{
    //    //reposition this transform to the new drag position
    //    currentInteractableObject.OnMouseDrag(eventData.position);

    //    Debug.LogError("On Drag");
    //    if (Vector2.Equals(eventData.delta, Vector2.zero))
    //    {
    //        isDragging = false;
    //    }
    //    else
    //    {
    //        isDragging = true;
    //    }
    //}


    //public void OnPointerUp(PointerEventData eventData)
    //{

    //    if (isDragging)
    //    {
    //        isDragging = false;
    //        return;
    //    }

    //    if (eventData.button == PointerEventData.InputButton.Left)
    //    {
    //        currentInteractableObject.OnLeftMouseClick();
    //    }
    //    else if (eventData.button == PointerEventData.InputButton.Right) {
    //        currentInteractableObject.OnRightMouseClick();
    //    }
    //}


    //public void OnPointerEnter()
    //{
    //    timestamp = Time.time;
    //    Debug.LogError("Pointer Enter");
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    Debug.LogError("Pointer Exit");
    //}


    void OnExtendedHover() {
        Debug.LogError("On Extended Hover");
    }
}
