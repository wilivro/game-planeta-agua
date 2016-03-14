using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Slot : MonoBehaviour, IDropHandler {

	// tipo do lixo correto a ser dropado nesta lixeira
	public int type;

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
		
		Garbage g = DragHandler.itemBeingDragged.gameObject.GetComponent<Garbage> ();
		Player.inventory.Remove (g);

		int _score = PlayerPrefs.GetInt ("Score");

		if (g.type == type) {
			PlayerPrefs.SetInt ("Score", _score + 10);	
		}

		Destroy (g.gameObject);

		//Lixeira.Draw ();
	

	}
	#endregion

}
