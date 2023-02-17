using UnityEngine;

public class TriangleShape : Shape, IMouseInteractable
{
    public void OnLeftMouseClick()
    {
        //Show Popup
        AppManager.Instance.popupManager.Toggle(true, longDescription);

        //Hide Tooltip
        AppManager.Instance.tooltipManager.Toggle(false);
    }

    public void OnRightMouseClick()
    {
        //Cycle this shapes Color
        CycleColor();

        //Hide Popup
        AppManager.Instance.popupManager.Toggle(false);

        //Hide Tooltip
        AppManager.Instance.tooltipManager.Toggle(false);
    }

    public void OnMouseDrag(Vector2 position)
    {
        //Hide Popup
        AppManager.Instance.popupManager.Toggle(false);

        //Hide Tooltip
        AppManager.Instance.tooltipManager.Toggle(false);

        //Move this Shape to the position
        transform.position = position;
    }

    public void OnMouseHoverExtended()
    {
        //Show ToolTip
        AppManager.Instance.tooltipManager.Toggle(true, shortDescription, transform.position);
    }

    public void OnMousePointerExit()
    {
        //Hide Tooltip if being shown
        AppManager.Instance.tooltipManager.Toggle(false);
    }
}