using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using Rpg;

public class PlayerBehaviour : MonoBehaviour, IControlable, IInteractable {

	// Use this for initialization
	public Rpg.Player self;

	Rigidbody2D rbody;
	Animator anim;
	GameObject interactable;

	private UnityAction<object[]> OnDialogueStart;
	private UnityAction<object[]> OnDialogueEnd;

	bool busy;

	public static bool created = false;

	void Awake() {
        if (!created) {
         	// this is the first instance - make it persist
     		DontDestroyOnLoad(this.gameObject);
     		created = true;
     		self = GameController.player;
			self.Load();

     	} else {
         	// this must be a duplicate from a scene reload - DESTROY!
         	Destroy(this.gameObject);
     	} 
    }

	void Start () {
		
		rbody = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();

		OnDialogueStart = new UnityAction<object[]> (OnDialogueStartCallback);
		EventManager.AddListener("PlayerDialogueStart", OnDialogueStart);

		OnDialogueEnd = new UnityAction<object[]> (OnDialogueEndCallback);
		EventManager.AddListener("PlayerDialogueEnd", OnDialogueEnd);
	}
	
	// Update is called once per frame

	void OnDialogueStartCallback(object[] param) {
		busy = true;
	}
	void OnDialogueEndCallback(object[] param) {
		busy = false;
	}

	void AjustDepht() {
		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y+(32f*0.01f) );
	}

	void Update () {
		AjustDepht();
		if(busy) return;
		Control();
		Interact(interactable);
	}

	public void OnInteractEnter(GameObject from) {

	}

	public void OnInteractExit(GameObject from){}

	public void Interact(GameObject to) {
		if(to == null) return;

		if(CrossPlatformInputManager.GetButtonDown("Submit")){
			to.SendMessage("OnInteractEnter", gameObject);
		}
	}

	public bool AutoInteract() {
		return false;
	}

	public void Control() {

		Vector2 movement_vector = new Vector2();
        Vector2 axis = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"));
        
  		movement_vector = axis;

  		if(movement_vector != Vector2.zero) {
			anim.SetBool("isWalking", true);
			anim.SetFloat("input_x", movement_vector.x);
			anim.SetFloat("input_y", movement_vector.y);
		} else {
			anim.SetBool("isWalking", false);
		}

		rbody.MovePosition(rbody.position + movement_vector*Time.deltaTime);
	}

	bool IsInteractable(GameObject test){
		return test.GetComponent<IInteractable>() != null;
	}

	void OnTriggerEnter2D(Collider2D other){
		if(interactable != null) return;
		if(IsInteractable(other.gameObject)) {
			interactable = other.gameObject;
			if(interactable.GetComponent<IInteractable>().AutoInteract()) {
				interactable.SendMessage("OnInteractEnter", gameObject);
			}
		}
	}

	void OnTriggerExit2D(Collider2D other){
		try {
			interactable.SendMessage("OnInteractExit", gameObject);
		} catch {}
		interactable = null;
	}

	void OnCollisionEnter2D(Collision2D other) {
		OnTriggerEnter2D(other.collider);
	}

	void OnCollisionExit2D(Collision2D other) {
		OnTriggerExit2D(other.collider);
	}


}
