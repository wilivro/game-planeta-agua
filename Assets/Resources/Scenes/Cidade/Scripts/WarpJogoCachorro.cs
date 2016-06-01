using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Rpg;

public class WarpJogoCachorro : WarpBehaviour {


	public Vector3 warpLocation;
	public void Start () {
		base.Start();
	}

	public override bool Teleporter() {
		GameObject.Find("Player").transform.position = warpLocation;
		Application.LoadLevel(3);
		return true;
	}

	public override bool AutoInteract() {
		return true;
	}
}
