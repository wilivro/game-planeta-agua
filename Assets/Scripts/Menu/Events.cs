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

	public void SpeedTextIntro() {
		Animator anim = GameObject.Find("Texto").GetComponent<Animator>();
		float speed = anim.GetFloat("speed");
		if(speed == 1.0f){
			speed = 5.0f;
		} else {
			speed = 1.0f;
		}

		anim.SetFloat("speed", speed);
	}

	public void ToggleMenu(){
		Canvas menu = GameObject.Find("Menu").GetComponent<Canvas>();
		if(menu.enabled){
			menu.enabled = false;
		} else {
			menu.enabled = true;
		}
	}


}
