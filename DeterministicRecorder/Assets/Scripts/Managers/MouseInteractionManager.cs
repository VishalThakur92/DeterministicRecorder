using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//This class is responsible for the Interaction with IMouseInteractable objects via mouse
public class MouseInteractionManager : MonoBehaviour
{


    #region Parameters
    //If timer gets past this many seconds then It will considered as an extended hover
    [SerializeField]
    float extendedHoverTime = 3;

    bool didExtendedHover = false;

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


            didExtendedHover = false;
        }

        if (Input.GetMouseButton(1)) {
            didExtendedHover = false;
        }
    }

    //Mouse Pointer entered an IMouseInteractable object
    public void OnPointerEnter(GameObject obj)
    {
        if (isDragging)
            return;

        //Start Checking for Extended Hover
        StartCoroutine(CheckExtendedHover());

        //save the obj if is of type IMouseInteractable 
        if (obj.GetComponent<IMouseInteractable>() != null)
            currentInteractableObject = obj.GetComponent<IMouseInteractable>();
        else
            Debug.LogError("Invalid IMouseInteractable obj");
    }

    //Pointer exited an IMouseInteractable  object
    public void OnPointerExit()
    {
        if(!isDragging)
            currentInteractableObject = null;

        didExtendedHover = false;
    }


    //Checks if the user is performing an extended hover
    //in simple words, check if the user has the mouse over the IMouseInteractable object for extendedHoverTime
    IEnumerator CheckExtendedHover() {
        didExtendedHover = true;
        int startTime = 0;
        while (startTime <= extendedHoverTime) {
            startTime++;
            yield return new WaitForSeconds(1);
        }
        if (didExtendedHover)
        {
            currentInteractableObject?.OnMouseHoverExtended();
            didExtendedHover = false;
        }
        StopCoroutine(CheckExtendedHover());
    }
    #endregion
}
