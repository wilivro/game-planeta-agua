using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


using Window;
using Rpg;
using Rpg.WindowSystem;
using Rpg.QuestSystem;

namespace Rpg
{

	namespace DialogueSystem
	{

		[Serializable]
		public class Dialogue
		{
			public string quest;
			public Speech[] before;
			public Speech[] request;
			public Speech[] inProgress;
			public Speech[] after;
			public string[] preRequirements;
			public string[] requirements;
			public bool finishHere;
		}

		[Serializable]
		public class CheckItem
		{
			public string[] item;
			public int[] qtd;
			public int[] gotoSpeech;
		}

		[Serializable]
		public class CheckGold
		{
			public int qtd;
			public int[] gotoSpeech;
		}

		[Serializable]
		public class Speech
		{
			public string name;
			public string text;
			public Choice[] choice;
			public int gotoSpeech = -1;
			public string[] giveQuest;
			public string[] giveItem;
			public string[] getItem;
			public bool exit;
			public int[] completeQuest;
			public string[] check;
			public LogData[] registerLog;
			public string image;
			public CheckItem checkItem;
			public CheckGold checkGold;

		}

		[Serializable]
		public class Choice
		{
			public string text;
			public int posImage = 0;
			public string imageBase;
			public bool correct;
			public int gotoSpeech = -1;
			public string[] giveQuest;
			public string[] giveItem;
			public string[] getItem;
			public int[] completeQuest;
			public LogData[] registerLog;
			public CheckItem checkItem;
			public CheckGold checkGold;
		}

		public class DialogueControl
		{
			Dialogue[] dialogue;
			Speech[] dummy;
			WindowSystem.Dialogue dialogueWindow;
			Transform canvas;
			string npcName;

			Speech[] actualDialogue;

			public DialogueControl(Dialogue[] _dialogue, Speech[] _dummy, string _npcName){
				canvas = GameObject.Find("GameController").transform;
				dialogue = _dialogue;
				npcName = _npcName;
				dummy = _dummy;

				dialogueWindow = new WindowSystem.Dialogue(canvas, npcName);
				
			}

			bool IsSubArray<T>(T[] universe, T[] planet) {

				bool exists = false;
				for(int i = 0; i < planet.Length; i++) {
					for(int j = 0; j < universe.Length; j++) {
						if(planet[i].Equals(universe[j])){
							exists = true;
							break;
						}

					}
					if(!exists) return false;
				}



				return true;
			}

			bool HasPreRequirements(string[] arr) {
				return Log.HasKey(arr);
			}

			bool RequestQuest(){
				//lista de quests do player
				if(dialogue == null) return false;

				string[] questLog = Player.questLog
								//.Where(q => q.status == Quest.QuestStatus.archived)
								.Select(q => q.id)
								.ToArray();


				Dialogue[] D = dialogue.Where(d =>
					(Array.IndexOf(questLog, d.quest) < 0) &&
					(HasPreRequirements(d.preRequirements))
				).ToArray();

				if(D.Length >= 1) {
					actualDialogue = D[0].request;
					if(actualDialogue == null) return false;
					dialogueWindow.Open(actualDialogue);
					return true;
				}



				return false;
			}

			bool HasQuest(){
				if(dialogue == null) return false;

				string[] questLog = Player.questLog
								.Where(q => q.status != Quest.QuestStatus.archived)
								.Select(q => q.id)
								.ToArray();

				Dialogue[] D = dialogue
									.Where(d => Array.IndexOf(questLog, d.quest) >= 0 && (HasPreRequirements(d.preRequirements)))
									.ToArray();

				if(D.Length > 0) {
					Dialogue d = D.First();
					Quest what = Player.questLog.Where(q => q.id == d.quest).Last();

					if(what.status == Quest.QuestStatus.archived){
						return false;
					}
					if(what.status == Quest.QuestStatus.complete || (HasPreRequirements(d.requirements) && d.finishHere) ) {
						actualDialogue = d.after;
						what.Archive();
						
					}
					else
						actualDialogue = d.inProgress;

					dialogueWindow.Open(actualDialogue);
					return true;
				}

				return false;
			}

