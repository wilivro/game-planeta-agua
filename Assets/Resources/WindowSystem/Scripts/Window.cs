using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace Window
{
 

	public class WindowBase //BaseClass
	{
		public GameObject prefab;
		public GameObject instance;
		public Transform myCanvas;
		public static WindowBase openedNow = null;

		bool opened;

		public WindowBase() {
			
		}

		public WindowBase(Transform _canvas) {
			myCanvas = _canvas;
			opened = false;
		}

		public void StandAlone() {
			try
			{
				openedNow.Close();
				
			} catch {

			}
			openedNow = this;
			
			
		}

		public virtual void Open(bool preserveParent = false) {
			if(prefab == null) return;
			if(opened) return;
			if(!preserveParent) StandAlone();

			instance = UnityEngine.Object.Instantiate(prefab);
			RectTransform rt = instance.GetComponent<RectTransform>();
			rt.SetParent(myCanvas, false);
			opened = true;
		}

		public virtual void Close(bool preserveParent = false) {
			if(instance == null) return;
			UnityEngine.Object.Destroy(instance);
			opened = false;
			if(!preserveParent) openedNow = null;
		}

		public void Toggle() {
			if(!opened) {
				Open();
			} else {
				Close();
			}
		}
	}

	public class Book : WindowBase
	{
		string[] pages;
		GameObject text;
		GameObject image;
		bool preserveParent;
		public Book(Transform _canvas, string[] _pages, bool _preserveParent = false) : base(_canvas) {
			prefab = Resources.Load("WindowSystem/Prefabs/Book/Book") as GameObject;
			text = Resources.Load("WindowSystem/Prefabs/Book/Text") as GameObject;
			image = Resources.Load("WindowSystem/Prefabs/Book/Image") as GameObject;
			pages = _pages;
			preserveParent = _preserveParent;

		}

		public void Open(){
			OpenPage(0);
		}

		public void OpenPage(int page) {

			if(page < 0) return;
			if(page >= pages.Length) return;

			if(instance == null){
				instance = UnityEngine.Object.Instantiate(prefab);
				instance.transform.SetParent(myCanvas, false);

				instance.transform.Find("Close").GetComponent<Button>().onClick.AddListener(delegate {Close(preserveParent);});
			}

			Transform container = instance.transform.Find("Scroll View").Find("Viewport").Find("Container");

			string pattern = @"\{\{[a-zA-Z0-9/]+\}\}";
			string pattern2 = @"[a-zA-Z0-9]+";

			foreach(Transform t in container){
				UnityEngine.Object.Destroy(t.gameObject);
			}

  			MatchCollection  images = Regex.Matches(pages[page], pattern);
  			string[] texts = Regex.Split(pages[page], pattern);

  			for(int i=0; i < texts.Length; i++) {
  				if(texts[i] != ""){
  					GameObject a = UnityEngine.Object.Instantiate(text);
  					a.transform.SetParent(container, false);
  					a.GetComponent<Text>().text = texts[i];
  				}
  				if(i <  images.Count){
  					GameObject b = UnityEngine.Object.Instantiate(image);
  					b.transform.SetParent(container, false);

  					string imageName = images[i].ToString().Replace("{{", "");
  					imageName = imageName.Replace("}}", "");

  					Sprite imageFile = Resources.LoadAll<Sprite>(imageName)[0];

  					b.GetComponent<Image>().sprite = imageFile;	
  				}
  			}

  			instance.transform.Find("Pages").Find("Prev")
  				.GetComponent<Button>()
  				.onClick.AddListener(delegate {OpenPage(page-1);});

			instance.transform.Find("Pages").Find("Next")
  				.GetComponent<Button>()
  				.onClick.AddListener(delegate {OpenPage(page+1);});

		}
	};

	public class Span : WindowBase
	{
		public Span(Transform _canvas) : base(_canvas) {
			prefab = Resources.Load("WindowSystem/Prefabs/Span/Span") as GameObject;

		}

		public void Open(string _text, Vector3 _n) {
			base.Open(true);
			Follow(_n);
			instance.transform.Find("Text").GetComponent<Text>().text = _text;
		}

		public void Follow(Vector3 _n) {
			if(instance == null) return;
			instance.transform.position = _n;
		}
	};

	public class Config    : WindowBase {};
	public class PauseGame : WindowBase {};

}
