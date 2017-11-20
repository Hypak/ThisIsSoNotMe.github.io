using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Evaluator {

	public static int pawnValue = 100;
	public static int repeatedPawnLoss = 40;
	public static int islandCountLoss = 20;

	public static int knightValue = 300;
	public static int knightPairLoss = 15;
	public static int knightBonusPerPawn = 2;
	public static int bishopValue = 320;
	public static int bishopPairBonus = 50;
	public static int bishopLossPerPawn = 2;

	public static int minorBackRankLoss = 20;

	public static int rookValue = 550;
	public static int rookPairLoss = 30;
	public static int rookHalfOpenFileBonus = 15;
	public static int rookOpenFileBonus = 20;

	public static int rookLossPerPawn = 2;
	public static int queenValue = 900;
	public static int queenHalfOpenFileBonus = 10;
	public static int queenOpenFileBonus = 15;

	public static int advantagePawnBonus = 5;
	public static int advantagePieceLoss = 10;

	public static int castleAbilityBonus = 35;

	public static int endGamePointThreshhold = 11800; //Add 10000 for king
	public static int endGameKingDistanceValue = 30; //Deducted for winning side, added to losing side
	public static int endGameKingDistanceAdvantage = 400; //Required advantage for king distance to matter

	public static int[] pawnSquare;
	public static int[] knightSquare;
	public static int[] bishopSquare;
	public static int[] rookSquare;
	public static int[] queenSquare;
	public static int[] kingSquareMidGame;
	public static int[] kingSquareEndGame;

	static Evaluator () {
		pawnSquare = new int[] {
			0,  0,  0,  0,  0,  0,  0,  0,
			50, 50, 50, 50, 50, 50, 50, 50,
			10, 10, 20, 30, 30, 20, 10, 10,
			5,  5,  10, 25, 25, 10, 5,  5,
			0,  0,  0,  20, 20, 0,  0,  0,
			5,  -5, -10,0,  0, -10, -5, 5,
			5,  10, 10, -20,-20,10, 10, 5,
			0,  0,  0,  0,  0,  0,  0,  0
		};
		knightSquare = new int[] {
			-50,-40,-30,-30,-30,-30,-40,-50,
			-40,-20,  0,  0,  0,  0,-20,-40,
			-30,  0, 10, 15, 15, 10,  0,-30,
			-30,  5, 15, 20, 20, 15,  5,-30,
			-30,  0, 15, 20, 20, 15,  0,-30,
			-30,  5, 10, 15, 15, 10,  5,-30,
			-40,-20,  0,  5,  5,  0,-20,-40,
			-50,-40,-30,-30,-30,-30,-40,-50
		};
		bishopSquare = new int[]  {
			-20,-10,-10,-10,-10,-10,-10,-20,
			-10,  0,  0,  0,  0,  0,  0,-10,
			-10,  0,  5, 10, 10,  5,  0,-10,
			-10,  5,  5, 10, 10,  5,  5,-10,
			-10,  0, 10, 10, 10, 10,  0,-10,
			-10, 10, 10, 10, 10, 10, 10,-10,
			-10,  5,  0,  0,  0,  0,  5,-10,
			-20,-10,-10,-10,-10,-10,-10,-20
		};
		rookSquare = new int[] { 
			0,  0,  0,  0,  0,  0,  0,  0,
			10, 20, 20, 20, 20, 20, 20,  10,
			-5,  0,  0,  0,  0,  0,  0, -5,
			-5,  0,  0,  0,  0,  0,  0, -5,
			-5,  0,  0,  0,  0,  0,  0, -5,
			-5,  0,  0,  0,  0,  0,  0, -5,
			-5,  0,  0,  0,  0,  0,  0, -5,
			0,  0,  10,  10,  10,  10,  0,  0
		};
		queenSquare = new int[] {
			-20,-10,-10, -5, -5,-10,-10,-20,
			-10,  0,  0,  0,  0,  0,  0,-10,
			-10,  0,  5,  5,  5,  5,  0,-10,
			-5,  0,  5,  5,  5,  5,  0, -5,
			0,  0,  5,  5,  5,  5,  0, -5,
			-10,  5,  5,  5,  5,  5,  0,-10,
			-10,  0,  5,  0,  0,  0,  0,-10,
			-20,-10,-10, -5, -5,-10,-10,-20
		};
		kingSquareMidGame = new int[] {
			-30,-40,-40,-50,-50,-40,-40,-30,
			-30,-40,-40,-50,-50,-40,-40,-30,
			-30,-40,-40,-50,-50,-40,-40,-30,
			-30,-40,-40,-50,-50,-40,-40,-30,
			-20,-30,-30,-40,-40,-30,-30,-20,
			-10,-20,-20,-20,-20,-20,-20,-10,
			20, 20,  0,  0,  0,  0, 20, 20,
			20, 30, 10,  0,  0, 10, 30, 20
		};
		kingSquareEndGame = new int[] {
			-50,-45,-40,-35,-35,-40,-45,-50,
			-45,-30,-10,-10, -10,-10,-30,-45,
			-40,-10, 20, 30, 30, 20,-10,-40,
			-35,-10, 30, 40, 40, 30,-10,-35,
			-35,-10, 30, 40, 40, 30,-10,-35,
			-40,-10, 20, 30, 30, 20,-10,-40,
			-45,-30,-10,-10,-10,-10,-30,-45,
			-50,-45,-40,-35,-35,-40,-45,-50
		};
	}

	public static int GetIslandCount (bool[] pawnFiles) {
		int islandCount = 0;
		bool prevWasFile = false;
		for (int i = 0; i < pawnFiles.Length; i++) {
			if (pawnFiles[i]) {
				if (!prevWasFile) {
					islandCount++;
					prevWasFile = true;
				}
			} else {
				prevWasFile = false;
			}
		}
		return islandCount;
	}
	public static int GetBasicPoint (Piece piece) {
		switch (piece) {
			case Piece.Void:
				break;
			case Piece.WPawn:
				return pawnValue;
			case Piece.WKnight:
				return knightValue;
			case Piece.WBishop:
				return bishopValue;
			case Piece.WRook:
				return rookValue;
			case Piece.WQueen:
				return queenValue;
			case Piece.BPawn:
				return pawnValue;
			case Piece.BKnight:
				return knightValue;
			case Piece.BBishop:
				return bishopValue;
			case Piece.BRook:
				return rookValue;
			case Piece.BQueen:
				return queenValue;
		}
		return 0;
	}

	public static int WhitePosToIndex (int x, int y) {
		return (7 - y) * 8 + x;
	}
	public static int BlackPosToIndex (int x, int y) {
		return y * 8 + x;
	}
	public static bool IsEndgame (Game game) {
		Piece[][] board = game.board;
		int bPoints = 0;
		int wPoints = 0;
		for (int x = 0; x < board.Length; x++) {
			for (int y = 0; y < board[x].Length; y++) {
				Piece piece = board [x] [y];
				if (piece == Piece.Void) {
					continue;
				}
				switch (board [x] [y]) {
					case (Piece.BKnight):
						bPoints += knightValue;
						break;
					case (Piece.WKnight):
						wPoints += knightValue;
						break;
					case (Piece.BBishop):
						bPoints += bishopValue;
						break;
					case (Piece.WBishop):
						wPoints += bishopValue;
						break;
					case (Piece.BRook):
						bPoints += rookValue;
						break;			
					case (Piece.WRook):
						wPoints += rookValue;
						break;
					case (Piece.BQueen):
						bPoints += queenValue;
						break;
					case (Piece.WQueen):
						wPoints += queenValue;
					break;	
				}	
			} //End y for loop
		} //End x for loop
		return System.Math.Min (wPoints, bPoints) < 1000;
	}
	public static int BlackAdvantage (Game game) {
		Piece[][] board = game.board;
		int bPoints = 0;
		int wPoints = 0;
		int bPawn = 0;
		int wPawn = 0;
		int bKnight = 0;
		int wKnight = 0;
		int bBishop = 0;
		int wBishop = 0;
		int bRook = 0;
		int wRook = 0;
		int bQueen = 0;
		int wQueen = 0;

		bool[] bPawnFiles = new bool[8];
		bool[] wPawnFiles = new bool[8];

		int bKingX = 0;
		int bKingY = 0;
		int wKingX = 0;
		int wKingY = 0;
		int kingDistance = 0;

		for (int x = 0; x < board.Length; x++) {
			bool bPawnInFile = false;
			bool wPawnInFile = false;
	
			int bRookCount = 0;
			int bQueenCount = 0;
			int wRookCount = 0;
			int wQueenCount = 0;

			for (int y = 0; y < board[x].Length; y++) {
				Piece piece = board [x] [y];
				if (piece == Piece.Void) {
					continue;
				}
				switch (board [x] [y]) {
				case (Piece.BPawn):
					bPawn++;
					if (bPawnInFile) {
						bPoints -= repeatedPawnLoss;
					}
					bPawnInFile = true;
					bPoints += pawnSquare [BlackPosToIndex (x, y)];
					break;
				case (Piece.WPawn):
					wPawn++;
					if (wPawnInFile) {
						wPoints -= repeatedPawnLoss;
					}
					wPawnInFile = true;
					wPoints += pawnSquare [WhitePosToIndex (x, y)];

					break;
				case (Piece.BKnight):
					bKnight++;
					bPoints += knightSquare [BlackPosToIndex (x, y)];

					break;
				case (Piece.WKnight):
					wKnight++;
					wPoints += knightSquare [WhitePosToIndex (x, y)];

					break;
				case (Piece.BBishop):
					bBishop++;
					bPoints += bishopSquare [BlackPosToIndex (x, y)];

					break;
				case (Piece.WBishop):
					wBishop++;
					wPoints += bishopSquare [WhitePosToIndex (x, y)];

					break;
				case (Piece.BRook):
					bRook++;
					bRookCount++;
					bPoints += rookSquare [BlackPosToIndex (x, y)];

					break;			
				case (Piece.WRook):
					wRookCount++;
					wRook++;
					wPoints += rookSquare [WhitePosToIndex (x, y)];

					break;
				case (Piece.BQueen):
					bQueenCount++;
					bQueen++;
					bPoints += queenSquare [BlackPosToIndex (x, y)];

					break;
				case (Piece.WQueen):
					wQueenCount++;
					wQueen++;
					wPoints += queenSquare [WhitePosToIndex (x, y)];

					break;
				case (Piece.BKing):
					bPoints += 10000;
					bKingX = x;
					bKingY = y;
					break;
				case (Piece.WKing):
					wPoints += 10000;
					wKingX = x;
					wKingY = y;
					break;
				}					
			} //End y for loop
			bPawnFiles [x] = bPawnInFile;
			wPawnFiles [x] = wPawnInFile;
			if (!bPawnInFile) {
				if (!wPawnInFile) {
					//Open
					bPoints += bRookCount * rookOpenFileBonus + bQueenCount * queenOpenFileBonus;
					wPoints += wRookCount * rookOpenFileBonus + wQueenCount * queenOpenFileBonus;

				} else {
					//Half open for black
					bPoints += bRookCount * rookHalfOpenFileBonus + bQueenCount * queenHalfOpenFileBonus;
				}
			} else {
				if (wPawnInFile) {
					//Half open for white
					wPoints += wRookCount * rookHalfOpenFileBonus + wQueenCount * queenHalfOpenFileBonus;
				} //else closed
			}
		} //End x for loop
		int pawnCount = bPawn + wPawn;
		bPoints += bPawn * pawnValue;
		bPoints -= GetIslandCount (bPawnFiles) * islandCountLoss;
		wPoints += wPawn * pawnValue;
		wPoints -= GetIslandCount (wPawnFiles) * islandCountLoss;

		int adjustedKnightValue = knightValue + knightBonusPerPawn * pawnCount;
		bPoints += bKnight * adjustedKnightValue;
		if (bKnight == 2) {
			bPoints -= knightPairLoss;
		}
		wPoints += wKnight * adjustedKnightValue;
		if (wKnight == 2) {
			wPoints -= knightPairLoss;
		}

		int adjustedBishopValue = bishopValue - bishopLossPerPawn * pawnCount;
		bPoints += bBishop * adjustedBishopValue;
		if (bBishop == 2) {
			bPoints += bishopPairBonus;
		}
		wPoints += wBishop * adjustedBishopValue;
		if (wBishop == 2) {
			wPoints += bishopPairBonus;
		}

		int adjustedRookValue = rookValue - rookLossPerPawn * pawnCount;
		bPoints += bRook * adjustedRookValue;
		if (bRook == 2) {
			bPoints -= rookPairLoss;
		}
		wPoints += wRook * adjustedRookValue;
		if (wRook == 2) {
			wPoints -= rookPairLoss;
		}

		bPoints += bQueen * queenValue;
		wPoints += wQueen * queenValue;

		if (!game.bKingMoved && !(game.bLRookMoved && game.bRRookMoved)) {
			bPoints += castleAbilityBonus;
		}
		if (!game.wKingMoved && !(game.wLRookMoved && game.wRRookMoved)) {
			wPoints += castleAbilityBonus;
		}


		if (System.Math.Min (wPoints, bPoints) < endGamePointThreshhold) {
			kingDistance = System.Math.Max (
				System.Math.Abs (bKingX - wKingX),
				System.Math.Abs (bKingY - wKingY)
			);
			if (bPoints > wPoints + endGameKingDistanceAdvantage) {
				bPoints -= kingDistance * endGameKingDistanceValue;
				wPoints += kingDistance * endGameKingDistanceValue;
				wPoints += kingSquareEndGame [WhitePosToIndex (wKingX, wKingY)];
			}
			else if (wPoints > bPoints + endGameKingDistanceAdvantage) {
				bPoints += kingDistance * endGameKingDistanceValue;
				wPoints -= kingDistance * endGameKingDistanceValue;
				bPoints += kingSquareEndGame [BlackPosToIndex (bKingX, bKingY)];
			} 
			else {
				bPoints += kingSquareEndGame [BlackPosToIndex (bKingX, bKingY)];
				wPoints += kingSquareEndGame [WhitePosToIndex (wKingX, wKingY)];
			}
		} else {
			bPoints += kingSquareMidGame [BlackPosToIndex (bKingX, bKingY)];
			wPoints += kingSquareMidGame [WhitePosToIndex (wKingX, wKingY)];
		}

		int advantage = bPoints - wPoints;

		if (advantage > 0) {
			advantage += 1000;
			advantage += advantagePawnBonus * bPawn;
			advantage -= advantagePieceLoss * (bBishop + bKnight + bRook + bQueen);
		}
		if (advantage < 0) {
			advantage -= 1000;
			advantage -= advantagePawnBonus * bPawn;
			advantage += advantagePieceLoss * (bBishop + bKnight + bRook + bQueen);
		}

		return advantage;
	}
}


