using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : GenericSingletonClass<AppManager>
{
    [SerializeField]
    public UIElement popupManager;

    [SerializeField]
    public UIElement tooltipManager;

    [SerializeField]
    public MouseInteractionManager mouseInteractionManager;
}
