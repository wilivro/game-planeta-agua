using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class ItemInteractable : Interactable
{
	public string name;

	public Item self;

	void Start() {
		base.Start();
		name = gameObject.name;
		self = new Item(name);
	}

	void Update() {
		if(CrossPlatformInputManager.GetButton("Submit")) {
			Player.inventory.content.Add(self);
			Destroy(transform.gameObject);
		}
	}
	
}