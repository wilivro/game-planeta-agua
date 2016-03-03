using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;


public class Character : MonoBehaviour {

	Rigidbody2D rbody;
	public Animator anim;

	public TextAsset behaviourFile;
	public bool isNPC;

	private GameObject actualColider;
	private bool initialInteractionCondition;
	private Behaviour myBehaviour;

	private DialogWindow dialogWindow;

	public int actualSpeech;
	public bool inChoice;

	private bool wait;

	bool LoadBehaviour() {
		if(behaviourFile == null){
			return false;
		}

		try {
			XmlSerializer serializer = new XmlSerializer(typeof(Behaviour));
			MemoryStream stream = new MemoryStream(behaviourFile.bytes);
	 		myBehaviour = serializer.Deserialize(stream) as Behaviour;
	 		initialInteractionCondition = myBehaviour.canInteract;

	 		//dialogWindow = new DialogWindow(myBehaviour.charName, "Characters/Leo/face");
	 		dialogWindow = gameObject.AddComponent<DialogWindow>() as DialogWindow;
	 		dialogWindow.Config(myBehaviour.charName, "Characters/Leo/face");

	 		return true;
 		} catch {
 			return false;
 		}
	}

	void Start () {
		isNPC = LoadBehaviour();

		rbody = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		//if(isNPC) StartCoroutine("Idle");

		PlayerPrefs.SetInt("Quest", 0);
		PlayerPrefs.SetInt("SubQuest", 0);

	}


	void Movement() {

		if(SmoothCamera.isFading) {
			anim.SetBool("isWalking", false);
			return;
		};

		Vector2 movement_vector = new Vector2();
		Vector2 axis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		if (axis == Vector2.zero) {
            //Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //print(mouse);
            //movement_vector = new Vector2(mouse.x, mouse.y);
        } else if(axis != Vector2.zero){
        	movement_vector = axis;
        } else {
        	//movement_vector = new Vector2(Math.Sign(nav.velocity.x), Math.Sign(nav.velocity.y));
        }

		if(movement_vector != Vector2.zero) {
			anim.SetBool("isWalking", true);
			anim.SetFloat("input_x", movement_vector.x);
			anim.SetFloat("input_y", movement_vector.y);
		} else {
			anim.SetBool("isWalking", false);
		}

		rbody.MovePosition(rbody.position + movement_vector*Time.deltaTime);

	}

	int GetDialogIndex() {
		int quest = PlayerPrefs.GetInt("Quest");
		int subQuest = PlayerPrefs.GetInt("SubQuest");

		return (subQuest.CompareTo(myBehaviour.subQuest) +2)*Convert.ToInt32(myBehaviour.quest==quest);

	}

	public Speech GetDialog(){
		int dialog = GetDialogIndex(); 

		return myBehaviour.dialogs[dialog].speechs[actualSpeech];
	}

	bool DialogEnd(){
		int dialog = GetDialogIndex();

		return actualSpeech == myBehaviour.dialogs[dialog].speechs.Count;
	}
	
	void WaitInteraction() {
		if(actualColider == null || actualColider.tag != "Player") return;

		if(Input.GetKey("space") && myBehaviour.canInteract) {

			dialogWindow.Destroy();
			myBehaviour.canInteract = false;
			
			if(DialogEnd()) {
				actualSpeech = 0;
				dialogWindow.Destroy();
				int quest = PlayerPrefs.GetInt("Quest");
				int subQuest = PlayerPrefs.GetInt("SubQuest");
				if(myBehaviour.subQuest == subQuest && myBehaviour.quest == quest){
					PlayerPrefs.SetInt("SubQuest", subQuest+1);
				}
			} else {
				bool hasChioces = dialogWindow.Show(this);
				if(!hasChioces) actualSpeech++;
			}
			return;
		}

		if(Input.GetKeyUp("space") && !myBehaviour.canInteract) {
			myBehaviour.canInteract = true;
			return;
		}

		if(Input.GetKey("escape")) {
			actualSpeech = 0;
			myBehaviour.canInteract = true;
			dialogWindow.Destroy();
			//TODO close window;
			return;
		}
	}


	int compare(WalkPoint v1, WalkPoint v2){
		float distance1 = Vector3.Distance(v1.from, v1.to);
		float distance2 = Vector3.Distance(v2.from, v2.to);
		return distance2.CompareTo(distance1);
	}

	void Idle() {

		RaycastHit2D hitUp;
		RaycastHit2D hitDown;
		RaycastHit2D hitLeft;
		RaycastHit2D hitRight;

		WalkPoint newPosition;

		List<WalkPoint> tests = new List<WalkPoint>();

			tests.Clear();

			hitUp    = Physics2D.Raycast(transform.position + new Vector3(0, 0.2f, 0), Vector2.up);
			hitDown  = Physics2D.Raycast(transform.position + new Vector3(0, -0.4f, 0), Vector2.down);
			hitLeft  = Physics2D.Raycast(transform.position + new Vector3(-0.4f, 0, 0), Vector2.left);
			hitRight = Physics2D.Raycast(transform.position + new Vector3(0.4f, 0, 0), Vector2.right);

			if (hitUp.collider != null) {
			Debug.DrawLine(transform.position + new Vector3(0, 0.2f, 0), hitUp.point);
			tests.Add(new WalkPoint(transform.position, hitUp.point, Vector2.up));
			}
			if (hitDown.collider != null) {
				Debug.DrawLine(transform.position + new Vector3(0, -0.4f, 0), hitDown.point);
			tests.Add(new WalkPoint(transform.position, hitDown.point, Vector2.down));
			}
			if (hitLeft.collider != null) {
				Debug.DrawLine(transform.position + new Vector3(-0.4f, 0, 0), hitLeft.point);
			tests.Add(new WalkPoint(transform.position, hitLeft.point, Vector2.left));
			}
			if (hitRight.collider != null) {
				Debug.DrawLine(transform.position + new Vector3(0.4f, 0, 0), hitRight.point);
			tests.Add(new WalkPoint(transform.position, hitRight.point, Vector2.right));
			}

			tests.Sort(compare);

			newPosition = tests[0];
			print(newPosition.dir);

			rbody.MovePosition(transform.position + newPosition.dir*Time.deltaTime);

	}

	// Update is called once per frame
	void Update () {
		if(!isNPC){
			Movement();
		} else {
			if(actualColider){
				if(anim){
					Vector3 lookAt = actualColider.transform.position-transform.position;
					anim.SetBool("isWalking", false);
					anim.SetFloat("input_x", lookAt.x);
					anim.SetFloat("input_y", lookAt.y);
				}

				WaitInteraction();
			} else {
				//Idle();
			}
		}
	}

	void OnCollisionEnter2D (Collision2D col)
    {
		if(col.gameObject.tag == "Player"){
        	actualColider = col.gameObject;
		}
    }

    void OnCollisionExit2D (Collision2D col)
    {
    	actualColider = null;

    	if(isNPC){
    		dialogWindow.Destroy();
    		myBehaviour.canInteract = initialInteractionCondition;
    	}
    }
}

public class WalkPoint
{
	public Vector3 from;
	public Vector3 to;
	public Vector3 dir;

	public WalkPoint(Vector3 _from, Vector3 _to, Vector3 _dir){
		from = _from;
		to = _to;
		dir = _dir;
	}
}