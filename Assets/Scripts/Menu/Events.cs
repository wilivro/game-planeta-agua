using UnityEngine;
using System.Collections;

public class Events : MonoBehaviour
{

	public void OnClickNewGameButton() {
		Application.LoadLevel("cena1");
	}
}
