using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Rpg;
using Rpg.QuestSystem;
using Rpg.DialogueSystem;

public class WarpBehaviour : MonoBehaviour, IInteractable {

	// Use this for initialization
	public string name;

	public Warp self;
	private UnityAction<object[]> OnFader;
	private bool teleporting;
	private GameObject fader;

	public void Start () {

		fader = Resources.Load("Effects/Prefabs/Fader") as GameObject;
		OnFader = new UnityAction<object[]> (OnFaderCallback);
		EventManager.AddListener("FadeToBlack", OnFader);

		if(name == "") return;
		self = new Warp(name);
		teleporting = false;
	}

	void OnFaderCallback(object[] param) {
		if(!teleporting) return;
		if(Teleporter()) {
			GameController.player.Save();
		}
		teleporting = false;
	}

	public virtual bool Teleporter() {
		return true;
	}

	public virtual void NotReady(){
		if(name == "") return;
		if(self.message == null) return;

		Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
		screenPos += new Vector3(0, 45, 0);

		self.span.Open(self.message, screenPos);
	}

	public void OnInteractEnter(GameObject from) {
		if(name == "" || self.Ready()) {
			if(teleporting) return;
			teleporting = true;
			GameObject it = Instantiate(fader);
			it.transform.SetParent(GameController.canvas,false);
			return;
		}

		NotReady();
	}

	public void OnInteractExit(GameObject from){
		if(name == "") return;
		self.span.Close();
	}

	public virtual bool AutoInteract() {
		return true;
	}

	public void Interact(GameObject to) {}
}
