using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Video : MonoBehaviour {

	// Use this for initialization
	// public MovieTexture video;
	public AudioSource audio;
	public bool isPlayingBefore;

	void Start () {
		// video = GetComponent<RawImage>().mainTexture as MovieTexture;
		// audio = GetComponent<AudioSource>();
		// video.Play();
		Handheld.PlayFullScreenMovie ("NascenteVideo.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
		isPlayingBefore = true;
	}
	
	// Update is called once per frame
	void Update () {
		// if(!video.isPlaying && isPlayingBefore){
		// 	Destroy(gameObject.transform.parent.gameObject);
		// }
	}

	public void PlayVideo(){
		// if(video.isPlaying){
		// 	video.Pause();
		// 	isPlayingBefore = false;
		// } else {
		// 	video.Play();
		// 	audio.Play();
		// 	isPlayingBefore = true;
		// }
	}

	public void StopVideo() {
		// video.Stop();
		// PlayVideo();
		// video.Pause();
	}
}
