using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Placeable : Interactable {

	public Item accept;
	public SpriteRenderer image;
	public bool placed;
	public int score;
	void Start () {
		image = gameObject.GetComponent<SpriteRenderer>();
		placed = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(placed) return;
		if(actualColider == null || actualColider.gameObject.tag != "Player") return;
        if(CrossPlatformInputManager.GetButton("Submit")) {
        	bool contains = Player.inventory.Contains(accept);
        	if(contains) {
        		Player.inventory.Remove(accept);
        		image.sprite = accept.GetComponent<SpriteRenderer>().sprite;
        		placed = true;
        		int _score = PlayerPrefs.GetInt("Score");
        		PlayerPrefs.SetInt("Score", score + _score);
        		PlayerPrefs.Save();
        	}
        }
	}

}
