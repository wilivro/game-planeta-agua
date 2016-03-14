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

		if(content.Contains(i) && i.acumulative){
			Item it = content.Find(x => x.name == i.name);
			it.qtd += i.qtd;

			Destroy(i.gameObject);

			return it;
		}

		GameObject it2 = Instantiate(i.gameObject) as GameObject;
		it2.active = false;
		Item itt = it2.GetComponent<Item>();
		itt.gameObject.transform.parent = transform;

		content.Add(itt);

		return i;
	}

	public Item Remove(Item i) {
		Item it = content.Find(x => x.name == i.name);

		if(it.acumulative){
			it.qtd -= 1;

			if(it.qtd == 0) content.Remove(it);

			return it;
		}

		content.Remove(it);

		return it;
	}

	public bool Contains(Item i){
		return content.Contains(i);
	}
}
