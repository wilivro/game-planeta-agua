using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ItemMiniGameEscolhas
{
	public string text;
	public int value;
	public int index;

	

	public ItemMiniGameEscolhas(string _text, int _value) {
		text = _text;
		value = _value;
		index = 0;
	}
}

public class MiniGameEscolhas : MonoBehaviour {

	// Use this for initialization
	public Dropdown dd;
	public List<ItemMiniGameEscolhas> itens;

	public int[] template;
	public int score;
	public Item giveItem;

	public string correctMsg;
	public string wrongMsg;

	void Start () {
		dd = GameObject.Find("Dropdown").GetComponent<Dropdown>();
		itens = new List<ItemMiniGameEscolhas>();
		//template = new int[] {3, 1, 4, 2, 5, 1, 4};
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

	public void OnClickSair(){
		Joystick.ShowJoy();
		Joystick.ShowButtons();
		Destroy(gameObject);
		MiniGameOpen.opened = false;
	}

	public void OnAddItem() {

		string text = dd.options[dd.value].text;
		int value = dd.value;

		if(value == 0) return;

		itens.Add(new ItemMiniGameEscolhas(text, value));
		DrawListItem();
	}

	public IEnumerator End(bool condition) {
		string text = correctMsg;
		if(condition){
			text = wrongMsg;
		}

		Transform panel = GameObject.Find("Cine").transform;
		Text ctx = panel.Find("Text").GetComponent<Text>();
		Transform button = panel.Find("Sair");
		Transform buttonRun = panel.Find("Button");

		Destroy(buttonRun.gameObject);

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

	
}