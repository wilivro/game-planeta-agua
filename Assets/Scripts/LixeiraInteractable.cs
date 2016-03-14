using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class LixeiraInteractable : MiniGameOpen {

	public string name;
	public int type;

	// Use this for initialization
	void Start () {
		base.Start();
		name = gameObject.name;

	}
	
	// Update is called once per frame
	void Update () {
		if(CrossPlatformInputManager.GetButton("Submit")){
			if(actualColider == null || actualColider.gameObject.tag != "Player") return;
			if(miniGame != "")
				StartCoroutine("WarpScene");
		}
	}
}
