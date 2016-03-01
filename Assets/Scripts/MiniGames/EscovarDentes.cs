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
	List<ItemEscovarDentes> items;

	public int[] template;
	public int score;
	void Start () {
		dd = GameObject.Find("Dropdown").GetComponent<Dropdown>();
		items = new List<ItemEscovarDentes>();
		template = new int[] {3, 1, 4, 2, 5, 1, 4};
		score = template.Length*10;
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

		for(int i = 0; i < items.Count; i++) {
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

			ist.Find("Text").GetComponent<Text>().text = items[i].text;
			int index = i;
			items[i].index = index;
			ist.Find("Button").GetComponent<Button>().onClick.AddListener(delegate { OnDelete(index); });
		}
	}

	void OnDelete(int value){
		items.RemoveAt(value);
		DrawListItem();
	}

	public void OnAddItem() {

		string text = dd.options[dd.value].text;
		int value   = dd.value;

		if(value == 0) return;

		items.Add(new ItemEscovarDentes(text, value));
		DrawListItem();
	}

	public void OnClickEscovar(){

		for(int i = 0; i < items.Count; i++) {
			if(i >= template.Length || items[i].value != template[i]){
				score -= 10;
			}
		}

		score = (score*100)/(template.Length*10);
	}
}