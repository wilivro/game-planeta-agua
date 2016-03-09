using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;


public class Player : MonoBehaviour {

	Rigidbody2D rbody;
	public Animator anim;

	public static Inventory inventory;
	static Transform QuestHelper;

	public static bool created = false;

	void Awake() {
        if (!created) {
         	// this is the first instance - make it persist
         	QuestHelper = GameObject.Find("QuestHelper").transform;
     		DontDestroyOnLoad(this.gameObject);
     		created = true;
     	} else {
         	// this must be a duplicate from a scene reload - DESTROY!
         	Destroy(this.gameObject);
     	} 
    }

	void Start () {

		rbody = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();

		PlayerPrefs.SetInt("Quest", 1);
		PlayerPrefs.SetInt("SubQuest", 1);
		PlayerPrefs.SetString("QuestLog", "");

		inventory = GameObject.Find("Inventory").AddComponent<Inventory>() as Inventory;

		print("player");

	}


	void Movement() {

		if(SmoothCamera.isFading) {
			anim.SetBool("isWalking", false);
			return;
		};

		Vector2 movement_vector = new Vector2();
        Vector2 axis = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"));
        
    	movement_vector = axis;

		if(movement_vector != Vector2.zero) {
			anim.SetBool("isWalking", true);
			anim.SetFloat("input_x", movement_vector.x);
			anim.SetFloat("input_y", movement_vector.y);
		} else {
			anim.SetBool("isWalking", false);
		}

		rbody.MovePosition(rbody.position + movement_vector*Time.deltaTime);

	}

	public static bool CheckQuestIsCompleted() {
		return false;
	}
	
	// Update is called once per frame
	void Update () {
		Movement();
		print(inventory.content.Count);
	}

}