using UnityEngine;
using System.Collections;
using Rpg;

public class WarpLixeira : WarpBehaviour, IInteractable {


	GameObject glow;
	GameObject glowInstance;

	public void Start () {
		base.Start();

		glow = Resources.Load("Items/Animations/glow") as GameObject;
		
		glowInstance = Instantiate(glow);
		glowInstance.transform.SetParent(transform, false);
	}


	public override bool Teleporter() {
		Application.LoadLevel(4);
		return true;
	}

	public override void NotReady() {
	}

	public override bool AutoInteract() {
		return false;
	}
}
