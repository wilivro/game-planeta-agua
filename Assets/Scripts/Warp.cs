using UnityEngine;
using System.Collections;

public class Warp : MonoBehaviour {

	// Use this for initialization
	public GameObject warpTO;
	public Vector3 offset;

	public GameObject player;
	public Animator animPlayer;
	public Vector2 Direction;

	public GameObject warpFade;
	public Animator anim;

	void Start () {
		warpFade = GameObject.Find("Fade");
		anim = warpFade.GetComponent<Animator>();
		player = GameObject.Find("Player");
		animPlayer = player.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator WarpPlayer() {
		SmoothCamera.isFading = true;
		anim.SetTrigger("FadeIn");

		while(SmoothCamera.isFading)
			yield return null;

		player.transform.position = warpTO.transform.position + offset;

		animPlayer.SetFloat("input_x", Direction.x);
		animPlayer.SetFloat("input_y", Direction.y);

		StartCoroutine("FadeOut");
	}

	IEnumerator FadeOut() {
		SmoothCamera.isFading = true;
		anim.SetTrigger("FadeOut");

		while(SmoothCamera.isFading)
			yield return null;
	}

	void OnCollisionEnter2D(Collision2D other) {

		if(other.gameObject.tag == "Player"){
			StartCoroutine("WarpPlayer");
		}
			
	}
}
