using UnityEngine;
using System.Collections;
using Rpg;

public class PortaCercaBehaviour : MonoBehaviour, IInteractable {

	// Use this for initialization

	GameObject doorOpened;
	GameObject doorClosed;
	GameObject door;

	bool open;
	void Start () {

		open = true;

		doorOpened = Resources.Load("MinigameCachorro/Prefabs/porta-cachoro-open") as GameObject;
		doorClosed = Resources.Load("MinigameCachorro/Prefabs/porta-cachoro-closed") as GameObject;

		string status = Log.GetValue("mn01dog");
		if(status != null){
			door = Instantiate(doorClosed);
			open = false;
		} else {
			door = Instantiate(doorOpened);
		}

		door.transform.SetParent(transform, false);
	}
	

	public void OnInteractEnter(GameObject from) {

		if(!DogBehaviour.locked) return;
		if(!open) return;

		Destroy(door);
		Log.Register("mn01dog", "Closed");
		door = Instantiate(doorClosed);
		door.transform.SetParent(transform, false);
		
		open = false;

		string status = Log.GetValue("mn01pipe");
		if(status != null){
			Log.Register("mn01", "Done");
		}

	}

	public void OnInteractExit(GameObject from){}
	public void Interact(GameObject to) {}
	public bool AutoInteract() {
		return false;
	}
}
