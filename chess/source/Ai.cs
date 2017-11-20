using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Ai : MonoBehaviour {

	int nonCaptureDepthDecrease = 2;
	int captureDepthDecrease = 1;

	public int endgameLegalDepth = 0;
	public int midGameLegalDepth = 0;

	public int toMoveScoreBonus = 15;

	public Render render;

	public int qSearches;

	public Game[] games;

	System.Diagnostics.Stopwatch moveGenWatch;
	System.Diagnostics.Stopwatch evaluatorWatch;
	int evaluateCount = 0;

	public List<Move> OrderMovesShuffle (List<Move> moves, Piece[][] board) {
		List<Move> newMoves = new List<Move> ();
		while (moves.Count > 0) {
			Debug.Log (newMoves.Count);
			Move move = moves [Random.Range (0, moves.Count)];
			moves.Remove (move);
			int value = Evaluator.GetBasicPoint (board[move.endPos.x][move.endPos.y]);
			bool inserted = false;
			for (int i = 0; i < newMoves.Count; i++) {
				if (value > Evaluator.GetBasicPoint (board[newMoves[i].endPos.x][newMoves[i].endPos.y])) {
					newMoves.Insert (i, move);
					inserted = true;
					break;
				}
			}
			if (!inserted) {
				newMoves.Add (move);
			}
		}
		return newMoves;
	}

	public Move GetImprovedMove (Game game, int depth, bool isWhite, bool isEndgame) {
		games = new Game[30];
		for (int i = 0; i < games.Length; i++) {
			games [i] = new Game ();
		}
		moveGenWatch = new System.Diagnostics.Stopwatch ();
		evaluatorWatch = new System.Diagnostics.Stopwatch ();
		evaluateCount = 0;

		List<Move> moves;
		moveGenWatch.Start ();
		if (isWhite) {
			moves = game.GetAllLegalWhiteMoves ();
		} else {
			moves = game.GetAllLegalBlackMoves ();
		}
		moveGenWatch.Stop ();

		moves = OrderMovesShuffle (moves, game.board);

		Debug.Log (moves[0].endPos.x + " " + moves[0].endPos.y);

		Move bestMove = moves[0];

		float alpha = float.MinValue;
		float beta = float.MaxValue;

		int legalDepth;
		if (isEndgame) {
			legalDepth = endgameLegalDepth;
		} else {
			legalDepth = midGameLegalDepth;
		}

		for (int i = 0; i < moves.Count; i++) {
			if (game.TakesKing (moves[i])) {
				//If a move allows ai to take king, that move is v. good
				return moves[i];
			}
			Game newGame = new Game (game);
			newGame.PlayMove (moves[i]);
			float value = Min (newGame, depth - 1, isWhite, alpha, beta, legalDepth);
//			Debug.Log (game.board [moves [i].startPos.x] [moves [i].startPos.y] + moves [i].endPos.x.ToString () + " " + moves[i].endPos.y.ToString () + "\t" + value);

			if (value > alpha) {
				alpha = value;
				bestMove = moves [i];
			}
			
		}
		Debug.Log (alpha);
		//Debug.Log ("Evaluate: " + evaluatorWatch.Elapsed.TotalSeconds);
		Debug.Log ("Move Gen: " + moveGenWatch.Elapsed.TotalSeconds);
		Debug.Log ("Evaluate count: " + evaluateCount);
		return bestMove;
	}
	public float Max (Game game, int depth, bool aiIsWhite, float alpha, float beta, int legalDepth) {
		if (depth <= 0) {
			if (aiIsWhite) {
				evaluatorWatch.Start ();
				int ret = toMoveScoreBonus - Evaluator.BlackAdvantage (game);
				evaluatorWatch.Stop ();
				evaluateCount++;
				return ret;
			} else {
				evaluatorWatch.Start ();
				int ret = toMoveScoreBonus + Evaluator.BlackAdvantage (game);
				evaluatorWatch.Stop ();
				evaluateCount++;

				return ret;
			}
		}
		//int highestValue = int.MinValue;
		List<Move> moves;
		moveGenWatch.Start ();
		if (depth > legalDepth) {
			if (aiIsWhite) {
				moves = game.GetAllLegalWhiteMoves ();
			} else {
				moves = game.GetAllLegalBlackMoves ();
			}
			moveGenWatch.Stop ();

			if (moves.Count == 0) { //Stalemate
				if (aiIsWhite && game.WhiteInCheck ()) {
					return depth * -10000;				
				}
				if (!aiIsWhite && game.BlackInCheck ()) {
					return depth * -10000;
				}
				return 0;
			}
		} else {
			if (aiIsWhite) {
				moves = game.GetAllWhiteMoves ();
			} else {
				moves = game.GetAllBlackMoves ();
			}
		}
		moveGenWatch.Stop ();

		foreach (Move move in moves) {
			if (game.TakesKing (move)) {
				return (depth + 1) * -10000;
			}
			int depthDecrease;
			if (game.board [move.endPos.x] [move.endPos.y] == Piece.Void) {
				depthDecrease = nonCaptureDepthDecrease;
			} else {
				depthDecrease = captureDepthDecrease;
			}

			games [depth].SetAs (game);
			games [depth].PlayMove (move);

			alpha = Mathf.Max (alpha, Min (games [depth], depth - depthDecrease, aiIsWhite, alpha, beta, legalDepth));
			if (alpha >= beta) {
				return alpha;
			}

			//highestValue = Mathf.Max (value, highestValue);
		}
		return alpha;
	}
	public float Min (Game game, int depth, bool aiIsWhite, float alpha, float beta, int legalDepth) {
		if (depth <= 0) {
			if (aiIsWhite) {
				evaluatorWatch.Start ();
				int ret = -toMoveScoreBonus - Evaluator.BlackAdvantage (game);
				evaluatorWatch.Stop ();
				evaluateCount++;

				return ret;
			} else {
				evaluatorWatch.Start ();
				int ret = -toMoveScoreBonus + Evaluator.BlackAdvantage (game);
				evaluatorWatch.Stop ();
				evaluateCount++;

				return ret;
			}
		}
		List<Move> moves;
		moveGenWatch.Start ();
		if (depth > legalDepth) {
			if (aiIsWhite) {
				moves = game.GetAllLegalBlackMoves ();
			} else {
				moves = game.GetAllLegalWhiteMoves ();
			}
			moveGenWatch.Stop ();

			if (moves.Count == 0) { //Stalemate
				if (aiIsWhite && game.BlackInCheck ()) {
					
					return depth * 10000;				
				}
				if (!aiIsWhite && game.WhiteInCheck ()) {
					return depth * 10000;
				}
				return 0;
			}
		} else {
			if (aiIsWhite) {
				moves = game.GetAllBlackMoves ();
			} else {
				moves = game.GetAllWhiteMoves ();
			}
		}
		moveGenWatch.Stop ();

		foreach (Move move in moves) {
			if (game.TakesKing (move)) {
				return (depth + 1) * -10000;
			}
			int depthDecrease;
			if (game.board [move.endPos.x] [move.endPos.y] == Piece.Void) {
				depthDecrease = nonCaptureDepthDecrease;
			} else {
				depthDecrease = captureDepthDecrease;
			}
			games [depth].SetAs (game);
			games [depth].PlayMove (move);
			beta = Mathf.Min (beta, Max (games [depth], depth - depthDecrease, aiIsWhite, alpha, beta, legalDepth));
			if (beta <= alpha) {
				return beta;
			}
		}
		return beta;
	}
}
