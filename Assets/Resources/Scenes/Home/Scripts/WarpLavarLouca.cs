using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Rpg;

public class WarpLavarLouca : WarpBehaviour {

	private UnityAction<object[]> OnItemAdd;

	GameObject glow;
	GameObject glowInstance;

	public void Start () {
		base.Start();
		OnItemAdd = new UnityAction<object[]> (OnItemAddCallback);

		glow = Resources.Load("Items/Animations/glow") as GameObject;
		EventManager.AddListener("QuestHelperItemAdd", OnItemAdd);
		if(Log.HasKey(self.requirements) && !Log.HasKey("q02s01")) {
			glowInstance = Instantiate(glow);
			glowInstance.transform.SetParent(transform, false);
		}
	}

	void OnItemAddCallback(object[] param) {
		if(self.requirements != null && !Log.HasKey(self.requirements) && !Log.HasKey("q02s01")) return;
		glowInstance = Instantiate(glow);
		glowInstance.transform.SetParent(transform, false);
	}


	public override bool Teleporter() {
		Application.LoadLevel(1);
		return true;
	}

	public override void NotReady() {
	}

	public override bool AutoInteract() {
		return false;
	}
}