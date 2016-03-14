using UnityEngine;
using System.Collections;

public class Warning : Communicative {

	// Use this for initialization

	//public DialogWindow dialogWindow;

	void Start () {
		base.Start();
		
	}
	
	// Update is called once per frame
	void Update () {
		WaitInteraction();
	}

	void OnCollisionEnter2D(Collision2D other) {
		OnTriggerEnter2D(other.collider);
	}
	void OnTriggerEnter2D(Collider2D other) {

		base.OnTriggerEnter2D(other);

		if(other.gameObject.tag == "Player"){
			if(isNPC){
				dialogWindow.Show(myBehaviour.dialogs[0].speechs[0], null);
				return;
			}
		}
			
	}
}
