using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

using Window;
using Rpg;
using Rpg.WindowSystem;

public class WindowCanvas : MonoBehaviour {

	// Use this for initialization
	public static Scene.Language language;
	public static Journal journal;
	public static Rpg.WindowSystem.Inventory inventory;
	public static QuestHelper helper;
	public static Database database;

	GameObject wrapper;

	void Start () {
		database  = new Database("Database/database");
		language  = new Scene.Language("pt-br");
		journal   = new Journal(transform);
		inventory = new Rpg.WindowSystem.Inventory(transform, false, false);
		helper    = new QuestHelper();

		wrapper = transform.Find("Wrapper").gameObject;
	}

	void WrapperHandler(){
		if(WindowBase.openedNow != null){
			wrapper.active = true;
			return;
		}

		wrapper.active = false;
	}
	
	// Update is called once per frame
	void Update () {
		WrapperHandler();
		if(CrossPlatformInputManager.GetButtonUp("Journal")){
			journal.Toggle();
		}
		if(CrossPlatformInputManager.GetButtonUp("Inventory")){
			inventory.Toggle();
		}
	}
}
