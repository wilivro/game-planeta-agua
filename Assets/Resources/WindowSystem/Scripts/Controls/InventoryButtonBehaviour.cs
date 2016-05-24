using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using Rpg;

public class InventoryButtonBehaviour : MonoBehaviour {

	// Use this for initialization
	private UnityAction<object[]> OnQuestItemListener;

	GameObject effect1pp;
	void Awake () {
		OnQuestItemListener = new UnityAction<object[]> (OnItemAdd);
		EventManager.AddListener("ItemAdd", OnQuestItemListener);

		effect1pp = Resources.Load("Effects/Prefabs/1pp") as GameObject;
	}

	void OnItemAdd(object[] param){

		Component[] others = GetComponentsInChildren (typeof (Effect1ppBehaviour));

		GameObject a = Instantiate(effect1pp);
		a.transform.SetParent(transform, false);
		a.transform.Find("Text").GetComponent<Text>().text = "+"+(others.Length+1).ToString();

	}

}
