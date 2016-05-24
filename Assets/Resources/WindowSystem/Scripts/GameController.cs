using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

using Window;
using Rpg;
using Rpg.WindowSystem;

public class GameController : MonoBehaviour {

	// Use this for initialization
	public static Scene.Language language;
	public static Journal journal;
	public static Rpg.WindowSystem.Inventory inventory;
	public static QuestHelper helper;
	public static Database database;
	public static Rpg.Player player;

	GameObject wrapper;

	public static bool created = false;

	void Awake () {

		

		wrapper = transform.Find("Wrapper").gameObject;

		if (!created) {
         	// this is the first instance - make it persist
     		DontDestroyOnLoad(this.gameObject);
     		created = true;

     		database  = new Database("Database/database");
				language  = new Scene.Language("pt-br");
				journal   = new Journal(transform);
				inventory = new Rpg.WindowSystem.Inventory(transform, false, false);
				helper    = new QuestHelper();

				player = new Rpg.Player("Teste");
			
     	} else {
         	// this must be a duplicate from a scene reload - DESTROY!
         	Destroy(this.gameObject);
     	}

     	
	}


	void WrapperHandler(){
		if(WindowBase.openedNow != null){
			wrapper.active = true;
			return;
		}

		wrapper.active = false;
	}
	
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
