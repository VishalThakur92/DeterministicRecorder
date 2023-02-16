using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : GenericSingletonClass<AppManager>
{
    [SerializeField]
    public PopupManager popupManager;

    [SerializeField]
    public TooltipManager tooltipManager;

    [SerializeField]
    public MouseInteractionManager mouseInteractionManager;

    [SerializeField]
    public SceneHandler sceneHandler;
}
