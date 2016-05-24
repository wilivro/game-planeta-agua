using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using Rpg;

public class JournalButtonBehaviour : MonoBehaviour {

	// Use this for initialization
	private UnityAction<object[]> OnQuestAddListener;
	private UnityAction<object[]> OnQuestReadListener;

	Text badge;
	int qtd;
	GameObject effect1pp;
	void Awake () {
		badge = transform.Find("Badge").Find("Text").gameObject.GetComponent<Text>();
		OnQuestAddListener = new UnityAction<object[]> (OnQuestAdd);
		OnQuestReadListener = new UnityAction<object[]> (OnQuestRead);
		EventManager.AddListener("QuestAdd", OnQuestAddListener);
		EventManager.AddListener("QuestRead", OnQuestReadListener);
		qtd = 0;

		effect1pp = Resources.Load("Effects/Prefabs/1pp") as GameObject;

		IsEmpty();
	}

	void OnQuestAdd(object[] param){
		qtd++;

		Component[] others = GetComponentsInChildren (typeof (Effect1ppBehaviour));

		GameObject a = Instantiate(effect1pp);
		a.transform.SetParent(transform, false);
		a.transform.Find("Text").GetComponent<Text>().text = "+"+(others.Length+1).ToString();

		badge.text = qtd.ToString();
		badge.gameObject.active = true;
		if(qtd == 0) badge.gameObject.active = false;
	}

	bool IsEmpty(){
		if(qtd == 0) {
			badge.gameObject.transform.parent.gameObject.active = false;
			return true;
		}

		badge.gameObject.transform.parent.gameObject.active = true;

		return false;
	}

	void OnQuestRead(object[] param){
		
		if(IsEmpty()) return;

		qtd--;
		badge.text = qtd.ToString();

		IsEmpty();
	}
}
