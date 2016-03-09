// using UnityEngine;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Xml;
// using System.Xml.Serialization;
// using System.IO;
// using System.Text;
// using UnityStandardAssets.CrossPlatformInput;


// public class Character : Interactable {

// 	Rigidbody2D rbody;
// 	public Animator anim;

	
// 	private bool wait;

	

// 	void Start () {
// 		//isNPC = LoadBehaviour();

// 		base.Start();

// 		print(isNPC);

// 		rbody = GetComponent<Rigidbody2D>();
// 		anim = GetComponent<Animator>();
// 		//if(isNPC) StartCoroutine("Idle");

// 		if(!isNPC){
// 			PlayerPrefs.SetInt("Quest", 0);
// 			PlayerPrefs.SetInt("SubQuest", 0);
// 		}

// 	}


// 	void Movement() {

// 		if(SmoothCamera.isFading) {
// 			anim.SetBool("isWalking", false);
// 			return;
// 		};

// 		Vector2 movement_vector = new Vector2();
//         Vector2 axis = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"));

//         if (axis == Vector2.zero) {
//             //Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//             //print(mouse);
//             //movement_vector = new Vector2(mouse.x, mouse.y);
//         } else if(axis != Vector2.zero){
//         	movement_vector = axis;
//         } else {
//         	//movement_vector = new Vector2(Math.Sign(nav.velocity.x), Math.Sign(nav.velocity.y));
//         }

// 		if(movement_vector != Vector2.zero) {
// 			anim.SetBool("isWalking", true);
// 			anim.SetFloat("input_x", movement_vector.x);
// 			anim.SetFloat("input_y", movement_vector.y);
// 		} else {
// 			anim.SetBool("isWalking", false);
// 		}

// 		rbody.MovePosition(rbody.position + movement_vector*Time.deltaTime);

// 	}

// 	// Update is called once per frame
// 	void Update () {

//         if (!isNPC){
// 			Movement();
// 			//int subQuest = PlayerPrefs.GetInt("SubQuest");
// 			// print(subQuest);
// 		} else {
			
// 			// if(myBehaviour.isItem){
// 			// 	if(GetDialogIndex() < 2){
// 			// 		gameObject.GetComponent<BoxCollider2D>().enabled = false;
// 			// 		Transform glow = gameObject.transform.Find("Glow");
// 			// 		if(glow) glow.gameObject.active = false;
// 			// 		return;
// 			// 	} else {
// 			// 		gameObject.GetComponent<BoxCollider2D>().enabled = true;
// 			// 		Transform glow = gameObject.transform.Find("Glow");
// 			// 		if(glow) glow.gameObject.active = true;
// 			// 	}
// 			// }

// 			// if(actualColider){
// 			// 	if(anim){
// 			// 		Vector3 lookAt = actualColider.transform.position-transform.position;
// 			// 		anim.SetBool("isWalking", false);
// 			// 		anim.SetFloat("input_x", lookAt.x);
// 			// 		anim.SetFloat("input_y", lookAt.y);
// 			// 	}

// 			// 	WaitInteraction();
// 			// } else {
// 			// 	//Idle();
// 			// }
// 		}
// 	}

// }