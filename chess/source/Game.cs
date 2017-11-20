using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct Move {
	public Pos startPos;
	public Pos endPos;
	public Move (Pos startPos, Pos endPos) {
		this.startPos = startPos;
		this.endPos = endPos;
	}
	public Move (int startX, int startY, int endX, int endY) {
		this.startPos = new Pos (startX, startY);
		this.endPos = new Pos (endX, endY);
	}
	public bool Equals (Move otherMove) {
		return (startPos.Equals (otherMove.startPos) && endPos.Equals (otherMove.endPos));
	}
	public void SetAs (Move otherMove) {
		startPos.SetAs (otherMove.startPos);
		endPos.SetAs (otherMove.endPos);
	}
};
public struct Pos {
	public int x;
	public int y;
	public Pos (int x, int y) {
		this.x = x;
		this.y = y;
	}
	public bool Equals (Pos otherPos) {
		return (x == otherPos.x && y == otherPos.y);
	}
	public void SetAs (Pos otherPos) {
		this.x = otherPos.x;
		this.y = otherPos.y;
	}
};
public enum Piece {
	WPawn,
	WKnight,
	WBishop,
	WRook,
	WQueen,
	WKing,

	BPawn,
	BKnight,
	BBishop,
	BRook,
	BQueen,
	BKing,

	Void
};
public class Game {

	public Piece[][] board;

	public Game () {
		SetUpBoard ();
	}
	public Game (Game game) {
		board = new Piece[8][] {
			new Piece[8],
			new Piece[8],
			new Piece[8],
			new Piece[8],
			new Piece[8],
			new Piece[8],
			new Piece[8],
			new Piece[8]
		};
		SetAs (game);
	}
	public void SetUpBoard () {
		board = new Piece[8] [] {
			new Piece[] { Piece.WRook, Piece.WPawn, Piece.Void, Piece.Void, Piece.Void, Piece.Void, Piece.BPawn, Piece.BRook},

			new Piece[] { Piece.WKnight, Piece.WPawn, Piece.Void, Piece.Void, Piece.Void, Piece.Void, Piece.BPawn, Piece.BKnight},

			new Piece[] { Piece.WBishop, Piece.WPawn, Piece.Void, Piece.Void, Piece.Void, Piece.Void, Piece.BPawn, Piece.BBishop},

			new Piece[] { Piece.WQueen, Piece.WPawn, Piece.Void, Piece.Void, Piece.Void, Piece.Void, Piece.BPawn, Piece.BQueen},

			new Piece[] { Piece.WKing, Piece.WPawn, Piece.Void, Piece.Void, Piece.Void, Piece.Void, Piece.BPawn, Piece.BKing},

			new Piece[] { Piece.WBishop, Piece.WPawn, Piece.Void, Piece.Void, Piece.Void, Piece.Void, Piece.BPawn, Piece.BBishop},

			new Piece[] { Piece.WKnight, Piece.WPawn, Piece.Void, Piece.Void, Piece.Void, Piece.Void, Piece.BPawn, Piece.BKnight},

			new Piece[] { Piece.WRook, Piece.WPawn, Piece.Void, Piece.Void, Piece.Void, Piece.Void, Piece.BPawn, Piece.BRook}		
		};
	}
	public bool wKingMoved = false;
	public bool wRRookMoved = false;
	public bool wLRookMoved = false;

	public bool bKingMoved = false;
	public bool bRRookMoved = false;
	public bool bLRookMoved = false;

	public bool IsWhite (Piece piece) {
		return (piece == Piece.WPawn || piece == Piece.WKnight || piece == Piece.WBishop || piece == Piece.WRook || piece == Piece.WQueen || piece == Piece.WKing);
	}
	public bool IsVoid (Piece piece) {
		return (piece == Piece.Void);
	}
	public bool IsBlack (Piece piece) {
		return (piece == Piece.BPawn || piece == Piece.BKnight || piece == Piece.BBishop || piece == Piece.BRook || piece == Piece.BQueen || piece == Piece.BKing);
	}

