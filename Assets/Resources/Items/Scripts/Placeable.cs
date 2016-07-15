using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using Rpg;

public class Placeable : IInteractable {

	public Item accept;
	public SpriteRenderer image;
	public bool placed;
	public int score;
    

    void Start () {
		//image = gameObject.GetComponent<SpriteRenderer>();
		placed = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnInteractEnter(GameObject from)
    {
        /*
        if (placed) return;
        if (actualColider == null || actualColider.gameObject.tag != "Player") return;
        if (CrossPlatformInputManager.GetButton("Submit"))
        {
            bool contains = Player.inventory.Contains(accept);
            if (contains)
            {
                Player.inventory.Remove(accept);
                image.sprite = accept.GetComponent<SpriteRenderer>().sprite;
                placed = true;
                int _score = PlayerPrefs.GetInt("Score");
                PlayerPrefs.SetInt("Score", score + _score);
                PlayerPrefs.Save();
            }
        }
        */
    }

    public bool AutoInteract()
    {
        return false;
    }

    public void OnInteractExit(GameObject from) { }

    public void Interact(GameObject to) { }
}
