using UnityEngine;
using System.Collections;
using Rpg;

public class VazamentoBehaviour : MonoBehaviour, IInteractable {

	// Use this for initialization
	Animator[] anim;
	GameObject agua;
	bool closed;

	void Start () {
		anim = GetComponentsInChildren<Animator>();
		agua = transform.Find("agua").gameObject;

		string status = Log.GetValue("mn01");
		if(status != null){
			closed = true;
			Destroy(agua);
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
