using UnityEngine;
using System.Collections;

public class LoucaBehaviour : MonoBehaviour {

	Animator anim;
	Vector2 size;

	Vector2 initialPosition;
	Vector2 centerPosition;

	bool stage1 = false;
	bool stage2 = false;
	bool used = false;

	int limpo;

	void Start () {
		anim = GetComponent<Animator>();

		limpo = Random.Range(0,2);
		anim.SetInteger("limpo", limpo);

		RectTransform rt = GetComponent<RectTransform>();

		float jitter = Random.Range(-100.0f, 100.0f);

		centerPosition = rt.localPosition + new Vector3(jitter, 60, 0);

		rt.localPosition = new Vector3(centerPosition.x, -600, 0f);

	}
	
	// Update is called once per frame
	void Update () {

		if(Mathf.Abs(transform.localPosition.y-centerPosition.y) <= 0.2 && !stage1) {
			stage1 = true;
			centerPosition.y -= 360f;
			if(!stage2) StartCoroutine("Wait");
			else Destroy(gameObject);
		}

		if(stage1 && !stage2) return;
		// if(stage1 && stage2) Destroy(gameObject);

		Move();
	}

	IEnumerator Wait() {
		float jitter =  Random.Range(0f, 0.5f);
		yield return new WaitForSeconds(jitter);
		stage1 = false;
		stage2 = true;
	}

	void Move() {
		transform.localPosition = Vector3.Lerp(
											transform.localPosition,
										  	(Vector3)centerPosition,
										   	0.1f
										 );
	}

	public void Lavar() {
		if(used) return;

		anim.SetTrigger("limpar");
		transform.parent.SendMessage("Wash", limpo == 0 ? true : false);
		used = true;
	}
}
