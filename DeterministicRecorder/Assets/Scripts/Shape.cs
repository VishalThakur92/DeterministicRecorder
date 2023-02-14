using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class Shape : MonoBehaviour, IDragHandler, IPointerUpHandler
{

    [SerializeField]
    public string uniqueName;
    [SerializeField]
    bool wasDragged = false;


    [SerializeField]
    List<Color> availableColors = new List<Color>();

    Image imageRef;


    public int currentColorIndex = 0;





    void Start() {

        //get the Shape's Image Ref
        imageRef = GetComponent<Image>();

        //set first color as default
        if (availableColors.Count > 0)
            imageRef.color = availableColors[currentColorIndex];
    }


    //Handle what happens OnDrag event
    public void OnDrag(PointerEventData eventData)
    {
        //reposition this transform to the new drag position
        transform.position = eventData.position;

        if (Vector2.Equals(eventData.delta, Vector2.zero))
        {
            wasDragged = false;
        }
        else
            wasDragged = true;
    }


    public void OnPointerUp(PointerEventData eventData)
    {

        if (wasDragged)
        {
            wasDragged = false;
            return;
        }
        CycleColor();
        Debug.LogError(gameObject.name + "was clicked");
    }

    void CycleColor()
    {
        currentColorIndex++;

        if (currentColorIndex >= availableColors.Count)
            currentColorIndex = 0;

        SetColor(currentColorIndex);
    }


    int lastSetColorIndex = 0;


    public void SetColor(int colorIndex)
    {
        //Skip setting same color
        if (lastSetColorIndex == colorIndex)
            return;

        lastSetColorIndex = colorIndex;
        imageRef.color = availableColors[colorIndex];
    }
}
