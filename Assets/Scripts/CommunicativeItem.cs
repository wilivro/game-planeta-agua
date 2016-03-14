using UnityEngine;
using System.Collections;

public class CommunicativeItem : Communicative {

	public Rigidbody2D rbody;
	public Animator anim;


	void Start () {
		base.Start();

 		rbody = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		if(GetDialogIndex() != 2){
			if(actualColider != null) base.OnTriggerExit2D(actualColider);

			gameObject.GetComponent<BoxCollider2D>().enabled = false;
			Transform glow = gameObject.transform.Find("Glow");
			if(glow) glow.gameObject.active = false;
			return;
		} else {
			gameObject.GetComponent<BoxCollider2D>().enabled = true;
			Transform glow = gameObject.transform.Find("Glow");
			if(glow) glow.gameObject.active = true;
		}

		WaitInteraction();
	}

}
