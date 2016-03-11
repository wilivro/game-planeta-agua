using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class LixeiraInteractable : Interactable {

	public string name;
	public int type;

	// Use this for initialization
	void Start () {
		base.Start();
		name = gameObject.name;

	}
	
	// Update is called once per frame
	void Update () {
		if(CrossPlatformInputManager.GetButton("Submit")) {
			//Player.inventory.content.Add(self);
			//Destroy(transform.gameObject);
			// Make a background box
		}
	}

	void OnGUI () {
		// Make a background box
		//GUI.Box(new Rect(10,10,100,90), "Loader Menu");

		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		//if(GUI.Button(new Rect(20,40,80,20), "Level 1")) {
		//	Application.LoadLevel(1);
		//}

		// Make the second button.
		//if(GUI.Button(new Rect(20,70,80,20), "Level 2")) {
		//	Application.LoadLevel(2);
		//}
	}
}
