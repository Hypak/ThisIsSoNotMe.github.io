using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour {

	public Text pauseButtonText;
	public GameObject settings;
	public bool paused = false;

	// Use this for initialization
	void Start () {
		Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			TogglePause ();
		}
	}

	public void TogglePause () {
		settings.GetComponent <Settings>().resetConfirm = false;
		paused = !paused;
		if (paused) {
			Time.timeScale = 0;
			pauseButtonText.text = "Play";
			settings.SetActive (true);
		} else {
			Time.timeScale = 1;
			pauseButtonText.text = "Pause";
			settings.SetActive (false);

		}
	}
}
