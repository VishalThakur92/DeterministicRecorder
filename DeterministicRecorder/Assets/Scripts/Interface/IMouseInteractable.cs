using UnityEngine;
public interface IMouseInteractable
{
	void OnMouseHoverExtended();

	void OnLeftMouseClick();

	void OnRightMouseClick();

	void OnMouseDrag(Vector2 position);

	void OnMousePointerExit();
}