			bool NotReadyQuest(){
				if(dialogue == null) return false;

				string[] questLog = Player.questLog
								//.Where(q => q.status == Quest.QuestStatus.archived)
								.Select(q => q.id)
								.ToArray();

				Dialogue[] D = dialogue.Where(d =>
					(Array.IndexOf(questLog, d.quest) >= 0) &&
					(!HasPreRequirements(d.preRequirements))
				).ToArray();

				if(D.Length >= 1) {
					actualDialogue = D[0].before;
					dialogueWindow.Open(actualDialogue);
					return true;
				}

				return false;
			}

			void DummySpeech() {
				System.Random rnd = new System.Random();
				int what = rnd.Next(0, dummy.Length);
				actualDialogue = new Speech[1] {dummy[what]};
				dialogueWindow.Open(actualDialogue);
			}

			public void Start() {
				if(HasQuest()) return;
				if(RequestQuest()) return;
				if(NotReadyQuest()) return;

				DummySpeech();

			}
		}

	}

	namespace WindowSystem
	{
		public class Dialogue : WindowBase
		{
			private UnityAction<object[]> OnNextPage;
			private UnityAction<object[]> OnWriteEnd;

			GameObject text;

			public DialogueSystem.Speech[] dialogue;
			public int page;
			string npcName;

			GameObject choices;
			GameObject choice;
			GameObject chs;

			public Dialogue(Transform _canvas, string _npcName) : base(_canvas) {
				prefab = Resources.Load("WindowSystem/Prefabs/Dialogue/DialogueWindow") as GameObject;
				choices = Resources.Load("WindowSystem/Prefabs/Dialogue/choices") as GameObject;
				choice = Resources.Load("WindowSystem/Prefabs/Dialogue/choice") as GameObject;

				OnNextPage = new UnityAction<object[]> (OnNextPageCallback);
				EventManager.AddListener("DialoguePageComplete", OnNextPage);


				OnWriteEnd = new UnityAction<object[]> (OnWriteEndCallback);
				EventManager.AddListener("DialogueWriteEnd", OnWriteEnd);

				npcName = _npcName;
			}

			void TriggerGive<T1, T2>(T2[] arr) {
				if(arr == null) return;
				if(typeof(T1) == typeof(Quest))
				{
						EventManager.Trigger("PlayerReciveQuest", new object[1]{arr});
						return;
				}
				if(typeof(T1) == typeof(Item))
				{
						EventManager.Trigger("PlayerReciveItem", new object[1]{arr});
						return;
				}
			}

			void TriggerGet<T1, T2>(T2[] arr) {
				if(arr == null) return;
				if(typeof(T1) == typeof(Item))
				{
						EventManager.Trigger("PlayerRemoveItem", new object[1]{arr});
						return;
				}
			}

			void RegisterLog(LogData[] registerLog){
				if(registerLog == null) return;
				Log.Register(registerLog);
			}

			void OnNextPageCallback(object[] param) {
				if(dialogue == null) return;
				RegisterLog(dialogue[page].registerLog);
				TriggerGive <Quest,   string> (dialogue[page].giveQuest);
				TriggerGive <Item, string> (dialogue[page].giveItem);
				TriggerGet  <Item, string> (dialogue[page].getItem);

				EventManager.Trigger("PlayerCompleteQuest", new object[1]{dialogue[page].completeQuest});

				if(dialogue[page].exit != null && dialogue[page].exit){
					Close();
					return;
				}

				if(dialogue[page].gotoSpeech >= 0) {
					page = dialogue[page].gotoSpeech;
				} else {
					page++;
				}

				ShowSpeech();
			}

