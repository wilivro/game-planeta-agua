using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class InventoryTrash : MonoBehaviour, IDropHandler
{

	#region IDropHandler implementation
	public void OnDrop (PointerEventData eventData)
	{
		WindowCanvas.inventory.OpenQtd(InventoryDraggableItem.item, this);
	}
	#endregion

	public void Cancel(){
		WindowCanvas.inventory.ShowItems();
	}

	public void RemoveItem(int qtd = 1){
		Rpg.Player.inventory.Remove(InventoryDraggableItem.item, qtd);
		Cancel();
	}
}
