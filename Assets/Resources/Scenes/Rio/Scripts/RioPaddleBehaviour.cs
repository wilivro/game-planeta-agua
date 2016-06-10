using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class RioPaddleBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler {

	GameObject itemBeingDragged;
	Vector3 startPosition;
	RectTransform rt;
	float canvasHeight;

	public static float pos;

	#region IBeginDragHandler implementation
	public void OnBeginDrag (PointerEventData eventData)
	{
		itemBeingDragged = gameObject;
		startPosition = transform.position;
		canvasHeight = transform.parent.parent.GetComponent<RectTransform>().sizeDelta.y;
		rt = transform.GetComponent<RectTransform>();
	}
	#endregion

	#region IDragHandler implementation
	public void OnDrag (PointerEventData eventData)
	{	
		float top = canvasHeight - rt.sizeDelta.y - 2;
		pos = rt.anchoredPosition.y / (top);

		transform.position = new Vector3(startPosition.x, Input.mousePosition.y-28, startPosition.z);
		if(rt.anchoredPosition.y >= top) {
			rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, top);
			return;
		} 

		if(rt.anchoredPosition.y <= 2) {
			rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, 2);
			return;
		}
	}
	#endregion
	
}
