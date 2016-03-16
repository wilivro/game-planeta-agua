using UnityEngine;
using System.Collections;

public class Events : MonoBehaviour
{

	public void OnClickNewGameButton() {
		Application.LoadLevel("cidade");
	}

	public void fadeComplete() {
		Time.timeScale = 1;
		SmoothCamera.isFading = false;
		print("oi");
	}

}
