using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
	
	public List<Item> content;	

	public static bool created = false;

	void Awake() {
        if (!created) {
         	// this is the first instance - make it persist
     		DontDestroyOnLoad(this.gameObject);
     		created = true;
     	} else {
         	// this must be a duplicate from a scene reload - DESTROY!
         	Destroy(this.gameObject);
     	} 
    }

	public Inventory() {
		content = new List<Item>();
	}

	public Item Add(Item i) {
		int index = content.Count;

		if(content.Contains(i) && i.unique){
			return i;
		}

		content.Add(i);

		return i;
	}
}
