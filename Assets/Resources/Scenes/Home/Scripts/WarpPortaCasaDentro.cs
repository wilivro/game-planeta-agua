using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Rpg;

public class WarpPortaCasaDentro : WarpBehaviour {

	private UnityAction<object[]> OnItemAdd;

	public void Start () {
		base.Start();
	}

	public override bool Teleporter() {
		Application.LoadLevel(2);
		return true;
	}

	public override bool AutoInteract() {
		return true;
	}
}