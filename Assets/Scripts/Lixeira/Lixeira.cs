using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Lixeira : MiniGameEscolhas {

	public static Transform itensContainer;
	public static GameObject mock;


	// Use this for initialization
	void Start () {
		mock = Resources.Load ("prefabs/Item") as GameObject;	
		itensContainer = GameObject.Find ("Itens").transform;

		Draw ();

		transform.Find("Title").Find("Exit").GetComponent<Button>().onClick.AddListener(delegate { OnClickSair(); });


	}

	public static void Draw(){
		try {
			int count = Player.inventory.content.Count;
			int left = -1;
			int top = 0;
			for (int i = 0; i < count; i++) {


				left += Convert.ToInt32 ((i % 3) == 0);

				Item item = Player.inventory.content[i];
				Garbage itemG = item.gameObject.GetComponent<Garbage> ();

				if(!itemG) continue;

				SpriteRenderer imgItem = item.gameObject.GetComponent<SpriteRenderer> ();

				GameObject it = Instantiate(mock);
				it.gameObject.transform.SetParent (itensContainer);
				Garbage g = it.GetComponent<Garbage> ();
				Image img = it.GetComponent<Image> ();
				img.sprite = imgItem.sprite;

				g.name = itemG.name;
				g.type = itemG.type;

				RectTransform rt = it.GetComponent<RectTransform>();

				//rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 9.4f, 0f);
				rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,  10f + (left*32f), 0);
				rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, ((32f*(i%3))+10f), 0f);
				rt.sizeDelta = new Vector2(32f, 32f);

				rt.localScale = new Vector3(1,1,1);
				rt.anchorMin = new Vector2(0f,1f);
				rt.anchorMax = new Vector2(0f,1f);
				rt.pivot = new Vector2(0f,1f);
			}
		} catch {}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
