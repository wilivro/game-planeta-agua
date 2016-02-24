using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;


public class Character : MonoBehaviour {

	public TextAsset behaviourFile;
	public bool isNPC;

	private GameObject actualColider;
	private bool initialInteractionCondition;
	private Behaviour myBehaviour;

	private DialogWindow dialogWindow;

	bool LoadBehaviour() {
		if(behaviourFile == null){
			return false;
		}

		try {
			XmlSerializer serializer = new XmlSerializer(typeof(Behaviour));
			MemoryStream stream = new MemoryStream(behaviourFile.bytes);
	 		myBehaviour = serializer.Deserialize(stream) as Behaviour;
	 		initialInteractionCondition = myBehaviour.canInteract;

	 		print(myBehaviour.dialogs[0].speechs.Count);

	 		dialogWindow = new DialogWindow(myBehaviour.charName, "Characters/Leo/face");
	 		return true;
 		} catch {
 			return false;
 		}
	}

	void Start () {
		isNPC = LoadBehaviour();

		if(isNPC){
			transform.position = new Vector3(5, 0, 0);
		} else {
			transform.position = new Vector3(-5, 0, 0);
		}
	}

	void Movement() {
		if (Input.GetKey("left")){
            transform.Translate(-2*Time.deltaTime, 0,0);
		}
        
        if (Input.GetKey("right")){
            transform.Translate(2*Time.deltaTime, 0,0);
        }
	}
	
	void WaitInteraction() {
		if(actualColider == null || actualColider.tag != "Player") return;

		if(Input.GetKey("space") && myBehaviour.canInteract) {
			myBehaviour.canInteract = false;
			print("Oi eu sou "+ myBehaviour.charName);
			dialogWindow.Show(myBehaviour.dialogs[0].speechs[0]);

			//TODO Open window;
			return;
		}

		if(Input.GetKey("escape") && !myBehaviour.canInteract) {
			myBehaviour.canInteract = true;
			dialogWindow.Destroy();
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
    		dialogWindow.Destroy();
    		myBehaviour.canInteract = initialInteractionCondition;
    	}
    }
}
