using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIElement : MonoBehaviour
{

    #region Parameters
    //UI ref
    [SerializeField]
    protected GameObject uiRef;

    //Text ref
    [SerializeField]
    protected Text textRef;


    //Represents if this UIElement is active over the GUI or not
    protected bool isActive;
    #endregion



    #region Unity
    protected virtual void Start()
    {
        Toggle(false);
    }

    //Used to Show/Hide this Element with a string data if provided
    public virtual void Toggle(bool flag, string data = null, Vector2 position = default)
    {
        //Show UI
        uiRef.gameObject.SetActive(flag);

        //Set the Specified text
        //Is set null if no data is provided
        textRef.text = data;

        isActive = flag;
    }


    //Get the IsActive state of this UIElement
    public virtual bool GetIsActive()
    {
        return isActive;
    }

    //Get the string data of the Text element
    public virtual string GetText()
    {
        return textRef.text;
    }
    #endregion
}
