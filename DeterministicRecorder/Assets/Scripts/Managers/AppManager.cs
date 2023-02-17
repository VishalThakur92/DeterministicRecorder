using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the Boss or the Main Singelton Class through which the game algorithm can access the small-manager classes
//This stores reference to all Possible Managers in this app
public class AppManager : GenericSingletonClass<AppManager>
{
    #region Parameters
    [SerializeField]
    public PopupManager popupManager;

    [SerializeField]
    public TooltipManager tooltipManager;

    [SerializeField]
    public MouseInteractionManager mouseInteractionManager;


    [SerializeField]
    public FileIOManager fileIOManager;

    [SerializeField]
    public ReplayManager replayManager;
    #endregion
}
