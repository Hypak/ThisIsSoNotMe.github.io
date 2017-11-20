using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DiffcultyDisplay : MonoBehaviour {

	public Text display;

	void Update () {
		display.text = "Difficulty: " + Controller.difficulty + "\nTime per Move: ~" + Controller.difficultyTime + " Seconds";
	}
}
