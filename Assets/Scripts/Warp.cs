using UnityEngine;
using System;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Warp : Interactable {

	// Use this for initialization
	public Warp warpTO;
	public Vector3 offset;

	public GameObject player;
	public Animator animPlayer;
	public Vector2 Direction;

	public GameObject warpFade;
	public Animator anim;
	public DialogWindow dialogWindow;

	public void Start () {

		base.Start();

		warpFade = GameObject.Find("Fade");
		anim = warpFade.GetComponent<Animator>();
		player = GameObject.Find("Player");
		animPlayer = player.GetComponent<Animator>();

		if(isNPC) {
			dialogWindow = gameObject.AddComponent<DialogWindow>() as DialogWindow;
 			dialogWindow.Config(myBehaviour.charName, "Characters/Leo/face");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(actualColider == null || actualColider.gameObject.tag != "Player") return;
		if(CrossPlatformInputManager.GetButton("Submit")||CrossPlatformInputManager.GetButton("Cancel")){
			if(isNPC) dialogWindow.Destroy();
		}
	}

	public int GetDialogIndex() {
		if(!isNPC) return -1;
		int quest = PlayerPrefs.GetInt("Quest");
		int subQuest = PlayerPrefs.GetInt("SubQuest");

		if(quest > myBehaviour.quest){
			return -1; 
		}

		return (subQuest.CompareTo(myBehaviour.subQuest)+2);

	}

	public Speech GetSpeech(){
		int dialog = GetDialogIndex(); 

		return myBehaviour.dialogs[dialog].speechs[0];
	}

	IEnumerator WarpPlayer() {
		SmoothCamera.isFading = true;
		anim.SetTrigger("FadeIn");

		while(SmoothCamera.isFading)
			yield return null;

		player.transform.position = warpTO.transform.position + offset;

		animPlayer.SetFloat("input_x", Direction.x);
		animPlayer.SetFloat("input_y", Direction.y);

		StartCoroutine("FadeOut");
	}

	IEnumerator FadeOut() {
		SmoothCamera.isFading = true;
		anim.SetTrigger("FadeOut");

		while(SmoothCamera.isFading)
			yield return null;
	}

	void OnCollisionEnter2D(Collision2D other) {

		base.OnCollisionEnter2D(other);

		if(other.gameObject.tag == "Player"){
			print(GetDialogIndex());
			if(isNPC && GetDialogIndex() > 0){
				dialogWindow.Show(GetSpeech(), null);
				return;
			}

			if(!warpTO){
				return;
			}

			StartCoroutine("WarpPlayer");
		}
			
	}
}
