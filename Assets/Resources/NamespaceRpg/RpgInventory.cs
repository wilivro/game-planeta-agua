using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Window;

namespace Rpg
{
	public class Inventory
	{
		public List<Item> items;
		int gold;
		public int Count {
			get {
				return items.Count;
			}

			set {

			}
		}

		public Inventory(){
			items = new List<Item>();
			gold = 100;
		}

		public Item Add(Item i){
			EventManager.Trigger("ItemAdd");
			Item[] it = items.Where(itt => itt.name == i.name).ToArray();
			int qtd = it.Length;

			if(!i.acumulative){
				items.Add(i);
				qtd ++;
			}else if(qtd == 1) {
				it[0].qtd += i.qtd;
				qtd = it[0].qtd;
			} else {
				items.Add(i);
				qtd = i.qtd;
			}
			
			Log.Register(i.id, qtd.ToString());

			return i;
		}

		public Item Add(Item i, int qtd){

			if(i.acumulative){
				i.qtd = qtd;
				return Add(i);
			} else {
				for(int n = 0; n < qtd; n++){
					Add(i);
				}
			}


			return i;
		}

		public Item Remove(string baseName, int qtd = 1) {
			return Remove(new Item(baseName), qtd);
		}

		public Item Remove(Item i, int _qtd = 1) {
			Item[] it = items.Where(itt => itt.id == i.id).ToArray();
			int qtd = it.Length;

			if(qtd > 0 && !i.acumulative){
				if(_qtd > it.Length) _qtd = qtd;
				for(int n = 0; n < _qtd; n++){
					qtd -= 1;
					items.Remove(it[n]);
				}
			}
			if(i.acumulative) {
				it[0].qtd -= _qtd;
				qtd = it[0].qtd;				
			}

			Log.Register(i.id, qtd.ToString());

			return i;
		}

		public int GetGold(int _qtd = 0) {
			gold -= _qtd;
			return gold;
		}

		public int AddGold(int _add){
			gold += _add;

			return gold;
		}

		public bool HasItem(string _name, int qtd = 1) {
			Item[] it = items.Where(itt => itt.name == _name).ToArray();
			if(it.Length >= qtd || it[0].qtd >= qtd) return false;
			return true;
		}
	}

	namespace WindowSystem
	{
		public class Inventory : WindowBase
		{
			public static int capacity = 30;

			GameObject slot;
			GameObject item;
			GameObject qtd;
			GameObject qtdInstance;
			bool qtdOpen;
			bool showMoney;
			bool showTrash;

			ItemDescription description;

			public Inventory(Transform _canvas, bool _showMoney = true, bool _showTrash = true) : base(_canvas) {
				prefab = Resources.Load("WindowSystem/Prefabs/Inventory/Inventory") as GameObject;
				prefab.transform.Find("Name").Find("Text").GetComponent<Text>().text = GameController.language.inventory;
				prefab.transform.Find("Gold").Find("Label").GetComponent<Text>().text = GameController.language.gold;
				slot = Resources.Load("WindowSystem/Prefabs/Inventory/Slot") as GameObject;
				item = Resources.Load("WindowSystem/Prefabs/Inventory/Item") as GameObject;
				qtd = Resources.Load("WindowSystem/Prefabs/Inventory/QtdInput") as GameObject;
				qtdOpen = false;

				showMoney = _showMoney;
				showTrash = _showTrash;

			}

			public void OpenQtd(Item it, InventoryTrash trash) {
				if(qtdOpen) return;

				if(!it.acumulative || it.qtd < 2){
					trash.RemoveItem(it.qtd);
					return;
				}

				qtdInstance = UnityEngine.Object.Instantiate(qtd);
				qtdInstance.transform.SetParent(instance.transform, false);

				InputField field = qtdInstance.transform.Find("InputField").gameObject
					.GetComponent<InputField>();

				field.text = it.qtd.ToString();
				field.Select();

				qtdInstance.transform.Find("Cancel").gameObject.GetComponent<Button>().onClick
					.AddListener(delegate {trash.Cancel();});

				qtdInstance.transform.Find("Submit").gameObject.GetComponent<Button>().onClick
					.AddListener(delegate {RemoveItem(it, trash);});

				qtdOpen = true;

			}

			void CloseQtd(){
				if(!qtdOpen) return;
				UnityEngine.Object.Destroy(qtdInstance);
				qtdOpen = false;
			}

			public void RemoveItem(Item it, InventoryTrash trash) {
				int value = Convert.ToInt32(qtdInstance.transform.Find("InputField").gameObject.GetComponent<InputField>().text);
				if(value > it.qtd) value = it.qtd;
				trash.RemoveItem(value);
			}

			public override void Open(bool preserveParent = false){
				base.Open();
				instance.transform.Find("Close").GetComponent<Button>().onClick.AddListener(delegate {Close();});

				if(showTrash) {
					instance.transform.Find("Trash").gameObject.AddComponent<InventoryTrash>();
				} else {
					UnityEngine.Object.Destroy(instance.transform.Find("Trash").gameObject);
					UnityEngine.Object.Destroy(instance.transform.Find("TrashIcon").gameObject);
				}

				if(showMoney) {
					instance.transform.Find("Gold").Find("Text").GetComponent<Text>().text = Player.inventory.GetGold().ToString();
				} else {
					UnityEngine.Object.Destroy(instance.transform.Find("Gold").gameObject);
				}
				description = new ItemDescription(instance.transform);
				ShowItems();
			}

			public void ShowItems(){

				CloseQtd();
				description.Close();

				Transform slots = instance.transform.Find("Slots");
				foreach(Transform tr in slots){
					UnityEngine.Object.Destroy(tr.gameObject);
				}

				Item it = null;
				int k = 0;
				for(int i = 0; i < Player.inventory.items.Count + capacity; i++){
					if(k >= capacity) break;
					GameObject a = UnityEngine.Object.Instantiate(slot);
					RectTransform rta = a.GetComponent<RectTransform>();
					rta.SetParent(slots ,false);

					try {
						it = Player.inventory.items[i];
						if(!it.visible){
							Debug.Log("Hidden one");
							UnityEngine.Object.Destroy(a);
							continue;
						}
						GameObject b = UnityEngine.Object.Instantiate(item);
						b.name = it.name;
						if(showTrash) {
							InventoryDraggableItem idi = b.AddComponent<InventoryDraggableItem>();
							idi.reference = it;
						}
						RectTransform rtb = b.GetComponent<RectTransform>();
						rtb.SetParent(rta, false);
						b.GetComponent<Image>().sprite = it.icon;
						rtb.Find("Qtd").Find("Text").GetComponent<Text>().text = it.qtd.ToString();

						Item itt = it;

						b.GetComponent<Button>().onClick.AddListener(delegate {description.Open(itt);});
					} catch	{}
					k++;
				}
			}
		};

		
	}
}