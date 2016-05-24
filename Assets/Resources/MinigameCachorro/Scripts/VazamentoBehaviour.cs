using UnityEngine;
using System.Collections;
using Rpg;

public class VazamentoBehaviour : MonoBehaviour, IInteractable {

	// Use this for initialization
	Animator[] anim;
	SpriteRenderer sprite;
	bool closed;

	void Start () {
		anim = GetComponentsInChildren<Animator>();
		sprite = GetComponent<SpriteRenderer>();

		string status = Log.GetValue("mn01pipe");
		if(status != null){
			sprite.color = new Color(255,255,255,0);
		}
	
	}
	
	public void OnInteractEnter(GameObject from) {
		if(closed) return;
		anim[0].SetTrigger("valve");
		Log.Register("mn01pipe", "Closed");
		string status = Log.GetValue("mn01dog");
		if(status != null){
			Log.Register("mn01", "Done");
		}
	}

	public void OnInteractExit(GameObject from){}
	public void Interact(GameObject to) {}
	public bool AutoInteract() {
		return false;
	}
}
