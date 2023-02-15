using UnityEngine;
public interface IInteractable 
{
	void OnMouseHoverExtended();

	void OnLeftMouseClick();

	void OnRightMouseClick();

	void OnMouseDrag(Vector2 position);

	void OnMousePointerExit();
}
