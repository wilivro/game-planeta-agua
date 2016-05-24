using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using Rpg;

public class InventoryDraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public static GameObject itemBeingDragged;
	Vector3 startPosition;
	Transform startParent;
	public Item reference;
	public static Item item;

	#region IBeginDragHandler implementation
	public void OnBeginDrag (PointerEventData eventData)
	{
		itemBeingDragged = gameObject;
		item = reference;
		startPosition = transform.position;
		startParent = transform.parent;
	}
	#endregion

	#region IDragHandler implementation
	public void OnDrag (PointerEventData eventData)
	{
		transform.position = Input.mousePosition + new Vector3(0, 0, 10);
	}
	#endregion

	#region IEndDragHandler implementation
	public void OnEndDrag (PointerEventData eventData)
	{
		itemBeingDragged = null;
		transform.position = Vector3.Lerp(transform.position, startPosition, 1);
	}
	#endregion
}