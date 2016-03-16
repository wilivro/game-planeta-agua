using UnityEngine;
using System;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class MiniGameOpen : Warp {

	// Use this for initialization
	public bool questProcess;
	public int quest;
	public int subQuest;
	public string miniGame;

	public bool permanent;
	void Start () {
		base.Start();
	}

	int getSatus(){
		int _quest = PlayerPrefs.GetInt("Quest");
		int _subQuest = PlayerPrefs.GetInt("SubQuest");
		return (_subQuest.CompareTo(subQuest) +2)*Convert.ToInt32(quest==_quest);
	}
	
	// Update is called once per frame
	void Update () {
		if(getSatus() != 2 && !(gameObject.GetComponent<BoxCollider2D>().enabled && permanent)){
			gameObject.GetComponent<BoxCollider2D>().enabled = false;
			Transform glow = gameObject.transform.Find("Glow");
			if(glow) glow.gameObject.active = false;
			return;
		} else {
			gameObject.GetComponent<BoxCollider2D>().enabled = true;
			Transform glow = gameObject.transform.Find("Glow");
			if(glow) glow.gameObject.active = true;
		}

		if(CrossPlatformInputManager.GetButton("Submit")){
			if(actualColider == null || actualColider.gameObject.tag != "Player") return;
			if(warpTO){
				StartCoroutine("WarpPlayer");
			} else {
				if(miniGame != ""){
					StartCoroutine("WarpScene");
				}
			}
			
		}
	}

	public IEnumerator WarpScene() {
		SmoothCamera.isFading = true;
		anim.SetTrigger("FadeIn");

		while(SmoothCamera.isFading)
			yield return null;

		Application.LoadLevel(miniGame);

		animPlayer.SetFloat("input_x", Direction.x);
		animPlayer.SetFloat("input_y", Direction.y);

		StartCoroutine("FadeOut");
	}

	public override void ColliderWithMe() {
		return;
	}
}
