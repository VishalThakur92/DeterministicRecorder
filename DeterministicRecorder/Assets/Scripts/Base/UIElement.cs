using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIElement : GenericSingletonClass<UIElement>
{

    #region Parameters
    //UI ref
    [SerializeField]
    protected GameObject uiRef;

    //Text ref
    [SerializeField]
    protected Text textRef;
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
    }
    #endregion
}
