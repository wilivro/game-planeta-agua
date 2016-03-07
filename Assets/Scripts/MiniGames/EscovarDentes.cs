using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ItemEscovarDentes
{
	public string text;
	public int value;
	public int index;

	

	public ItemEscovarDentes(string _text, int _value) {
		text = _text;
		value = _value;
		index = 0;
	}
}

public class EscovarDentes : MonoBehaviour {

	// Use this for initialization
	Dropdown dd;
	List<ItemEscovarDentes> itens;

	public int[] template;
	public int score;
	void Start () {
		dd = GameObject.Find("Dropdown").GetComponent<Dropdown>();
		itens = new List<ItemEscovarDentes>();
		template = new int[] {3, 1, 4, 2, 5, 1, 4};
		score = template.Length*0;
	}
	
	// Update is called once per frame
	void DrawListItem() {
		Transform root = GameObject.Find("List").transform;
		Transform mock = null;
		foreach (Transform t in root) {
		    if(t.gameObject.tag == "Mock") {
		    	mock = t;
		    	continue;
		    };
		    Destroy(t.gameObject);
	 	}

		for(int i = 0; i < itens.Count; i++) {
			Transform ist = Instantiate(mock);
			ist.SetParent(root);
			ist.gameObject.tag = "Untagged";
			ist.gameObject.active = true;

			RectTransform rt = ist.GetComponent<RectTransform>();

			rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 9.4f, 0f);
			rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,  9.4f, -19f);
			rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, ((20f*i) - 92.2f), 0f);
			rt.sizeDelta = new Vector2(-19f, 20f);

			rt.localScale = new Vector3(1,1,1);
			rt.anchorMin = new Vector2(0f,0.5f);
			rt.anchorMax = new Vector2(1f,0.5f);
			rt.pivot = new Vector2(0.5f,0.5f);

			ist.Find("Text").GetComponent<Text>().text = itens[i].text;
			int index = i;
			itens[i].index = index;
			ist.Find("Button").GetComponent<Button>().onClick.AddListener(delegate { OnDelete(index); });
		}
	}

	void OnDelete(int value){
		itens.RemoveAt(value);
		DrawListItem();
	}

	void OnClickSair(){
		Application.LoadLevel("cidade");
	}

	public void OnAddItem() {

		string text = dd.options[dd.value].text;
		int value = dd.value;

		if(value == 0) return;

		itens.Add(new ItemEscovarDentes(text, value));
		DrawListItem();
	}

	IEnumerator End(bool torneiraAberta) {
		string text = "Parabens Voce deixou sempre a torneira fechada. Continue assim.";
		if(torneiraAberta){
			text = "Leo, da proxima vez feche a torneira sempre q abrir";
		}

		Transform panel = GameObject.Find("Cine").transform;
		Text ctx = panel.Find("Text").GetComponent<Text>();
		Transform button = panel.Find("Sair");
		button.gameObject.active = true;
		Button buttonClick = button.GetComponent<Button>();
		ctx.text = "";
		for(int i = 0; i < text.Length; i++){
			try {
				ctx.text += text[i];
			} catch {

			}
			yield return new WaitForSeconds(0.05f);

		}

		buttonClick.onClick.AddListener(delegate { OnClickSair(); });
		yield return null;
	}

	public void OnClickEscovar(){

		bool torneiraAberta = false;
		int past = 0;

		for(int i = 0; i < itens.Count; i++) {
			if(past == 1 && itens[i].value != 4){
				torneiraAberta = true;
				score -= 10;
			} else {
				if(itens[i].value == template[i]){
					score += 10;
				}
			}
			past = itens[i].value;
			print(past);
		}

		int _score = PlayerPrefs.GetInt("Score");
		PlayerPrefs.SetInt("Score", score+_score);
		PlayerPrefs.Save();
		Player.inventory.content.Add(new Item("Escovar os dentes"));
		print(Player.inventory.content.Count);

		StartCoroutine(End(torneiraAberta));

	}
}