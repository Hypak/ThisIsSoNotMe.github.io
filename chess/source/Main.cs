using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
public class Main : MonoBehaviour {

	public Render render;
	public Ai ai;

	public UnityEngine.UI.Button restartButton;

	public Game game;

	bool playerIsWhite = true;

	bool selectedPiece = false;
	Pos selectedPos;

	List<Move> whiteMoves;
	List<Move> blackMoves;

	float animationEndTime = 0;
	Move animationMove;

	// Use this for initialization
	void Awake () {
		restartButton.gameObject.SetActive (false);

		game = new Game ();


		render.Rend (game);
		whiteMoves = game.GetAllLegalWhiteMoves ();
		/*
		Stopwatch watch = new Stopwatch ();
		watch.Start ();
		for (int i = 0; i < 1000; i++) {
			//Evaluator.BlackAdvantage (game);
			ai.OrderMovesShuffle (whiteMoves, game.board);
		}
		UnityEngine.Debug.Log ("Evaluation time (milliseconds): " + watch.Elapsed.TotalMilliseconds / 1000);
*/
	}

	// Update is called once per frame
	void Update () {
		try {
			float animationTimeLeft = animationEndTime - Time.time;
			if (animationTimeLeft > 0) {
				render.RenderAnimation (game, animationMove, render.animationTime - animationTimeLeft);
			} else {
				if (playerIsWhite) {
					WhiteUpdate ();
				} else {
					BlackUpdate ();
				}
			}
		} catch (UnityException exception) {
			UnityEngine.Debug.Log ("Caught exception " + exception);
		}
	}
	void WhiteUpdate () {
		if (Input.GetMouseButtonDown (0)) {
			if (!selectedPiece) {
				Pos mousePos = GetMousePosition ();
				SelectPiece (mousePos);
			} else {
				Pos mousePos = GetMousePosition ();
				Move move = new Move (selectedPos, mousePos);
				if (whiteMoves.Contains (move)) {
					game.PlayMove (move);

					playerIsWhite = false;
					blackMoves = game.GetAllLegalBlackMoves ();

					if (blackMoves.Count <= 0) {
						PromptRestart ();
					}
					render.DeleteOldMoves ();
					animationMove = move;
					animationEndTime = Time.time + render.animationTime;

					selectedPiece = false;
				} else {
					selectedPiece = false;
					if (mousePos.x == selectedPos.x && mousePos.y == selectedPos.y) {
						render.DeleteOldMoves ();
					} else {
						SelectPiece (mousePos);
					}
				}
			}
		}
	}
	void SelectPiece (Pos position) {
		//if (WhiteCanMoveFromPos (position)) {

					selectedPiece = true;
					selectedPos = position;
					render.RenderMoves (game, position);

		//}
	}
	void BlackUpdate () {
		//UnityEngine.Debug.Log ("Prior value: " + Evaluator.BlackAdvantage (game));
		Stopwatch watch = new Stopwatch ();
		watch.Start ();
		bool isEndgame = Evaluator.IsEndgame (game);
		Move move = ai.GetImprovedMove (game, Controller.aiSearchDepth, false, isEndgame);
		UnityEngine.Debug.Log ("Endgame: " + isEndgame);
		game.PlayMove (move);
		UnityEngine.Debug.Log ("Total time: " + watch.Elapsed.TotalSeconds);
		//UnityEngine.Debug.Log ("After value: " + Evaluator.BlackAdvantage (game));

		playerIsWhite = true;
		animationMove = move;
		animationEndTime = Time.time + render.animationTime;
		whiteMoves = game.GetAllLegalWhiteMoves ();

		if (whiteMoves.Count <= 0) {
			PromptRestart ();
		}

	}
	void WhiteAiUpdate () {
		Stopwatch watch = new Stopwatch ();
		watch.Start ();
		bool isEndgame = Evaluator.IsEndgame (game);
		Move move = ai.GetImprovedMove (game, Controller.aiSearchDepth, true, isEndgame);
		UnityEngine.Debug.Log ("Endgame: " + isEndgame);
		game.PlayMove (move);
		UnityEngine.Debug.Log ("Total time: " + watch.Elapsed.TotalSeconds);
		//UnityEngine.Debug.Log ("After value: " + Evaluator.BlackAdvantage (game));

		playerIsWhite = false;
		animationMove = move;
		animationEndTime = Time.time + render.animationTime;
		blackMoves = game.GetAllLegalBlackMoves ();

		if (blackMoves.Count <= 0) {
			PromptRestart ();
		}

	}
	Pos GetMousePosition () {
		return MouseToBoardPos (
			Input.mousePosition.x - Screen.width / 2,
			Input.mousePosition.y - Screen.height / 2
		);
	}
	Pos MouseToBoardPos (float mouseX, float mouseY) {
		int x = (int)Mathf.Round((mouseX / 100) + 3.5f);
		int y = (int)Mathf.Round((mouseY / 100) + 3.5f);
		return new Pos (x, y);
	}
	bool WhiteCanMoveFromPos (Pos pos) {
		return (
		    game.IsOnBoard (pos)
		    && game.IsWhite (game.board [pos.x] [pos.y])
		    && game.AllLegalMovesFrom (pos).Count > 0
		);
	}
	public void PromptRestart () {
		restartButton.gameObject.SetActive (true);
	}
	public void Restart () {
		Awake ();
	}
}
