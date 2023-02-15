using UnityEngine;

public class TriangleShape : Shape, IInteractable
{
    public void OnLeftMouseClick()
    {
        TooltipManager.Instance.Toggle(true , shortDescription , transform.position);
        //PopupManager.Instance.Toggle(true, longDescription);
        Debug.LogError(shapeName + "Left mouse Click");
    }

    public void OnRightMouseClick()
    {
        //Cycle this shapes Color
        CycleColor();
        Debug.LogError(shapeName + "Mouse right click");
        //Hide Popup
        PopupManager.Instance.Toggle(false);
    }

    public void OnMouseDrag(Vector2 position)
    {
        //Hide Popup
        PopupManager.Instance.Toggle(false);
        //Move this Shape to the position
        transform.position = position;
        Debug.LogError(shapeName + "Mouse Drag " + position);
    }

    public void OnMouseHoverExtended()
    {
        Debug.LogError(shapeName + "Mouse Hover Extended");
    }

    public void OnMousePointerExit()
    {
        //Hide Tooltip if being shown
    }
}
