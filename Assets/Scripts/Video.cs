using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Video : MonoBehaviour {

	void Start () {
		Handheld.PlayFullScreenMovie ("NascenteVideo.3gp", Color.black, FullScreenMovieControlMode.Full, FullScreenMovieScalingMode.Fill);
	}
}
