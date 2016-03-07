using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class ItemInteractable : Interactable
{
	string name;

	Item self;

	void Start() {
		base.Start();
		name = gameObject.name;
		self = new Item(name);
	}

	void Update() {

	}
	
}