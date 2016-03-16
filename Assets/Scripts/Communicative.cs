using UnityEngine;
using System;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Communicative : Interactable {

	public DialogWindow dialogWindow;
	public int actualSpeech;
	public bool inChoice;
	

	public void Start(){
		base.Start();

		dialogWindow = gameObject.AddComponent<DialogWindow>() as DialogWindow;
		dialogWindow.Config(myBehaviour.charName, "Characters/"+myBehaviour.charName+"/face");
	}

	public int GetDialogIndex() {
		int quest = PlayerPrefs.GetInt("Quest");
		int subQuest = PlayerPrefs.GetInt("SubQuest");

		if(quest < myBehaviour.quest){
			return 0;
		}

		if(quest > myBehaviour.quest){
			return 5;
		}

		if(Player.hasQuestLog && PlayerPrefs.GetString("QuestLog") != ""){
			return 4;
		}

		return (subQuest.CompareTo(myBehaviour.subQuest) +2);

	}

	public Speech GetSpeech(){
		int dialog = GetDialogIndex(); 

		return myBehaviour.dialogs[dialog].speechs[actualSpeech];
	}

	public virtual bool DialogEnd(){
		int dialog = GetDialogIndex();

		bool end = actualSpeech == myBehaviour.dialogs[dialog].speechs.Count;

		if(!end)
			return false;

		dialogWindow.Destroy();

		int dialogIndex = GetDialogIndex();
		string questLog = "";
		if(myBehaviour.dialogs[dialogIndex].questLog != null && myBehaviour.dialogs[dialogIndex].questLog.itens.Count > 0){
			Player.hasQuestLog = true;
			for(int i = 0; i < myBehaviour.dialogs[dialogIndex].questLog.itens.Count; i++) {
				questLog += myBehaviour.dialogs[dialogIndex].questLog.itens[i]+"|";
			}

			PlayerPrefs.SetString("QuestLog", questLog);
			PlayerPrefs.Save();

		}

		if(myBehaviour.dialogs[dialogIndex].give != null && myBehaviour.dialogs[dialogIndex].give.itens.Count > 0){

			for(int i = 0; i < myBehaviour.dialogs[dialogIndex].give.itens.Count; i++) {
				GameObject it = Resources.Load("Prefabs/Itens/"+myBehaviour.dialogs[dialogIndex].give.itens[i]) as GameObject;
				it = Instantiate(it);
				it.active = false;
				Item itt = it.GetComponent<Item>();
				Player.inventory.Add(itt);
				Destroy(it);
			}

		}

		actualSpeech = 0;
		int quest = PlayerPrefs.GetInt("Quest");
		int subQuest = PlayerPrefs.GetInt("SubQuest");

		questLog = PlayerPrefs.GetString("QuestLog");
		
		if(myBehaviour.subQuest == subQuest && myBehaviour.quest == quest){
			PlayerPrefs.SetInt("SubQuest", subQuest+1);
			PlayerPrefs.Save();
		}

		if(myBehaviour.dismissible && myBehaviour.quest == quest && questLog == ""){
			PlayerPrefs.SetInt("Quest", quest+1);
			PlayerPrefs.SetInt("SubQuest", 0);
			PlayerPrefs.Save();
		}


		return true;
	}

	public void Next() {
		bool hasChioces = dialogWindow.Show(this);
		if(!hasChioces) {
			Speech s = GetSpeech();
			actualSpeech = (s.gotoSpeech > 0) ? s.gotoSpeech : actualSpeech+1;
		}
	}

	public void WaitInteraction() {
		if(actualColider == null || actualColider.gameObject.tag != "Player") return;

		if(CrossPlatformInputManager.GetButton("Submit") && myBehaviour.canInteract) {

			dialogWindow.Destroy();
			myBehaviour.canInteract = false;
			
			if(!DialogEnd()) {
				Next();	
			}

			return;
		}

		if(CrossPlatformInputManager.GetButton("Cancel")) {
			actualSpeech = 0;
			myBehaviour.canInteract = true;
			dialogWindow.Destroy();
			return;
		}
	}
}
