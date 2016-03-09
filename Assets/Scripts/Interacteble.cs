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

	public Collider2D actualColider;
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
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnCollisionEnter2D (Collision2D col)
    {
		OnTriggerEnter2D(col.collider);
    }

    public void OnCollisionExit2D (Collision2D col)
    {
    	OnTriggerExit2D(col.collider);
    }

    public void OnTriggerEnter2D (Collider2D col)
    {
		if(col.gameObject.tag == "Player"){
			GameObject.Find("Buttons").GetComponent<Animator>().SetTrigger("FadeIN");
        	actualColider = col;
		}
    }

    public void OnTriggerExit2D (Collider2D col)
    {
    	actualColider = null;

    	if(col.gameObject.tag == "Player"){
    		GameObject.Find("Buttons").GetComponent<Animator>().SetTrigger("FadeOUT");
    		//dialogWindow.Destroy();
    		if(isNPC) myBehaviour.canInteract = initialInteractionCondition;
    	}
    }


}
