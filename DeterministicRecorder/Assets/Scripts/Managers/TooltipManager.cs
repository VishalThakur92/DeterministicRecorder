using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : UIElement
{
    public override void Toggle(bool flag, string data = null, Vector2 position = default)
    {
        base.Toggle(flag, data, position);


        //Reposition the tooltip to the specified position
        uiRef.transform.position = position;
    }

    //Get position of this Tooltip
    public Vector2 GetPosition() {
        return (Vector2)uiRef.transform.position;
    }
}
