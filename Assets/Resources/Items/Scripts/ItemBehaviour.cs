using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Window;
using Rpg;
using Rpg.QuestSystem;
using Rpg.DialogueSystem;

public class ItemBehaviour : MonoBehaviour, IInteractable {

	// Use this for initialization
	public string name;
	public bool autoInteract;
	Item self;
	GameObject interactable;

	private UnityAction<object[]> OnItemAdd;

	GameObject glow;
	GameObject glowInstance;

	void Start () {
		self = new Item(name);

		OnItemAdd = new UnityAction<object[]> (OnItemAddCallback);

		glow = Resources.Load("Items/Animations/glow") as GameObject;

		EventManager.AddListener("QuestHelperItemAdd", OnItemAdd);

		string position = Log.GetValue("c"+self.id);
		if(position == transform.position.ToString()) {
			Destroy(gameObject);
		}
	}

	void OnItemAddCallback(object[] param) {
		if(self.preRequirements != null && !Log.HasKey(self.preRequirements)) return;
		glowInstance = Instantiate(glow);
		glowInstance.transform.SetParent(transform, false);
	}

	public void OnInteractEnter(GameObject from) {
		if(self.preRequirements != null && !Log.HasKey(self.preRequirements)) return;
		if(self.book != null && self.autoOpen) {
			Transform target = GameObject.Find("GameController").transform;
			Window.Book book = new Window.Book(target, self.book);
			book.Open();
		}

		if(self.receive) Player.inventory.Add(self);

		if(self.registerLog != null) Log.Register(self.registerLog);

		if(!self.permanent) {
				Log.Register("c"+self.id, transform.position.ToString());
			Destroy(gameObject);
		}

		if(glowInstance != null) Destroy(glowInstance);
	}

	public bool AutoInteract() {
		return autoInteract;
	}

	public void OnInteractExit(GameObject from){}

	public void Interact(GameObject to) {}
}