	public void PlayMove (Move move) {
		//Doesn't check whether move is legal
		if (move.startPos.Equals (new Pos (0, 0))) {
			wLRookMoved = true;
		}
		if (move.startPos.Equals (new Pos (7, 0))) {
			wRRookMoved = true;
		}	
		if (move.startPos.Equals (new Pos (4, 0))) {
			wKingMoved = true;
		}

		if (move.startPos.Equals (new Pos (0, 7))) {
			bLRookMoved = true;
		}
		if (move.startPos.Equals (new Pos (7, 7))) {
			bRRookMoved = true;
		}	
		if (move.startPos.Equals (new Pos (4, 7))) {
			bKingMoved = true;
		}

		if (move.startPos.Equals (new Pos (4, 0)) && move.endPos.Equals (new Pos (6, 0)) && board [move.startPos.x] [move.startPos.y] == Piece.WKing) {
			board [7] [0] = Piece.Void;
			board [5] [0] = Piece.WRook;
		} else if (move.startPos.Equals (new Pos (4, 0)) && move.endPos.Equals (new Pos (2, 0)) && board [move.startPos.x] [move.startPos.y] == Piece.WKing) {
			board [0] [0] = Piece.Void;
			board [3] [0] = Piece.WRook;
		}
		if (move.startPos.Equals (new Pos (4, 7)) && move.endPos.Equals (new Pos (6, 7)) && board [move.startPos.x] [move.startPos.y] == Piece.BKing) {
			board [7] [7] = Piece.Void;
			board [5] [7] = Piece.BRook;
		} else if (move.startPos.Equals (new Pos (4, 7)) && move.endPos.Equals (new Pos (2, 7)) && board [move.startPos.x] [move.startPos.y] == Piece.BKing) {
			board [0] [7] = Piece.Void;
			board [3] [7] = Piece.BRook;
		}
		board [move.endPos.x] [move.endPos.y] = board [move.startPos.x] [move.startPos.y];
		board [move.startPos.x] [move.startPos.y] = Piece.Void;

		if (board [move.endPos.x] [move.endPos.y] == Piece.WPawn && move.endPos.y == 7) {
			board [move.endPos.x] [move.endPos.y] = Piece.WQueen;
		}
		if (board [move.endPos.x] [move.endPos.y] == Piece.BPawn && move.endPos.y == 0) {
			board [move.endPos.x] [move.endPos.y] = Piece.BQueen;
		}
	}
	public bool MoveIsCapture (Move move) {
		return !IsVoid (board [move.endPos.x] [move.endPos.y]);
	}
	public void SetAs (Game game) {
		
		for (int x = 0; x < game.board.Length; x++) {
			for (int y = 0; y < game.board[x].Length; y++) {
				board [x] [y] = game.board [x] [y];
			}
		}

		wKingMoved = game.wKingMoved;
		wRRookMoved = game.wRRookMoved;
		wLRookMoved = game.wLRookMoved;

		bKingMoved = game.bKingMoved;
		bRRookMoved = game.bRRookMoved;
		bLRookMoved = game.bLRookMoved;
	}