			void OnChoiceCallback(DialogueSystem.Choice _ch){
				if(dialogue == null) return;
				TriggerGive <Quest,   string> (dialogue[page].giveQuest);
				TriggerGive <Item, string> (dialogue[page].giveItem);
				TriggerGet  <Item, string> (dialogue[page].getItem);

				TriggerGive <Quest,   string>  (_ch.giveQuest);
				TriggerGive <Item, string>  (_ch.giveItem);
				TriggerGet  <Item, string>  (_ch.getItem);

				EventManager.Trigger("PlayerCompleteQuest", new object[1]{_ch.completeQuest});

				page = _ch.gotoSpeech;
				UnityEngine.Object.Destroy(chs);
				ShowSpeech();
			}

			public void Open(DialogueSystem.Speech[] _d) {
				base.Open();
				dialogue = _d;
				page = 0;
				EventManager.Trigger("PlayerDialogueStart");
				ShowSpeech();
			}

			string FormatText(string _name) {
				string formatted = _name.Replace("{{name}}", npcName);
				formatted = formatted.Replace("{{player}}", Player.name);
				formatted = formatted.Replace("\t", "");
				formatted = formatted.Trim();

				return formatted;
			}

			void ShowSpeech() {

				if(page >= dialogue.Length) {
					dialogue = null;
					EventManager.Trigger("PlayerDialogueEnd");
					Close();
					return;
				};
				
				if(dialogue[page].checkGold.gotoSpeech != null) {
					if(Player.inventory.HasEnoughGold(dialogue[page].checkGold.qtd)) {
						if(dialogue[page].checkGold.gotoSpeech[0] != page){
							page = dialogue[page].checkGold.gotoSpeech[0];
							ShowSpeech();
							return;
						}
					} else {
						Debug.Log("Falhou");
						if(dialogue[page].checkGold.gotoSpeech[1] != page){
							page = dialogue[page].checkGold.gotoSpeech[1];
							ShowSpeech();
							return;
						}
					}
				}

				if(dialogue[page].checkItem.item != null) {
					for(int n = 0; n < dialogue[page].checkItem.item.Length; n++) {
						if(Player.inventory.HasItem(dialogue[page].checkItem.item[n], dialogue[page].checkItem.qtd[n])){
							continue;
						} else {
							if(dialogue[page].checkItem.gotoSpeech[1] != page){
								page = dialogue[page].checkItem.gotoSpeech[1];
								ShowSpeech();
								return;
							}
						}
					}

					if(dialogue[page].checkItem.gotoSpeech[0] != page){
						page = dialogue[page].checkItem.gotoSpeech[0];
						ShowSpeech();
						return;
					}
				}

				instance.transform.Find("Name").Find("Text").GetComponent<Text>().text = FormatText(dialogue[page].name);

				DatabaseImage dbi = (DatabaseImage) GameController.database.Find(dialogue[page].image);
				Sprite[] baseI = Resources.LoadAll<Sprite>(dbi.source);
				instance.transform.Find("Image").GetComponent<Image>().sprite = baseI[dbi.index];

				text = instance.transform.Find("Window").gameObject;
				text.SendMessage("Write", FormatText(dialogue[page].text));
			}

			void ShowChoices() {
				chs = UnityEngine.Object.Instantiate(choices);
				chs.transform.SetParent(instance.transform, false);
				chs.name = "Choices";

				for(int i=0; i < dialogue[page].choice.Length; i++) {
					GameObject ch = UnityEngine.Object.Instantiate(choice);
					ch.transform.SetParent(chs.transform, false);
					ch.transform.Find("Text").GetComponent<Text>().text = dialogue[page].choice[i].text;
					Sprite[] imgBase = Resources.LoadAll<Sprite>(dialogue[page].choice[i].imageBase);

					ch.transform.Find("Image").GetComponent<Image>().sprite = imgBase[dialogue[page].choice[i].posImage];

					DialogueSystem.Choice cho = dialogue[page].choice[i];

					Button bt = ch.GetComponent<Button>();
					bt.onClick.AddListener(delegate {
						OnChoiceCallback(cho);	
					});
				}
			}

			void OnWriteEndCallback(object[] param){
				if(dialogue == null) return;
				if(dialogue[page].choice == null) return;

				if(dialogue[page].choice.Length > 0)
					ShowChoices();
			}

		}
	}
	
}