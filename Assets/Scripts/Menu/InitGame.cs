﻿using UnityEngine;
using System.Collections;

public class InitGame : MonoBehaviour
{

	void Start () {
		PlayerPrefs.SetInt("Quest", 0);
		PlayerPrefs.SetInt("SubQuest", 0);
		PlayerPrefs.Save();
		print("newGame");
	}
}
