using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;


public class Character : MonoBehaviour {

	Rigidbody2D rbody;
	Animator anim;

	public TextAsset behaviourFile;
	public bool isNPC;

	private GameObject actualColider;
	private bool initialInteractionCondition;
	private Behaviour myBehaviour;

	
	bool LoadBehaviour() {
		if(behaviourFile == null){
			return false;
		}

		try {
			XmlSerializer serializer = new XmlSerializer(typeof(Behaviour));
			MemoryStream stream = new MemoryStream(behaviourFile.bytes);
	 		myBehaviour = serializer.Deserialize(stream) as Behaviour;
	 		initialInteractionCondition = myBehaviour.canInteract;
	 		return true;
 		} catch {
 			return false;
 		}
	}

	void Start () {
		isNPC = LoadBehaviour();

		rbody = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		
	}

	void Movement() {
		Vector2 movement_vector = new Vector2();
		Vector2 axis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		if (Input.GetMouseButtonDown(0) && axis == Vector2.zero) {
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            print(mouse);
            //movement_vector = new Vector2(mouse.x, mouse.y);
        } else if(axis != Vector2.zero){
        	movement_vector = axis;
        } else {
        	//movement_vector = new Vector2(Math.Sign(nav.velocity.x), Math.Sign(nav.velocity.y));
        }

		if(movement_vector != Vector2.zero) {
			anim.SetBool("isWalking", true);
			anim.SetFloat("input_x", movement_vector.x);
			anim.SetFloat("input_y", movement_vector.y);
		} else {
			anim.SetBool("isWalking", false);
		}

		rbody.MovePosition(rbody.position + movement_vector*50*Time.deltaTime);

	}
	
	void WaitInteraction() {
		if(actualColider == null || actualColider.tag != "Player") return;

		if(Input.GetKey("space") && myBehaviour.canInteract) {
			myBehaviour.canInteract = false;
			print("Oi eu sou "+ myBehaviour.charName);
			//TODO Open window;
			return;
		}

		if(Input.GetKey("escape") && !myBehaviour.canInteract) {
			myBehaviour.canInteract = true;
			//TODO close window;
			return;
		}
	}

	// Update is called once per frame
	void Update () {
		if(!isNPC){
			Movement();
		} else {
			WaitInteraction();
		}
	}

	void OnCollisionEnter (Collision col)
    {
        actualColider = col.gameObject;
    }

    void OnCollisionExit (Collision col)
    {
    	actualColider = null;

    	if(isNPC){
    		myBehaviour.canInteract = initialInteractionCondition;
    	}
    }
}
