using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Rpg;
using Rpg.QuestSystem;

public class ControlCanvas : MonoBehaviour {

	// Use this for initialization
	Canvas canvas;
	void Start () {
		canvas = GetComponent<Canvas>();
	}
	
	// Update is called once per frame
	void Update () {
		int qtd = 0;
		foreach(Quest qt in Player.questLog){
			if(qt.read) qtd++;
		}

		print(qtd);
	}
}
