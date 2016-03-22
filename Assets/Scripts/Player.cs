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

	public static bool hasQuestLog = false;

	public static bool interacting = false;

	public static int layer = 11;

	void Awake() {
       	QuestHelper = GameObject.Find("QuestHelper").transform;
    }

	void Start () {

		rbody = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();

		PlayerPrefs.SetInt("Quest", 6);
		PlayerPrefs.SetInt("SubQuest", 0);
		PlayerPrefs.SetString("QuestLog", "");
		// PlayerPrefs.Save();

		inventory = GameObject.Find("Inventory").AddComponent<Inventory>() as Inventory;
	}


	void Movement() {

		if(SmoothCamera.isFading || Player.interacting) {
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
	}

}