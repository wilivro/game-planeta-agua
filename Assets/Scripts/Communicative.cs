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
 		dialogWindow.Config(myBehaviour.charName, "Characters/Leo/face");
	}

	public int GetDialogIndex() {
		int quest = PlayerPrefs.GetInt("Quest");
		int subQuest = PlayerPrefs.GetInt("SubQuest");

		return (subQuest.CompareTo(myBehaviour.subQuest) +2)*Convert.ToInt32(myBehaviour.quest==quest);

	}

	public Speech GetSpeech(){
		int dialog = GetDialogIndex(); 

		return myBehaviour.dialogs[dialog].speechs[actualSpeech];
	}

	public bool DialogEnd(){
		int dialog = GetDialogIndex();

		return actualSpeech == myBehaviour.dialogs[dialog].speechs.Count;
	}

	public void WaitInteraction() {
		if(actualColider == null || actualColider.gameObject.tag != "Player") return;

		if(CrossPlatformInputManager.GetButton("Submit") && myBehaviour.canInteract) {

			dialogWindow.Destroy();
			myBehaviour.canInteract = false;
			
			if(DialogEnd()) {

				dialogWindow.Destroy();

				int dialogIndex = GetDialogIndex();
				if(myBehaviour.dialogs[dialogIndex].questLog != null && myBehaviour.dialogs[dialogIndex].questLog.itens.Count > 0){
					string questLog = "";
					for(int i = 0; i < myBehaviour.dialogs[dialogIndex].questLog.itens.Count; i++) {
						questLog += myBehaviour.dialogs[dialogIndex].questLog.itens[i]+"|";
					}

					PlayerPrefs.SetString("QuestLog", questLog);
					PlayerPrefs.Save();

					return;
				}

				actualSpeech = 0;
				int quest = PlayerPrefs.GetInt("Quest");
				int subQuest = PlayerPrefs.GetInt("SubQuest");
				if(myBehaviour.subQuest == subQuest && myBehaviour.quest == quest){
					PlayerPrefs.SetInt("SubQuest", subQuest+1);
					PlayerPrefs.Save();
				}

			} else {
				bool hasChioces = dialogWindow.Show(this);
				if(!hasChioces) actualSpeech++;
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
