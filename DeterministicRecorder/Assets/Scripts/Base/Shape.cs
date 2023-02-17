using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//This is the base class of any Shape

//In our case we have Triangle, Circle, Star
//All of them can have individual characteristic and behaviours
[RequireComponent(typeof(Image))]
public abstract class Shape : MonoBehaviour
{

    #region Parameters
    //Name of this shape
    [SerializeField]
    protected string shapeName;

    //short Description about this shape
    [SerializeField]
    protected string shortDescription;

    //Detailed Description about this shape
    [SerializeField]
    protected string longDescription;

    //the possible Color Variations for this shape
    [SerializeField]
    protected List<Color> colorVariations = new List<Color>();

    //Ref to this shape's Image component
    protected Image imageRef;

    //Index of the currently selected color amongst the colorVariations
    protected int currentColorIndex = 0;

    //Last selected color index , Used to avoid setting of same color again & again
    protected int lastSetColorIndex = 0;
    #endregion



    #region Core
    void Start() {

        //grab the Shape's Image Ref
        imageRef = GetComponent<Image>();

        //set first color as default
        if (colorVariations.Count > 0)
            imageRef.color = colorVariations[currentColorIndex];
    }
    protected virtual void CycleColor()
    {
        currentColorIndex++;

        if (currentColorIndex >= colorVariations.Count)
            currentColorIndex = 0;

        SetColor(currentColorIndex);
    }

    public virtual void SetColor(int colorIndex)
    {
        //Skip setting same color
        if (lastSetColorIndex == colorIndex || colorIndex >= colorVariations.Count)
            return;

        //save the last set color index 
        lastSetColorIndex = colorIndex;

        //set shape's Color
        imageRef.color = colorVariations[colorIndex];
    }

    public int GetCurrentColorIndex() {
        return currentColorIndex;
    }
    #endregion
}
