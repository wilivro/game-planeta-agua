using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

using Rpg;

public class Slot : MonoBehaviour, IDropHandler {

	// tipo do lixo correto a ser dropado nesta lixeira
	public string type;

	public GameObject item
	{
		get
		{
			if(transform.childCount > 0)
			{
				return transform.GetChild(0).gameObject;
			}
			else
			{
				return null;
			}
		}
	}
	#region IDropHandler implementation
	public void OnDrop (PointerEventData eventData)
	{
		Debug.Log(InventoryDraggableItem.item);


		if (InventoryDraggableItem.item.type == type) {
			GameController.player.score += 10;
		}

		Player.inventory.Remove(InventoryDraggableItem.item);
		
		Destroy (InventoryDraggableItem.itemBeingDragged);

	

	}
	#endregion

}
