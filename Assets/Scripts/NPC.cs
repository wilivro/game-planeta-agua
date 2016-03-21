using UnityEngine;
using System;
using System.Collections;


public class NPC : Communicative 
{

	public Rigidbody2D rbody;
	public Animator anim;

	void Start () {
		base.Start();

 		rbody = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(actualColider != null){
			if(anim){
				Vector3 lookAt = actualColider.transform.position-transform.position;
				anim.SetBool("isWalking", false);
				anim.SetFloat("input_x", lookAt.x);
				anim.SetFloat("input_y", lookAt.y);
			}

			WaitInteraction();
		} else {
			//TODO Idle();
		}
		
	}
}
