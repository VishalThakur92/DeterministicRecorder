using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StarShape : Shape, IInteractable
{
    public void OnLeftMouseClick()
    {
        //Show Popup
        PopupManager.Instance.TogglePopup(true,longDescription);
        //Debug.LogError(shapeName + "Left mouse Click");
    }

    public void OnRightMouseClick()
    {
        //Cycle this shapes Color
        CycleColor();

        //Hide Popup
        PopupManager.Instance.TogglePopup(false);
        //Debug.LogError(shapeName + "Mouse right click");
    }

    public void OnMouseDrag(Vector2 position)
    {
        //Hide Popup
        PopupManager.Instance.TogglePopup(false);
        //Move this Shape to the position
        transform.position = position;
        //Debug.LogError(shapeName + "Mouse Drag " + position);
    }

    public void OnMouseHoverExtended()
    {
        //Show ToolTip
        Debug.LogError(shapeName + "Mouse Hover Extended");
    }

    public void OnMousePointerExit()
    {
        //Hide Tooltip if being shown
    }
}
