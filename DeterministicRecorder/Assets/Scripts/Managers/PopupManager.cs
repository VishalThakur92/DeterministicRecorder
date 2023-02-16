using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : UIElement
{
    [SerializeField]
    Button closeButton;


    void Awake()
    {
        closeButton.onClick.AddListener(()=> Toggle(false));
    }

}
