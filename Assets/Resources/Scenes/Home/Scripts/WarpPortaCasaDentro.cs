using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Rpg;

public class WarpPortaCasaDentro : WarpBehaviour {

	private UnityAction<object[]> OnItemAdd;
	public Vector3 warpLocation;
	public void Start () {
		base.Start();
	}

	public override bool Teleporter() {
		GameObject.Find("Player").transform.position = warpLocation;
		Application.LoadLevel(2);
		return true;
	}

	public override bool AutoInteract() {
		return true;
	}
}