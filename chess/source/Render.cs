using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Render : MonoBehaviour {

	public GameObject canvas;

	public GameObject WPawn;
	public GameObject WKnight;
	public GameObject WBishop;
	public GameObject WRook;
	public GameObject WQueen;
	public GameObject WKing;

	public GameObject BPawn;
	public GameObject BKnight;
	public GameObject BBishop;
	public GameObject BRook;
	public GameObject BQueen;
	public GameObject BKing;

	public GameObject possibleMoveObject;

	public float pieceSize = 0.9f;
	public float animationTime = 0.5f;

	public Dictionary <Piece, GameObject> pieceToObject = new Dictionary<Piece, GameObject> ();

	public void Awake () {

		pieceToObject.Add (Piece.WPawn, WPawn);
		pieceToObject.Add (Piece.WKnight, WKnight);
		pieceToObject.Add (Piece.WBishop, WBishop);
		pieceToObject.Add (Piece.WRook, WRook);
		pieceToObject.Add (Piece.WQueen, WQueen);
		pieceToObject.Add (Piece.WKing, WKing);

		pieceToObject.Add (Piece.BPawn, BPawn);
		pieceToObject.Add (Piece.BKnight, BKnight);
		pieceToObject.Add (Piece.BBishop, BBishop);
		pieceToObject.Add (Piece.BRook, BRook);
		pieceToObject.Add (Piece.BQueen, BQueen);
		pieceToObject.Add (Piece.BKing, BKing);

	}
	public Vector3 CoordToWorld (int boardX, int boardY) {
		float pieceWidth = 100;
		float pieceHeight = 100;

		float worldX = (boardX - 3.5f) * pieceWidth;
		float worldY = (boardY - 3.5f) * pieceHeight;
		return new Vector3 (worldX, worldY, 0);
	}

	public void Rend (Game game) {
		DeleteOldPieces ();

		Piece[][] board = game.board;
		for (int x = 0; x < board.Length; x++) {
			for (int y = 0; y < board[x].Length; y++) {
				if (board [x] [y] != Piece.Void) {
					RenderPiece (game, x, y);
				}
			}
		}
	}
	public void RenderMoves (Game game, Pos piecePos) {
		DeleteOldMoves ();
		foreach (Move move in game.GetAllLegalWhiteMoves ()) {
			if (move.startPos.x == piecePos.x && move.startPos.y == piecePos.y) {
				RenderMove (move);
			}
		}
	}

	public void RenderAnimation (Game game, Move move, float time) {
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag ("Render")) {
			GameObject.Destroy (obj);
		}
		Piece[][] board = game.board;
		for (int x = 0; x < board.Length; x++) {
			for (int y = 0; y < board[x].Length; y++) {
				if (
					board [x] [y] == Piece.Void 
					|| (x == move.endPos.x && y == move.endPos.y)) 
				{
					continue;
				}
				RenderPiece (game, x, y);
			}
		}
		GameObject animatedObj = pieceToObject [game.board[move.endPos.x][move.endPos.y]];
		Vector3 pos = GetAnimationPos (move, time);
		RenderWorldPiece (animatedObj, pos);
	}

	public void DeleteOldMoves () {
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag ("Move")) {
			GameObject.Destroy (obj);
		}
	}
	public void DeleteOldPieces () {
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag ("Render")) {
			GameObject.Destroy (obj);
		}
	}

	void RenderPiece (Game game, int x, int y) {
		GameObject obj = pieceToObject [game.board [x] [y]];
		Vector3 vec3 = CoordToWorld (x, y);
		GameObject gameObj = GameObject.Instantiate (obj, canvas.transform);
		gameObj.transform.localPosition = vec3;
		gameObj.transform.localScale = new Vector3 (pieceSize, pieceSize, pieceSize);
	}
	void RenderMove (Move move) {
		Vector3 vec3 = CoordToWorld (move.endPos.x, move.endPos.y);
		GameObject gameObj = GameObject.Instantiate (possibleMoveObject, canvas.transform);
		gameObj.transform.localPosition = vec3;
		Image image = gameObj.GetComponent <Image> ();
		image.color = new Color (0.4f, 1, 0.8f, 0.5f);
	}
	void RenderWorldPiece (GameObject obj, Vector3 pos) {
		GameObject gameObj = GameObject.Instantiate (obj, canvas.transform);
		gameObj.transform.localPosition = pos;
		gameObj.transform.localScale = new Vector3 (pieceSize, pieceSize, pieceSize);
	}

	Vector3 GetAnimationPos (Move move, float time) {
		Vector3 start = CoordToWorld (move.startPos.x, move.startPos.y);
		Vector3 end = CoordToWorld (move.endPos.x, move.endPos.y);
		Vector3 diff = end - start;
		//
		float proportion = time / animationTime;
		float smoothedPropotion = SmoothedProportion (proportion);
		return start + diff * smoothedPropotion;
	}

	float SmoothedProportion (float proportion) {
		float scaled = proportion * 2 - 1; //Scaled from (0, 1) to (-1, 1)
		if (scaled > 0) {
			return -0.5f * scaled * scaled + scaled + 0.5f;
		} else {
			return 0.5f * scaled * scaled + scaled + 0.5f;
		}
	}
}
