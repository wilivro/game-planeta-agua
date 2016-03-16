using UnityEngine;
using System;
using System.Collections;

public class Walter : MonoBehaviour {

	// Use this for initialization
	public GameObject walter;
	void Awake () {
		walter = Resources.Load ("Prefabs/Walter") as GameObject;
		Vector3 sp = transform.localScale;
		Vector3 s = new Vector3 (1.0f / sp.x, 1.0f / sp.y, 1.0f / sp.z);

		Vector3 tamanho = new Vector3(s.x , s.y, 0);

		Vector3 dx = new Vector3 (tamanho.x, 0, 0);
		Vector3 dy = new Vector3 (0, tamanho.y, 0);

		int qx = Convert.ToInt32(sp.x);
		int qy = Convert.ToInt32(sp.y);

		Vector3 p0 = new Vector3( (-1) * (qx / 2.0f) * tamanho.x + (tamanho.x / 2.0f), (qy / 2.0f) * tamanho.y - (tamanho.y / 2.0f), 0);

		GameObject clone;
		SpriteRenderer mySprite;

		mySprite = GetComponent<SpriteRenderer> ();

		SpriteRenderer sprite;
		gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		for (int i = 0; i < qx; i++) {
			for (int j = qy - 1; j >= 0; j--) {
				clone = Instantiate (walter) as GameObject;

				clone.transform.SetParent (transform);

				clone.transform.localScale = s;
				clone.transform.localPosition = p0 + i * dx - j * dy;

				sprite = clone.GetComponent<SpriteRenderer> ();
				sprite.flipX = mySprite.flipX;
				sprite.color = mySprite.color;
				sprite.sortingOrder = mySprite.sortingOrder;

			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
