using UnityEngine;
using System;
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

	private int actualSpeech;

	bool LoadBehaviour() {
		if(behaviourFile == null){
			return false;
		}

		try {
			XmlSerializer serializer = new XmlSerializer(typeof(Behaviour));
			MemoryStream stream = new MemoryStream(behaviourFile.bytes);
	 		myBehaviour = serializer.Deserialize(stream) as Behaviour;
	 		initialInteractionCondition = myBehaviour.canInteract;

	 		dialogWindow = new DialogWindow(myBehaviour.charName, "Characters/Leo/face");
	 		return true;
 		} catch {
 			return false;
 		}
	}

	void Start () {
		isNPC = LoadBehaviour();

		if(!isNPC){
			if(!PlayerPrefs.HasKey("Quest")) {
				PlayerPrefs.SetInt("Quest", 0);
				PlayerPrefs.SetInt("SubQuest", 0);
			}
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

	int GetDialogIndex() {
		int quest = PlayerPrefs.GetInt("Quest");
		int subQuest = PlayerPrefs.GetInt("SubQuest");
		return (myBehaviour.subQuest.CompareTo(subQuest) +1)*Convert.ToInt32(myBehaviour.quest==quest);

	}

	Speech GetDialog(){
		
		int dialog = GetDialogIndex(); 

		return myBehaviour.dialogs[dialog].speechs[actualSpeech];
	}

	bool DialogEnd(){
		int dialog = GetDialogIndex();

		return actualSpeech == myBehaviour.dialogs[dialog].speechs.Count;
	}
	
	void WaitInteraction() {
		if(actualColider == null || actualColider.tag != "Player") return;

		if(Input.GetKey("space") && myBehaviour.canInteract) {
			dialogWindow.Destroy();
			myBehaviour.canInteract = false;
			
			if(DialogEnd()) {
				actualSpeech = 0;
				dialogWindow.Destroy();
				print("end");
			} else {
				dialogWindow.Show(GetDialog());
				actualSpeech++;
			}
			return;
		}

		if(Input.GetKeyUp("space") && !myBehaviour.canInteract) {
			myBehaviour.canInteract = true;
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
