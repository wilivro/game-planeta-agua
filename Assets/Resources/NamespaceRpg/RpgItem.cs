using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Window;

using Rpg;
using Rpg.QuestSystem;
using Rpg.DialogueSystem;

namespace Rpg
{
	public class Item
	{
		public string id;
		public string baseName;
		public string name;
		public string description;
		public int iconIndex;
		public string imageName;
		public int qtd;
		public bool acumulative;
		public bool consumible;
		public bool unique;
		public string[] book;
		public bool visible;
		public string[] preRequirements;
		public bool receive = true;
		public bool autoOpen;
		public bool permanent;
		public LogData[] registerLog;

		Sprite[] icons;
		public Sprite icon;
		Sprite[] images;
		public Sprite image;

		public Item(string _id) {
			id = _id;
			DatabaseItem dd = (DatabaseItem)GameController.database.Find(id);

			if(dd == null){
				visible = false;
				return;
			};

			string path = dd.path+dd.filename;
			TextAsset itemFile = Resources.Load(path) as TextAsset;

			baseName = dd.filename;

			visible = true;
			JsonUtility.FromJsonOverwrite(itemFile.text, this);

			icons = Resources.LoadAll<Sprite>(dd.path+"Images/icons");
			images = Resources.LoadAll<Sprite>(dd.path+"Images/"+imageName);

			icon = icons[iconIndex];
			image = images[0];


			qtd = 1;
		}

		public override string ToString() {
			return id+"|"+qtd;
		}
	}

	namespace WindowSystem
	{
		public class ItemDescription : WindowBase
		{

			public ItemDescription(Transform _canvas) : base(_canvas) {
				prefab = Resources.Load("WindowSystem/Prefabs/Inventory/Description") as GameObject;
			}

			public void Open(Item it){
				base.Close();
				base.Open(true);
				instance.transform.Find("Close").GetComponent<Button>().onClick.AddListener(delegate {Close(true);});
				RectTransform rt = instance.GetComponent<RectTransform>();
				rt.SetParent(instance.transform, false);

				rt.Find("Name").GetComponent<Text>().text = it.name;
				rt.Find("Image").GetComponent<Image>().sprite = it.image;
				rt.Find("Scroll View").Find("Viewport").Find("Description").GetComponent<Text>().text = it.description;
				if(it.book == null){
					UnityEngine.Object.Destroy(rt.Find("Book").gameObject);	
					return;
				}
				rt.Find("Book").GetComponent<Button>().onClick.AddListener(delegate {OpenBook(it);});
			}

			void OpenBook(Item it){
				Transform target = GameObject.Find("GameController").transform;
				Window.Book book = new Window.Book(target, it.book, true);
				book.Open();
			}
		}
	}
}
