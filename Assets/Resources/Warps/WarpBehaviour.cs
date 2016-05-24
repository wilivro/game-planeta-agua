using UnityEngine;
using System.Collections;
using Rpg;
using Rpg.QuestSystem;
using Rpg.DialogueSystem;

public class WarpBehaviour : MonoBehaviour, IInteractable {

	// Use this for initialization
	public string name;

	public Warp self;

	public void Start () {
		self = new Warp(name);
	}

	public virtual bool Teleporter() {
		return true;
	}

	public virtual void NotReady(){
		if(self.message == null) return;

		Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
		screenPos += new Vector3(0, 45, 0);

		self.span.Open(self.message, screenPos);
	}

	public void OnInteractEnter(GameObject from) {
		if(self.Ready()) {

			if(Teleporter()) {
				GameController.player.Save();
			}

			return;
		}

		NotReady();
	}

	public void OnInteractExit(GameObject from){
		self.span.Close();
	}

	public virtual bool AutoInteract() {
		return true;
	}

	public void Interact(GameObject to) {}
}
