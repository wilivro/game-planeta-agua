using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	// Use this for initialization
	public Animator anim;
	void Start () {
		anim = gameObject.GetComponent<Animator>();
	}

	void Open() {
		anim.SetTrigger("Open");
	}

	void OnTriggerEnter2D(Collider2D col) {
		Open();
	}
}
