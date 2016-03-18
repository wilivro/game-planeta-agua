using UnityEngine;
using System.Collections;

public class Events : MonoBehaviour
{

	public void EndAnimation() {
		Application.LoadLevel("newGame");
	}

	public void EndIntro() {
		Application.LoadLevel("cidade");
	}

	public void OnClickNewGameButton() {
		PlayerPrefs.SetInt("Score", 0);
		PlayerPrefs.SetInt("Quest", 0);
		PlayerPrefs.SetInt("SubQuest", 0);
		PlayerPrefs.SetString("QuestLog", "");
		PlayerPrefs.Save();
		Application.LoadLevel("TextoIntro");
	}

	public void onClickSair(){
		Application.Quit();
	}

	public void fadeComplete() {
		SmoothCamera.isFading = false;
	}

}
