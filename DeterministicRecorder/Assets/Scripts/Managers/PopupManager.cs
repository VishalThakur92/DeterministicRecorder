using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : GenericSingletonClass<PopupManager>
{
    //Popup UI ref
    [SerializeField]
    GameObject popupUI;

    //Popup Text
    [SerializeField]
    Text popupUIText;

    private void Start()
    {
        TogglePopup(false);
    }

    //Used to Show/Hide popup with a string data
    public void TogglePopup(bool flag, string data = null) {
        //Show Popup UI
        popupUI.gameObject.SetActive(flag);

        //Set the Specified popup text
        //Is set null if no data is provided
        popupUIText.text = data;
    }
}
