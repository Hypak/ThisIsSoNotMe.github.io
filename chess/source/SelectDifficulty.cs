using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SelectDifficulty : MonoBehaviour {

	public int aiSearchDepth;
	public string difficulty;
	public float difficultyTime;

	public void Clicked () {
		Controller.aiSearchDepth = aiSearchDepth;
		Controller.difficulty = difficulty;
		Controller.difficultyTime = difficultyTime;
	}

}
