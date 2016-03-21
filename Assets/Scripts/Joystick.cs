using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Joystick : MonoBehaviour {

	// Use this for initialization
	public static bool joyState;
	public static bool btnState;
	public static Canvas joyCanvas;
	public static Image joy;
	public static Image aBtn;
	public static Image bBtn;

	void Start () {
		joyCanvas = GameObject.Find("Joystick").GetComponent<Canvas>();
		joy = joyCanvas.transform.Find("MobileJoystick").GetComponent<Image>();
		aBtn = joyCanvas.transform.Find("Buttons").Find("AButton").GetComponent<Image>();
		bBtn = joyCanvas.transform.Find("Buttons").Find("BButton").GetComponent<Image>();
	}
	
	// Update is called once per frame

	public static void ShowJoy(){
		joy.enabled = true;
	}

	public static void HideJoy(){
		joy.enabled = false;
	}

	public static void HideButtons(){
		aBtn.enabled = false;
		bBtn.enabled = false;
	}

	public static void ShowButtons(){
		aBtn.enabled = true;
		bBtn.enabled = true;
	}

	void Update () {
	
	}
}
