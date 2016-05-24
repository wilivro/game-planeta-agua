using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Fader : MonoBehaviour {

	// Use this for initialization

	public void OnBlack() {
		EventManager.Trigger("FadeToBlack");
	}

	public void Die() {
		Destroy(gameObject);
	}
}
