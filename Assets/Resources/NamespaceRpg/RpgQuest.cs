using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Window;
using Rpg.QuestSystem;

namespace Rpg
{
	namespace QuestSystem
	{
		public class Quest
		{
			public enum QuestStatus {progress, complete, archived};
			public string name;
			public string id;
			public string description;
			public float exp;
			public string icon;
			public string image;
			public List<string> rewardName;
			public List<Item> reward;
			public int index;
			public bool read;
			public QuestStatus status;
			public string[] requirements;

			public Quest(string _id) {
				id = _id;
				string path = ((DatabaseQuest) GameController.database.Find(id)).GetFullPath();

				TextAsset questFile = Resources.Load(path) as TextAsset;

				JsonUtility.FromJsonOverwrite(questFile.text, this);
				read = false;
				SetStatus(QuestStatus.progress);

				EventManager.Trigger("QuestAdd");

				foreach(string s in requirements){
					EventManager.Trigger("QuestHelperItemAdd",new object[1] {s});
				}

			}

			public Quest(string _id, QuestStatus _status) : this(_id) {
				SetStatus(_status);
			}

			public void SetStatus(QuestStatus _status) {
				status = _status;
				Log.Register(id, status.ToString());
			}

			public override string ToString() {
				return id+"|"+status.ToString();
			}

			public void Archive() {
				SetStatus(Quest.QuestStatus.archived);
				foreach(string s in requirements){
					EventManager.Trigger("QuestHelperItemDelete",new object[1] {s});
				}
			}
		}
	}

	namespace WindowSystem
	{
		public class Journal : WindowBase
		{
			GameObject questJournalSelect;
			Transform tabs;
			GameObject space;
			RectTransform myRt;
			ToggleGroup tg;
			RectTransform pane;
			GameObject questPane;

			public static Color unReadColor = new Color(1f,1f,1f, 1);
			public static Color readColor = new Color(0.3f,0.3f,0.3f, 1);

			public Journal(Transform _canvas) : base(_canvas) {
				prefab = Resources.Load("WindowSystem/Prefabs/QuestJournal/QuestJournal") as GameObject;

				prefab
					.transform.Find("Name")
					.Find("Text")
					.GetComponent<Text>()
					.text = GameController.language.journal;

				questJournalSelect = Resources.Load("WindowSystem/Prefabs/QuestJournal/QuestJournalSelect") as GameObject;
				space = Resources.Load("WindowSystem/Prefabs/QuestJournal/Space") as GameObject;
				questPane = Resources.Load("WindowSystem/Prefabs/QuestJournal/QuestJournalPane") as GameObject;
			}

			public override void Open(bool preserveParent = false) {
				base.Open();
				myRt = instance.GetComponent<UnityEngine.RectTransform>();
				tg = instance.GetComponent<ToggleGroup>();

				instance
					.transform.Find("Close")
					.GetComponent<Button>()
					.onClick.AddListener(delegate {Close();});

				tabs = myRt.Find("Scroll View").Find("Viewport").Find("Tabs");
				pane = myRt.Find("Panel").GetComponent<RectTransform>();

				LoadJournal();
			}

			void LoadJournal() {
				int i = 0;
				GameObject a;

				UnityEngine.RectTransform rt;
				foreach(Quest qt in Player.questLog){
					a = UnityEngine.Object.Instantiate(questJournalSelect);
					Toggle at = a.GetComponent<Toggle>();

					at.group = tg;
					at.isOn = i++ == 0? true : false;

					rt = a.GetComponent<RectTransform>();
					rt.SetParent(tabs, false);

					Text label = a.transform.Find("Handler").Find("Text").gameObject.GetComponent<Text>();

					if(qt.read){
						label.color = Journal.readColor;
					} else {
						label.color = Journal.unReadColor;
					}

					ConfigSelectToggle(at, qt, label);

				}

				a = UnityEngine.Object.Instantiate(space);
				rt = a.GetComponent<RectTransform>();
				rt.SetParent(tabs, false);
			}

			void ConfigSelectToggle(Toggle at, Quest qt, Text label) {
				RectTransform rt = at.GetComponent<RectTransform>();
				rt.Find("Handler").Find("Text").GetComponent<Text>().text = qt.name;
				if(at.isOn) InstantiateQuestPane(false, qt, label);
				at.onValueChanged.AddListener(delegate {InstantiateQuestPane(!at.isOn, qt, label);});
			}

			public void InstantiateQuestPane(bool selected, Quest quest, Text label){
				if(selected) return;
				label.color = Journal.readColor;
				//Removing All
				foreach (RectTransform t in pane) {
					UnityEngine.Object.Destroy(t.gameObject);
				}

				quest.read = true;
				EventManager.Trigger("QuestRead");

				GameObject a = UnityEngine.Object.Instantiate(questPane);
				RectTransform rt = a.GetComponent<RectTransform>();
				rt.SetParent(pane, false);

				RectTransform content = rt.Find("Viewport").Find("Content") as RectTransform;

				content.Find("QuestName").GetComponent<Text>().text = quest.name;

				content.Find("QuestDescription").GetComponent<Text>().text = quest.description;
				//content.Find("Image").GetComponent<Image>().sprite = quest.description;
				RectTransform reward = content.Find("Reward") as RectTransform;
				reward.Find("Exp").GetComponent<Text>().text = quest.exp+" exp";

				Text status = content.Find("Status").gameObject.GetComponent<Text>();
				
				status.text = (quest.status == Quest.QuestStatus.complete) ? "Completed" : "";

			}

		};

		public class QuestHelper
		{
			private UnityAction<object[]> OnItemAdd;
			private UnityAction<object[]> OnItemDelete;
			private UnityAction<object[]> OnItemCheck;

			GameObject item;
			Transform ctx;
			Dictionary<string, GameObject> instances;

			public QuestHelper(){

				ctx = UnityEngine.GameObject.Find("QuestHelper").transform.Find("Scroll View").Find("Viewport").Find("Content");

				item = Resources.Load("WindowSystem/Prefabs/QuestHelper/Item") as GameObject;
				instances = new Dictionary<string, GameObject>();

				OnItemAdd = new UnityAction<object[]> (OnItemAddCallback);
				OnItemDelete = new UnityAction<object[]> (OnItemDeleteCallback);
				OnItemCheck = new UnityAction<object[]> (OnItemCheckCallback);

				EventManager.AddListener("QuestHelperItemAdd", OnItemAdd);
				EventManager.AddListener("QuestHelperItemDelete", OnItemDelete);
				EventManager.AddListener("QuestHelperItemCheck", OnItemCheck);
			}

			void OnItemAddCallback(object[] param){
				string key = (string)param[0];
				GameObject it = UnityEngine.Object.Instantiate(item);

				it.transform.SetParent(ctx, false);
				it.transform.Find("Label").GetComponent<Text>().text = ((DatabaseDictionary)GameController.database.Find(key)).GetLabel();

				instances.Add(key, it);
			}

			void OnItemDeleteCallback(object[] param){
				string key = (string)param[0];

				GameObject _value = null;
				if(instances.TryGetValue(key, out _value)){
					instances.Remove(key);
					UnityEngine.Object.Destroy(_value);
				}
			}

			void OnItemCheckCallback(object[] param){
				string key = (string)param[0];

				GameObject _value = null;
				if(instances.TryGetValue(key, out _value)){
					_value.GetComponent<Toggle>().isOn = true;
				}
			}
		}
	}
}