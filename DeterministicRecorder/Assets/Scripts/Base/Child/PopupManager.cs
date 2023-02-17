using UnityEngine;
using UnityEngine.UI;


//This is a child class of UIElement
//This class is responsible for showing the Popup
public class PopupManager : UIElement
{
    #region Parameters

    //The Close button used to close this popup
    [SerializeField]
    Button closeButton;
    #endregion


    #region Core
    void Awake()
    {
        //Upon clicking the popup's Close button the Popup is closed in the rightful manner
        closeButton.onClick.AddListener(()=> Toggle(false));
    }
    #endregion

}
