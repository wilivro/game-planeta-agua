using UnityEngine;
using System.Collections;

public class InitGame : MonoBehaviour
{

	void Start () {
		if(PlayerPrefs.HasKey("Score")) {
			PlayerPrefs.SetInt("Score", 0);
			PlayerPrefs.SetInt("Quest", 0);
			PlayerPrefs.SetInt("SubQuest", 0);
			PlayerPrefs.SetString("QuestLog", "");
			PlayerPrefs.Save();
		}
		
		Time.timeScale = 1;
	}
}
