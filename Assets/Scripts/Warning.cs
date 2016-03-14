using UnityEngine;
using System.Collections;

public class Warning : Communicative {

	// Use this for initialization

	//public DialogWindow dialogWindow;

	public void Start () {
		base.Start();
		
	}

	public override bool DialogEnd() {
		bool end = base.DialogEnd();
		if(end) Destroy(transform.parent.gameObject);
		return end;
	}
	
	// Update is called once per frame
	void Update () {
		WaitInteraction();
		if(actualColider != null && actualColider.gameObject.tag == "Player") {
			
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		OnTriggerEnter2D(other.collider);
	}

	void OnTriggerEnter2D(Collider2D other) {

		base.OnTriggerEnter2D(other);

		if(other.gameObject.tag == "Player"){
			if(isNPC){
				bool hasChioces = dialogWindow.Show(this);
				if(!hasChioces) actualSpeech++;
				return;
			}
		}
			
	}
}
