using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;

public class Interactable : MonoBehaviour {

	// Use this for initialization

	public GameObject actualColider;
	public TextAsset behaviourFile;
	public Behaviour myBehaviour;
	public bool initialInteractionCondition;
	public bool isNPC;

	bool LoadBehaviour() {
		if(behaviourFile == null){
			return false;
		}

		try {
			XmlSerializer serializer = new XmlSerializer(typeof(Behaviour));
			MemoryStream stream = new MemoryStream(behaviourFile.bytes);
	 		myBehaviour = serializer.Deserialize(stream) as Behaviour;
	 		initialInteractionCondition = myBehaviour.canInteract;

	 		return true;
 		} catch {
 			return false;
 		}
	}

	public void Start () {
		isNPC = LoadBehaviour();
		print("oi");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D (Collision2D col)
    {
		if(col.gameObject.tag == "Player"){
			GameObject.Find("Buttons").GetComponent<Animator>().SetTrigger("FadeIN");
        	actualColider = col.gameObject;
		}
    }

    void OnCollisionExit2D (Collision2D col)
    {
    	actualColider = null;

    	if(col.gameObject.tag == "Player"){
    		GameObject.Find("Buttons").GetComponent<Animator>().SetTrigger("FadeOUT");
    		//dialogWindow.Destroy();
    		myBehaviour.canInteract = initialInteractionCondition;
    	}
    }
}
