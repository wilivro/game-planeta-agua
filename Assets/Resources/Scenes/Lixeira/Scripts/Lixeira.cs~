using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

using Rpg;

public class Lixeira : MonoBehaviour {

	Transform itensContainer;
	GameObject item;

	// Use this for initialization
	void Start () {
		itensContainer = transform.Find ("Itens").transform;
		item = Resources.Load("WindowSystem/Prefabs/Inventory/Item") as GameObject;
		transform.Find("Title").Find("Exit").GetComponent<Button>().onClick.AddListener(delegate {OnClickSair();});

		Draw ();

	}

	void Draw(){
		Item it = null;
		for(int i = 0; i < Player.inventory.items.Count; i++){
			it = Player.inventory.items[i];
			GameObject a = UnityEngine.Object.Instantiate(item);
			a.transform.SetParent(itensContainer ,false);
			a.GetComponent<Image>().sprite = it.icon;
			a.transform.Find("Qtd").Find("Text").GetComponent<Text>().text = it.qtd.ToString();
			InventoryDraggableItem idi = a.AddComponent<InventoryDraggableItem>();
			idi.reference = it;
		}
	}

	void OnClickSair() {
		Application.LoadLevel(2);
	}
}