	public bool TakesKing (Move move) {
		Piece piece = board [move.endPos.x] [move.endPos.y];
		return (piece == Piece.WKing || piece == Piece.BKing);
	}
	public bool WhiteInCheck () {
		for (int x = 0; x < board.Length; x++) {
			for (int y = 0; y < board [x].Length; y++) {
				if (IsBlack (board [x] [y]) && board[x][y] != Piece.BKing) {
					List<Move> newMoves = AllLegalMovesFrom (new Pos (x, y));
					foreach (Move move in newMoves) {
						if (board[move.endPos.x][move.endPos.y] == Piece.WKing) {
							return true;
						}
					}
				}
			}
		}
		return false;
	}
	public bool BlackInCheck () {
		for (int x = 0; x < board.Length; x++) {
			for (int y = 0; y < board [x].Length; y++) {
				if (IsWhite (board [x] [y]) && board[x][y] != Piece.WKing) {
					List<Move> newMoves = AllLegalMovesFrom (new Pos (x, y));
					foreach (Move move in newMoves) {
						if (board[move.endPos.x][move.endPos.y] == Piece.BKing) {
							return true;
						}
					}
				}
			}
		}
		return false;
	}
	public bool WhiteThreatens (int xPos, int yPos) {
		for (int x = 0; x < board.Length; x++) {
			for (int y = 0; y < board [x].Length; y++) {
				if (IsWhite (board [x] [y]) && board[x][y] != Piece.WKing) {
					List<Move> newMoves = AllLegalMovesFrom (new Pos (x, y));
					foreach (Move move in newMoves) {
						if (move.endPos.x == xPos && move.endPos.y == yPos) {
							return true;
						}
					}
				}
			}
		}
		return false;
	}
	public bool BlackThreatens (int xPos, int yPos) {
		for (int x = 0; x < board.Length; x++) {
			for (int y = 0; y < board [x].Length; y++) {
				if (IsBlack (board [x] [y]) && board[x][y] != Piece.BKing) {
					List<Move> newMoves = AllLegalMovesFrom (new Pos (x, y));
					foreach (Move move in newMoves) {
						if (move.endPos.x == xPos && move.endPos.y == yPos) {
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	public List<Move> GetAllWhiteMoves () {
		List<Move> moves = new List<Move> ();

		for (int x = 0; x < board.Length; x++) {
			for (int y = 0; y < board[x].Length; y++) {
				if (IsWhite (board [x] [y])) {
					List<Move> newMoves = AllLegalMovesFrom (new Pos (x, y));
					/*
					for (int i = 0; i < newMoves.Count; i++) {
						Game game2 = new Game (this);
						game2.PlayMove (newMoves[i]);
						if (game2.WhiteInCheck ()) {
							newMoves.RemoveAt (i);
							i--;
						}
					}
					*/
					moves.AddRange (newMoves);
				}
			}
		}
		return moves;
	}
	public List<Move> GetAllLegalWhiteMoves () {
		List<Move> moves = new List<Move> ();

		for (int x = 0; x < board.Length; x++) {
			for (int y = 0; y < board[x].Length; y++) {
				if (!IsWhite (board [x] [y])) {
					continue;
				}
				List<Move> newMoves = AllLegalMovesFrom (new Pos (x, y));
				for (int i = 0; i < newMoves.Count; i++) {
					Game game2 = new Game (this);
					game2.PlayMove (newMoves[i]);
					if (
						game2.WhiteInCheck ()
						|| (board[x][y] == Piece.WKing && game2.TouchingKing (newMoves[i].endPos.x, newMoves[i].endPos.y))
					) {
						newMoves.RemoveAt (i);
						i--;
					}
				}

				moves.AddRange (newMoves);
			}
		}
		return moves;
	}
	public List<Move> GetAllLegalBlackMoves () {
		List<Move> moves = new List<Move> ();

		for (int x = 0; x < board.Length; x++) {
			for (int y = 0; y < board[x].Length; y++) {
				if (!IsBlack (board [x] [y])) {
					continue;
				}
				List<Move> newMoves = AllLegalMovesFrom (new Pos (x, y));
				for (int i = 0; i < newMoves.Count; i++) {
					Game game2 = new Game (this);
					game2.PlayMove (newMoves[i]);
					if (
						game2.BlackInCheck ()
						|| (board[x][y] == Piece.BKing && game2.TouchingKing (newMoves[i].endPos.x, newMoves[i].endPos.y))
					) {
						newMoves.RemoveAt (i);
						i--;
					}
				}

				moves.AddRange (newMoves);
			}
		}
		return moves;
	}
	public bool TouchingKing (int x, int y) {
		for (int deltaX = -1; deltaX < 2; deltaX++) {
			for (int deltaY = -1; deltaY < 2; deltaY++) {
				if (deltaX == 0 && deltaY == 0) {
					continue;
				}

				if (IsOnBoard (x + deltaX, y + deltaY) && (board[x + deltaX][y + deltaY] == Piece.BKing || board[x + deltaX][y + deltaY] == Piece.WKing)) {
					return true;
				}
			}
		}
		return false;
	}
	public List<Move> GetAllBlackMoves () {
		List<Move> moves = new List<Move> ();

		for (int x = 0; x < board.Length; x++) {
			for (int y = 0; y < board[x].Length; y++) {
				if (IsBlack (board [x] [y])) {
					List<Move> newMoves = AllLegalMovesFrom (new Pos (x, y));
					/*
					for (int i = 0; i < newMoves.Count; i++) {
						Game game2 = new Game (this);
						game2.PlayMove (newMoves[i]);
						if (game2.BlackInCheck ()) {
							newMoves.RemoveAt (i);
							i--;
						}
					}
*/
					moves.AddRange (newMoves);
				}
			}
		}
		return moves;
	}
	public bool IsOnBoard (Pos pos) {
		return (pos.x >= 0 && pos.x < 8 && pos.y >= 0 && pos.y < 8);
	}
	public bool IsOnBoard (int x, int y) {
		return (x >= 0 && x < 8 && y >= 0 && y < 8);
	}
	public List<Move> AllLegalMovesFrom (Pos pos) {

		int x = pos.x;
		int y = pos.y;

		List<Move> moves = new List<Move> ();
		Piece piece = board [x] [y];

		//Pawns

		if (piece == Piece.WPawn) {
			//If pawn isn't on end
			if (y < 7) {
				if (IsVoid (board [x] [y + 1])) {
					moves.Add (new Move (pos, new Pos (x, y + 1)));
					if (y == 1 && IsVoid (board [x] [y + 2])) {
						moves.Add (new Move (pos, new Pos (x, y + 2)));
					}
				}
				if (x < 7 && IsBlack (board [x + 1] [y + 1])) {
					moves.Add (new Move (pos, new Pos (x + 1, y + 1)));
				}
				if (x > 0 && IsBlack (board [x - 1] [y + 1])) {
					moves.Add (new Move (pos, new Pos (x - 1, y + 1)));
				}
			}
			return moves;

		} else if (piece == Piece.BPawn) {
			//If pawn isn't on end
			if (y > 0) {
				if (IsVoid (board [x] [y - 1])) {
					moves.Add (new Move (pos, new Pos (x, y - 1)));
					if (y == 6 && IsVoid (board [x] [y - 2])) {
						moves.Add (new Move (pos, new Pos (x, y - 2)));
					}
				}
				if (x < 7 && IsWhite (board [x + 1] [y - 1])) {
					moves.Add (new Move (pos, new Pos (x + 1, y - 1)));
				}
				if (x > 0 && IsWhite (board [x - 1] [y - 1])) {
					moves.Add (new Move (pos, new Pos (x - 1, y - 1)));
				}
			}
			return moves;

		}

		//Knight

		else if (piece == Piece.WKnight) {
			List<Pos> JumpPos = new List<Pos> ();
			JumpPos.Add (new Pos (x + 1, y + 2));
			JumpPos.Add (new Pos (x - 1, y + 2));
			JumpPos.Add (new Pos (x + 1, y - 2));
			JumpPos.Add (new Pos (x - 1, y - 2));
			JumpPos.Add (new Pos (x + 2, y + 1));
			JumpPos.Add (new Pos (x - 2, y + 1));
			JumpPos.Add (new Pos (x + 2, y - 1));
			JumpPos.Add (new Pos (x - 2, y - 1));
			foreach (Pos i in JumpPos) {
				//If not jumping off board and position is not own colour
				if (IsOnBoard (i) && !IsWhite (board [i.x] [i.y])) {
					moves.Add (new Move (pos, i));
				}
			}
			return moves;

		} else if (piece == Piece.BKnight) {
			List<Pos> JumpPos = new List<Pos> ();
			JumpPos.Add (new Pos (x + 1, y + 2));
			JumpPos.Add (new Pos (x - 1, y + 2));
			JumpPos.Add (new Pos (x + 1, y - 2));
			JumpPos.Add (new Pos (x - 1, y - 2));
			JumpPos.Add (new Pos (x + 2, y + 1));
			JumpPos.Add (new Pos (x - 2, y + 1));
			JumpPos.Add (new Pos (x + 2, y - 1));
			JumpPos.Add (new Pos (x - 2, y - 1));
			foreach (Pos i in JumpPos) {
				//If not jumping off board and position is not own colour
				if (IsOnBoard (i) && !IsBlack (board [i.x] [i.y])) {
					moves.Add (new Move (pos, i));
				}
			}
			return moves;

		}

		//Bishops

		else if (piece == Piece.WBishop) {
			//For each diagonal
			for (int deltaX = -1; deltaX < 2; deltaX += 2) {
				for (int deltaY = -1; deltaY < 2; deltaY += 2) {

					int tempX = x + deltaX;
					int tempY = y + deltaY;
					while (IsOnBoard (tempX, tempY) && IsVoid (board [tempX] [tempY])) {
						moves.Add (new Move (pos, new Pos (tempX, tempY)));

						tempX += deltaX;
						tempY += deltaY;
					}
					if (IsOnBoard (tempX, tempY) && IsBlack (board[tempX] [tempY])) {
						moves.Add (new Move (pos, new Pos (tempX, tempY)));
					}

				}
			}
			return moves;

		} else if (piece == Piece.BBishop) {
			//For each diagonal
			for (int deltaX = -1; deltaX < 2; deltaX += 2) {
				for (int deltaY = -1; deltaY < 2; deltaY += 2) {

					int tempX = x + deltaX;
					int tempY = y + deltaY;
					while (IsOnBoard (tempX, tempY) && IsVoid (board [tempX] [tempY])) {
						moves.Add (new Move (pos, new Pos (tempX, tempY)));

						tempX += deltaX;
						tempY += deltaY;
					}
					if (IsOnBoard (tempX, tempY) && IsWhite (board[tempX] [tempY])) {
						moves.Add (new Move (pos, new Pos (tempX, tempY)));
					}

				}
			}
			return moves;

		}

		//Rook

		else if (piece == Piece.WRook) {
			List<Pos> vectors = new List<Pos> ();
			vectors.Add (new Pos (1, 0));
			vectors.Add (new Pos (-1, 0));
			vectors.Add (new Pos (0, 1));
			vectors.Add (new Pos (0, -1));

			foreach (Pos i in vectors) {
				int deltaX = i.x;
				int deltaY = i.y;

				int tempX = x + deltaX;
				int tempY = y + deltaY;
				while (IsOnBoard (tempX, tempY) && IsVoid (board [tempX] [tempY])) {
					moves.Add (new Move (pos, new Pos (tempX, tempY)));

					tempX += deltaX;
					tempY += deltaY;
				}
				if (IsOnBoard (tempX, tempY) && IsBlack (board[tempX] [tempY])) {
					moves.Add (new Move (pos, new Pos (tempX, tempY)));
				}
			}
			return moves;

		} else if (piece == Piece.BRook) {
			List<Pos> vectors = new List<Pos> ();
			vectors.Add (new Pos (1, 0));
			vectors.Add (new Pos (-1, 0));
			vectors.Add (new Pos (0, 1));
			vectors.Add (new Pos (0, -1));

			foreach (Pos i in vectors) {
				int deltaX = i.x;
				int deltaY = i.y;

				int tempX = x + deltaX;
				int tempY = y + deltaY;
				while (IsOnBoard (tempX, tempY) && IsVoid (board [tempX] [tempY])) {
					moves.Add (new Move (pos, new Pos (tempX, tempY)));

					tempX += deltaX;
					tempY += deltaY;
				}
				if (IsOnBoard (tempX, tempY) && IsWhite (board[tempX] [tempY])) {
					moves.Add (new Move (pos, new Pos (tempX, tempY)));
				}
			}
			return moves;

		}

		//Queen
		else if (piece == Piece.WQueen) {
			//For each diagonal
			for (int deltaX = -1; deltaX < 2; deltaX++) {
				for (int deltaY = -1; deltaY < 2; deltaY++) {
					if (deltaX == 0 && deltaY == 0) {
						continue;
					}
					int tempX = x + deltaX;
					int tempY = y + deltaY;
					while (IsOnBoard (tempX, tempY) && IsVoid (board [tempX] [tempY])) {
						moves.Add (new Move (pos, new Pos (tempX, tempY)));

						tempX += deltaX;
						tempY += deltaY;
					}
					if (IsOnBoard (tempX, tempY) && IsBlack (board[tempX] [tempY])) {
						moves.Add (new Move (pos, new Pos (tempX, tempY)));
					}

				}
			}
			return moves;

		} else if (piece == Piece.BQueen) {
			//For each diagonal
			for (int deltaX = -1; deltaX < 2; deltaX++) {
				for (int deltaY = -1; deltaY < 2; deltaY++) {
					if (deltaX == 0 && deltaY == 0) {
						continue;
					}

					int tempX = x + deltaX;
					int tempY = y + deltaY;
					while (IsOnBoard (tempX, tempY) && IsVoid (board [tempX] [tempY])) {
						moves.Add (new Move (pos, new Pos (tempX, tempY)));

						tempX += deltaX;
						tempY += deltaY;
					}
					if (IsOnBoard (tempX, tempY) && IsWhite (board[tempX] [tempY])) {
						moves.Add (new Move (pos, new Pos (tempX, tempY)));
					}

				}
			}
			return moves;

		}

		//King

		else if (piece == Piece.WKing) {
			for (int deltaX = -1; deltaX < 2; deltaX++) {
				for (int deltaY = -1; deltaY < 2; deltaY++) {
					if (deltaX == 0 && deltaY == 0) {
						continue;
					}

					if (IsOnBoard (x + deltaX, y + deltaY) && !IsWhite (board [x + deltaX] [y + deltaY])) {
						moves.Add (new Move (pos, new Pos (x + deltaX, y + deltaY)));
					}
				}
			}
			if (!wKingMoved && !wRRookMoved && IsVoid (board [5] [0]) && IsVoid (board [6] [0]) && board [7] [0] == Piece.WRook) {
				if (!(WhiteInCheck () || BlackThreatens (5, 0) || BlackThreatens (6, 0))) {
					moves.Add (new Move (pos, new Pos (6, 0)));
				}
			}
			if (!wKingMoved && !wLRookMoved && IsVoid (board [3] [0]) && IsVoid (board [2] [0]) && IsVoid (board [1] [0]) && board [0] [0] == Piece.WRook) {
				if (!(WhiteInCheck () || BlackThreatens (3, 0) || BlackThreatens (2, 0))) {
					moves.Add (new Move (pos, new Pos (2, 0)));
				}
			}
			return moves;

		} else if (piece == Piece.BKing) {
			for (int deltaX = -1; deltaX < 2; deltaX++) {
				for (int deltaY = -1; deltaY < 2; deltaY++) {
					if (deltaX == 0 && deltaY == 0) {
						continue;
					}

					if (IsOnBoard (x + deltaX, y + deltaY) && !IsBlack (board [x + deltaX] [y + deltaY])) {
						moves.Add (new Move (pos, new Pos (x + deltaX, y + deltaY)));
					}
				}
			}
			if (!bKingMoved && !bRRookMoved && IsVoid (board [5] [7]) && IsVoid (board [6] [7]) && board [7] [7] == Piece.BRook) {
				if (!(BlackInCheck () || WhiteThreatens (5, 7) || WhiteThreatens (6, 7))) {
					moves.Add (new Move (pos, new Pos (6, 7)));
				}
			}
			if (!bKingMoved && !bLRookMoved && IsVoid (board [3] [7]) && IsVoid (board [2] [7]) && IsVoid (board [1] [7]) && board [0] [7] == Piece.BRook) {
				if (!(BlackInCheck () || WhiteThreatens (3, 7) || WhiteThreatens (2, 7))) {
					moves.Add (new Move (pos, new Pos (2, 7)));
				}
			}
			return moves;

		}
		return moves;
	}
}
