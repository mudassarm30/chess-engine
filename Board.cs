using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Originsoft
{
    public static class Board
    {
        
        public static String[,] ChessBoard { get; set; } = new String[8, 8]{
                {"r","n","b","q","k","b","n","r"},
                {"p","p","p","p","p","p","p","p"},
                {" "," "," "," "," "," "," "," "},
                {" "," "," "," "," "," "," "," "},
                {" "," "," "," "," "," "," "," "},
                {" "," "," "," "," "," "," "," "},
                {"P","P","P","P","P","P","P","P"},
                {"R","N","B","Q","K","B","N","R"}};

        public static string LastMove { get; set; } = "";
        public static Boolean WhiteToMove { get; set; } = false;
        public static Boolean CWK { get; set; } = false;
        public static Boolean CWQ { get; set; } = false;
        public static Boolean CBK { get; set; } = false;
        public static Boolean CBQ { get; set; } = false;
        public static Boolean CB { get; set; } = false;
        public static Boolean CW { get; set; } = false;

        public static void Print()
        {
            Console.WriteLine("");
            for(var r = 0; r < 8; r++)
            {
                Console.Write("info [");
                for(var c = 0; c < 8; c++)
                {
                    Console.Write(ChessBoard[r, c]);
                    if (c < 7)
                        Console.Write(",");
                }
                Console.Write("]\n");
            }
        }

        public static string GetAllPossibleMoves()
        {
            var possibilities = "";
            for(var r = 0; r < 8; r++)
            {
                for(var c = 0; c < 8; c++)
                {
                    var position = FromCoordinates(r, c);
                    var piece = GetPiece(r, c);
                    var opponent = Opponent(piece);

                    if(piece != " " && !opponent)
                    {
                        possibilities += PieceMoves(piece, position, r, c, 0, 0);
                    }
                }
            }

            return FilterPossibleMoves(possibilities.Trim()).Trim();
        }

        private static string FilterPossibleMoves(string moves)
        {
            var possibleMoves = moves.Split(" ");
            var filteredMoves = "";

            foreach(var possibleMove in possibleMoves)
            {
                var move = possibleMove.Trim();

                if(TryMove(move))
                {
                    filteredMoves += (move + " ");
                }
            }

            return filteredMoves;
        }

        private static Boolean TryMove(string move)
        {
            var _WhiteToMove = WhiteToMove;
            var _CWK = CWK;
            var _CWQ = CWQ;
            var _CBK = CBK;
            var _CBQ = CBQ;
            var _CB = CB;
            var _CW = CW;
            var _LastMove = LastMove;
            var clonedBoard = (String[,])ChessBoard.Clone();

            MakeMove(move);

            var possible = true;

            if ((_WhiteToMove && KingInCheck("K")) || !_WhiteToMove && KingInCheck("k"))
                possible = false;
            
            switch(move)
            {
                case "e1g1":
                    if (KingInCheck("K", "e1") || KingInCheck("K", "f1") || KingInCheck("K", "g1"))
                        possible = false;
                    break;
                case "e1c1":
                    if (KingInCheck("K", "e1") || KingInCheck("K", "d1") || KingInCheck("K", "c1"))
                        possible = false;
                    break;
                case "e8g8":
                    if (KingInCheck("k", "e8") || KingInCheck("k", "f8") || KingInCheck("k", "g8"))
                        possible = false;
                    break;
                case "e8c8":
                    if (KingInCheck("k", "e8") || KingInCheck("k", "d8") || KingInCheck("k", "c8"))
                        possible = false;
                    break;
            }
            
            ChessBoard = clonedBoard;
            WhiteToMove = _WhiteToMove;
            CWK = _CWK;
            CWQ = _CWQ;
            CBK = _CBK;
            CBQ = _CBQ;
            CB = _CB;
            CW = _CW;
            LastMove = _LastMove;

            return possible;
        }

        public static Boolean KingInCheck(string king)
        {
            for (var r = 0; r < 8; r++)
            {
                for (var c = 0; c < 8; c++)
                {
                    var position = FromCoordinates(r, c);
                    var piece = GetPiece(r, c);

                    if(king == piece)
                    {
                        return KingInCheck(king, position, r, c, 0, 0, 0);
                    }
                }
            }

            return false;
        }

        private static Boolean KingInCheck(string king, string pos)
        {
            var coords = ToCoordinates(pos);
            return KingInCheck(king, pos, coords.Item1, coords.Item2, 0, 0, 0);
        }

        private static Boolean KingInCheck(string king, string pos, int r, int c, int dr, int dc, int dist)
        {

            if (r < 0 || r > 7 || c < 0 || c > 7)
                return false;

            var pot = FromCoordinates(r, c);

            if(pos == pot)
            {
                return KingInCheck(king, pos, r + 1, c, 1, 0, dist + 1) || KingInCheck(king, pos, r - 1, c, -1, 0, dist + 1) ||
                       KingInCheck(king, pos, r, c + 1, 0, 1, dist + 1) || KingInCheck(king, pos, r, c - 1, 0, -1, dist + 1) ||

                       KingInCheck(king, pos, r + 1, c + 1, 1, 1, dist + 1) || KingInCheck(king, pos, r - 1, c + 1, -1, 1, dist + 1) ||
                       KingInCheck(king, pos, r + 1, c - 1, 1, -1, dist + 1) || KingInCheck(king, pos, r - 1, c - 1, -1, -1, dist + 1) ||

                       KingInCheck(king, pos, r + 1, c + 2, 1, 2, dist + 1) || KingInCheck(king, pos, r + 1, c - 2, 1, -2, dist + 1) ||
                       KingInCheck(king, pos, r - 1, c + 2, -1, 2, dist + 1) || KingInCheck(king, pos, r - 1, c - 2, -1, -2, dist + 1) ||
                       KingInCheck(king, pos, r + 2, c + 1, 2, 1, dist + 1) || KingInCheck(king, pos, r + 2, c - 1, 2, -1, dist + 1) ||
                       KingInCheck(king, pos, r - 2, c + 1, -2, 1, dist + 1) || KingInCheck(king, pos, r - 2, c - 1, -2, -1, dist + 1);

            }
            else
            {
                var piece = GetPiece(pot);

                if(king == "K")
                {
                    if (((Math.Abs(dr) == 1 && Math.Abs(dc) == 2) || (Math.Abs(dr) == 2 && Math.Abs(dc) == 1)) && piece == "n")
                        return true;

                    if((dr == 0 && Math.Abs(dc) == 1) || (Math.Abs(dr) == 1 && dc == 0))
                    {
                        if (piece == "q" || piece == "r")
                            return true;

                        if (dist == 1 && piece == "k")
                            return true;

                        if (piece == " ")
                            return KingInCheck(king, pos, r + dr, c + dc, dr, dc, dist + 1);
                    }
                    else if ((Math.Abs(dr) == 1 && Math.Abs(dc) == 1))
                    {
                        if (piece == "q" || piece == "b")
                            return true;

                        if (dist == 1 && piece == "k")
                            return true;

                        if (dist == 1 && piece == "p" && dr == -1)
                            return true;

                        if (piece == " ")
                            return KingInCheck(king, pos, r + dr, c + dc, dr, dc, dist + 1);
                    }
                }
                else if(king == "k")
                {
                    if (((Math.Abs(dr) == 1 && Math.Abs(dc) == 2) || (Math.Abs(dr) == 2 && Math.Abs(dc) == 1)) && piece == "N")
                        return true;

                    if ((dr == 0 && Math.Abs(dc) == 1) || (Math.Abs(dr) == 1 && dc == 0))
                    {
                        if (piece == "Q" || piece == "R")
                            return true;

                        if (dist == 1 && piece == "K")
                            return true;

                        if (piece == " ")
                            return KingInCheck(king, pos, r + dr, c + dc, dr, dc, dist + 1);
                    }
                    else if ((Math.Abs(dr) == 1 && Math.Abs(dc) == 1))
                    {
                        if (piece == "Q" || piece == "B")
                            return true;

                        if (dist == 1 && piece == "K")
                            return true;

                        if (dist == 1 && piece == "P" && dr == 1)
                            return true;

                        if (piece == " ")
                            return KingInCheck(king, pos, r + dr, c + dc, dr, dc, dist + 1);
                    }
                }
            }

            return false;
        }

        // Check if the piece [pc] at position [pos] can move to position [r, c]
        // The params dr and dc are used recursion for directions
        //
        // Note: The space separated string of moves may contain illegal moves and will require futher filtering e.g.
        //
        //       1) Moving a piece may cause the king to get checked
        //       2) May be the piece is the king and new position is on check.
        private static string PieceMoves(string pc, string pos, int r, int c, int dr, int dc)
        {

            // During recursion, check if we moved out of board,
            // If yes then that move is not possible

            if (r < 0 || r > 7 || c < 0 || c > 7)
                return "";

            // The potential position [pot] where move possibility is being checked
            var pot = FromCoordinates(r, c);
            
            // The piece (or empty place) at the position [pot]
            var cpc = GetPiece(r, c);

            switch (pc)
            {
                case "r":
                case "R":

                    if(pos == pot)
                    {
                        // First entry in the recursion, so we will move in four directions 
                        // horizontally and vertically, both directions and will collect all possible moves

                        return PieceMoves(pc, pos, r + 1, c, 1, 0) + PieceMoves(pc, pos, r - 1, c, -1, 0) +
                               PieceMoves(pc, pos, r, c + 1, 0, 1) + PieceMoves(pc, pos, r, c - 1, 0, -1);
                        
                    }
                    else
                    {
                        // If the box at place [r, c] is empty,
                        // add that move possibility and go in the same direction
                        // here dr and dc will help us to keep the same direction

                        if(cpc == " ")
                        {
                            return (pos + pot) + " " + PieceMoves(pc, pos, r + dr, c + dc, dr, dc);
                        }

                        // If the piece [cpc] is from opposite side
                        // add that move possibility and stop the recursion

                        else if(Opponent(cpc))
                        {
                            return (pos + pot) + " ";
                        }
                    }

                    break;

                case "n":
                case "N":

                    if (pos == pot)
                    {
                        // First entry in the recursion, so we will move in 8 L shaped jumps and will collect all possible moves

                        return PieceMoves(pc, pos, r + 1, c + 2, 1, 2) + PieceMoves(pc, pos, r + 1, c - 2, 1, -2) +
                               PieceMoves(pc, pos, r - 1, c + 2, -1, 2) + PieceMoves(pc, pos, r - 1, c - 2, -1, -2) +
                               PieceMoves(pc, pos, r + 2, c + 1, 2, 1) + PieceMoves(pc, pos, r + 2, c - 1, 2, -1) +
                               PieceMoves(pc, pos, r - 2, c + 1, -2, 1) + PieceMoves(pc, pos, r - 2, c - 1, -2, -1);

                    }
                    else
                    {
                        // If the box at place [r, c] is empty or has an opponent piece,
                        // add that move possibility and end recursion

                        if (cpc == " " || Opponent(cpc))
                        {
                            return (pos + pot) + " ";
                        }
                    }

                    break;

                case "b":
                case "B":

                    if (pos == pot)
                    {
                        // First entry in the recursion, so we will move in four directions 
                        // diagonally and will collect all possible moves

                        return PieceMoves(pc, pos, r + 1, c + 1, 1, 1) + PieceMoves(pc, pos, r - 1, c - 1, -1, -1) +
                               PieceMoves(pc, pos, r - 1, c + 1, -1, 1) + PieceMoves(pc, pos, r + 1, c - 1, 1, -1);

                    }
                    else
                    {
                        // If the box at place [r, c] is empty,
                        // add that move possibility and go in the same direction
                        // here dr and dc will help us to keep the same direction

                        if (cpc == " ")
                        {
                            return (pos + pot) + " " + PieceMoves(pc, pos, r + dr, c + dc, dr, dc);
                        }

                        // If the piece [cpc] is from opposite side
                        // add that move possibility and stop the recursion

                        else if (Opponent(cpc))
                        {
                            return (pos + pot) + " ";
                        }
                    }

                    break;

                case "q":
                case "Q":

                    if (pos == pot)
                    {
                        // First entry in the recursion, so we will move in four directions 
                        // diagonally, horizontally and vertically, both directions and will collect all possible moves

                        return PieceMoves(pc, pos, r + 1, c, 1, 0) + PieceMoves(pc, pos, r - 1, c, -1, 0) +
                               PieceMoves(pc, pos, r, c + 1, 0, 1) + PieceMoves(pc, pos, r, c - 1, 0, -1) +
                               PieceMoves(pc, pos, r + 1, c + 1, 1, 1) + PieceMoves(pc, pos, r - 1, c - 1, -1, -1) +
                               PieceMoves(pc, pos, r - 1, c + 1, -1, 1) + PieceMoves(pc, pos, r + 1, c - 1, 1, -1);

                    }
                    else
                    {
                        // If the box at place [r, c] is empty,
                        // add that move possibility and go in the same direction
                        // here dr and dc will help us to keep the same direction

                        if (cpc == " ")
                        {
                            return (pos + pot) + " " + PieceMoves(pc, pos, r + dr, c + dc, dr, dc);
                        }

                        // If the piece [cpc] is from opposite side
                        // add that move possibility and stop the recursion

                        else if (Opponent(cpc))
                        {
                            return (pos + pot) + " ";
                        }
                    }

                    break;

                case "k":
                case "K":

                    if (pos == pot)
                    {
                        // First entry in the recursion, so we will move in four directions 
                        // diagonally, horizontally and vertically, both directions and will collect all possible moves

                        return PieceMoves(pc, pos, r + 1, c, 1, 0) + PieceMoves(pc, pos, r - 1, c, -1, 0) +
                               PieceMoves(pc, pos, r, c + 1, 0, 1) + PieceMoves(pc, pos, r, c - 1, 0, -1) +
                               PieceMoves(pc, pos, r + 1, c + 1, 1, 1) + PieceMoves(pc, pos, r - 1, c - 1, -1, -1) +
                               PieceMoves(pc, pos, r - 1, c + 1, -1, 1) + PieceMoves(pc, pos, r + 1, c - 1, 1, -1) +

                               // Castle possibilities
                               PieceMoves(pc, pos, r, c + 2, 0, 2) + PieceMoves(pc, pos, r, c - 2, 0, -2);

                    }
                    else
                    {
                        // Check if castling is being considered
                        if (Math.Abs(dc) == 2)
                        {
                            if (cpc != " ")
                                return "";

                            if ((!CW && pos == "e1" && dc == 2 && CWK && WhiteToMove && GetPiece("f1") == " " && GetPiece("g1") == " ") || (!CW && pos == "e1" && dc == -2 && CWQ && WhiteToMove && GetPiece("d1") == " " && GetPiece("c1") == " " && GetPiece("b1") == " ") ||
                                (!CB && pos == "e8" && dc == 2 && CBK && !WhiteToMove && GetPiece("f8") == " " && GetPiece("g8") == " ") || (!CB && pos == "e8" && dc == -2 && CBQ && !WhiteToMove && GetPiece("d8") == " " && GetPiece("c8") == " " && GetPiece("b8") == " ") )
                                return (pos + pot) + " ";

                            return "";
                        }
                        else
                        {
                            // If the box at place [r, c] is empty or has an opponent piece,
                            // add that move possibility and end the recursion

                            if (cpc == " " || Opponent(cpc))
                            {
                                return (pos + pot) + " ";
                            }
                        }
                    }

                    break;

                case "p":
                case "P":

                    var dir = (pc == "p") ? 1 : -1;

                    if (pos == pot)
                    {
                        // First entry in the recursion, check if black (dir = 1) or white (dir = -1) pawn can move one or two boxes
                        // or if it can capture diagonally in each diagonal direction

                        return PieceMoves(pc, pos, r + dir, c, dir, 0) + PieceMoves(pc, pos, r + dir*2, c, dir*2, 0) +
                               PieceMoves(pc, pos, r + dir, c + 1, dir, 1) + PieceMoves(pc, pos, r + dir, c - 1, dir, -1);

                    }
                    else
                    {
                        if (Math.Abs(dr) == 1 && dc == 0)
                        {
                            // If the box at place [r, c] is empty,
                            // add that move possibility stop recursion

                            if (cpc == " ")
                            {
                                return (pos + pot) + " ";
                            }
                        }
                        else if (Math.Abs(dr) == 2 && dc == 0 && GetPiece(r - dir, c) == " " && (((r - 2*dir) == 6 && WhiteToMove) || ((r - 2 * dir) == 1 && !WhiteToMove)))
                        {
                            // If the box at place [r, c] is empty,
                            // add that move possibility stop recursion

                            if (cpc == " ")
                            {
                                return (pos + pot) + " ";
                            }
                        }
                        else if(Math.Abs(dc) == 1 && Opponent(cpc))
                        {
                            if ((dr == -1 && pc == "P" && WhiteToMove) || (dr == 1 && pc == "p" && !WhiteToMove))
                                return (pos + pot) + " ";
                        }
                        else if (Math.Abs(dc) == 1 && (cpc == " "))
                        {
                            if ((dr == -1 && pc == "P" && WhiteToMove))
                            {
                                if(dc == 1)
                                {
                                    if( (pos == "a5" && LastMove == "b7b5") ||
                                        (pos == "b5" && LastMove == "c7c5") ||
                                        (pos == "c5" && LastMove == "d7d5") ||
                                        (pos == "d5" && LastMove == "e7e5") ||
                                        (pos == "e5" && LastMove == "f7f5") ||
                                        (pos == "f5" && LastMove == "g7g5") ||
                                        (pos == "g5" && LastMove == "h7h5") )
                                        return (pos + pot) + " ";
                                }
                                else if(dc == -1)
                                {
                                    if ((pos == "b5" && LastMove == "a7a5") ||
                                        (pos == "c5" && LastMove == "b7b5") ||
                                        (pos == "d5" && LastMove == "c7c5") ||
                                        (pos == "e5" && LastMove == "d7d5") ||
                                        (pos == "f5" && LastMove == "e7e5") ||
                                        (pos == "g5" && LastMove == "f7f5") ||
                                        (pos == "h5" && LastMove == "g7g5"))
                                        return (pos + pot) + " ";
                                }
                            }
                                
                            if ((dr == 1 && pc == "p" && !WhiteToMove))
                            {
                                if (dc == 1)
                                {
                                    if ((pos == "a4" && LastMove == "b2b4") ||
                                        (pos == "b4" && LastMove == "c2c4") ||
                                        (pos == "c4" && LastMove == "d2d4") ||
                                        (pos == "d4" && LastMove == "e2e4") ||
                                        (pos == "e4" && LastMove == "f2f4") ||
                                        (pos == "f4" && LastMove == "g2g4") ||
                                        (pos == "g4" && LastMove == "h2h4"))
                                        return (pos + pot) + " ";
                                }
                                else if (dc == -1)
                                {
                                    if ((pos == "b4" && LastMove == "a2a4") ||
                                        (pos == "c4" && LastMove == "b2b4") ||
                                        (pos == "d4" && LastMove == "c2c4") ||
                                        (pos == "e4" && LastMove == "d2d4") ||
                                        (pos == "f4" && LastMove == "e2e4") ||
                                        (pos == "g4" && LastMove == "f2f4") ||
                                        (pos == "h4" && LastMove == "g2g4"))
                                        return (pos + pot) + " ";
                                }
                            }
                        }
                    }

                    break;                    
            }

            return "";
        }

        public static Boolean Opponent(string p)
        {
            if (p == " ")
                return false;

            if (WhiteToMove && (p == "R" || p == "N" || p == "B" || p == "K" || p == "Q" || p == "P"))
                return false;

            if (!WhiteToMove && (p == "r" || p == "n" || p == "b" || p == "k" || p == "q" || p == "p"))
                return false;

            return true;
        }

        public static string FromCoordinates(int row, int col)
        {
            return ((char)('a' + col)).ToString() + ((char)((8 - row) + '0')).ToString();
        }

        public static string FromCoordinates(Tuple<int, int> coordinates)
        {
            return FromCoordinates(coordinates.Item1, coordinates.Item2);
        }

        public static Tuple<int, int> ToCoordinates(String pos)   
        {
            int col = pos.ToCharArray()[0] - 'a';
            int row = 8 - (pos.ToCharArray()[1] - '0');

            return Tuple.Create<int, int>(row, col);
        }

        public static Boolean CapturePawnPassant(Tuple<int, int> _from, Tuple<int, int> _to)
        {
            var from = FromCoordinates(_from);
            var to = FromCoordinates(_to);

            if (WhiteToMove)
            {
                if(from == "a5" && to == "b6" && GetPiece(from) == "P" && LastMove == "b7b5" && GetPiece("b5") == "p")
                {
                    SetPiece("a5", " ");SetPiece("b5", " ");SetPiece("b6", "P"); return true;
                }
                if(from == "b5" && to == "c6" && GetPiece(from) == "P" && LastMove == "c7c5" && GetPiece("c5") == "p")
                {
                    SetPiece("b5", " "); SetPiece("c5", " "); SetPiece("c6", "P"); return true;
                }
                if (from == "c5" && to == "d6" && GetPiece(from) == "P" && LastMove == "d7d5" && GetPiece("d5") == "p")
                {
                    SetPiece("c5", " "); SetPiece("d5", " "); SetPiece("d6", "P"); return true;
                }
                if (from == "d5" && to == "e6" && GetPiece(from) == "P" && LastMove == "e7e5" && GetPiece("e5") == "p")
                {
                    SetPiece("d5", " "); SetPiece("e5", " "); SetPiece("e6", "P"); return true;
                }
                if (from == "e5" && to == "f6" && GetPiece(from) == "P" && LastMove == "f7f5" && GetPiece("f5") == "p")
                {
                    SetPiece("e5", " "); SetPiece("f5", " "); SetPiece("f6", "P"); return true;
                }
                if (from == "f5" && to == "g6" && GetPiece(from) == "P" && LastMove == "g7g5" && GetPiece("g5") == "p")
                {
                    SetPiece("f5", " "); SetPiece("g5", " "); SetPiece("g6", "P"); return true;
                }
                if (from == "g5" && to == "h6" && GetPiece(from) == "P" && LastMove == "h7h5" && GetPiece("h5") == "p")
                {
                    SetPiece("g5", " "); SetPiece("h5", " "); SetPiece("h6", "P"); return true;
                }

                if (from == "b5" && to == "a6" && GetPiece(from) == "P" && LastMove == "a7a5" && GetPiece("a5") == "p")
                {
                    SetPiece("b5", " "); SetPiece("a5", " "); SetPiece("a6", "P"); return true;
                }
                if (from == "c5" && to == "b6" && GetPiece(from) == "P" && LastMove == "b7b5" && GetPiece("b5") == "p")
                {
                    SetPiece("c5", " "); SetPiece("b5", " "); SetPiece("b6", "P"); return true;
                }
                if (from == "d5" && to == "c6" && GetPiece(from) == "P" && LastMove == "c7c5" && GetPiece("c5") == "p")
                {
                    SetPiece("d5", " "); SetPiece("c5", " "); SetPiece("c6", "P"); return true;
                }
                if (from == "e5" && to == "d6" && GetPiece(from) == "P" && LastMove == "d7d5" && GetPiece("d5") == "p")
                {
                    SetPiece("e5", " "); SetPiece("d5", " "); SetPiece("d6", "P"); return true;
                }
                if (from == "f5" && to == "e6" && GetPiece(from) == "P" && LastMove == "e7e5" && GetPiece("e5") == "p")
                {
                    SetPiece("f5", " "); SetPiece("e5", " "); SetPiece("e6", "P"); return true;
                }
                if (from == "g5" && to == "f6" && GetPiece(from) == "P" && LastMove == "f7f5" && GetPiece("f5") == "p")
                {
                    SetPiece("g5", " "); SetPiece("f5", " "); SetPiece("f6", "P"); return true;
                }
                if (from == "h5" && to == "g6" && GetPiece(from) == "P" && LastMove == "g7g5" && GetPiece("g5") == "p")
                {
                    SetPiece("h5", " "); SetPiece("g5", " "); SetPiece("g6", "P"); return true;
                }
            }
            else
            {
                if (from == "a4" && to == "b3" && GetPiece(from) == "p" && LastMove == "b2b4" && GetPiece("b4") == "P")
                {
                    SetPiece("a4", " "); SetPiece("b4", " "); SetPiece("b3", "p"); return true;
                }
                if (from == "b4" && to == "c3" && GetPiece(from) == "p" && LastMove == "c2c4" && GetPiece("c4") == "P")
                {
                    SetPiece("b4", " "); SetPiece("c4", " "); SetPiece("c3", "p"); return true;
                }
                if (from == "c4" && to == "d3" && GetPiece(from) == "p" && LastMove == "d2d4" && GetPiece("d4") == "P")
                {
                    SetPiece("c4", " "); SetPiece("d4", " "); SetPiece("d3", "p"); return true;
                }
                if (from == "d4" && to == "e3" && GetPiece(from) == "p" && LastMove == "e2e4" && GetPiece("e4") == "P")
                {
                    SetPiece("d4", " "); SetPiece("e4", " "); SetPiece("e3", "p"); return true;
                }
                if (from == "e4" && to == "f3" && GetPiece(from) == "p" && LastMove == "f2f4" && GetPiece("f4") == "P")
                {
                    SetPiece("e4", " "); SetPiece("f4", " "); SetPiece("f3", "p"); return true;
                }
                if (from == "f4" && to == "g3" && GetPiece(from) == "p" && LastMove == "g2g4" && GetPiece("g4") == "P")
                {
                    SetPiece("f4", " "); SetPiece("g4", " "); SetPiece("g3", "p"); return true;
                }
                if (from == "g4" && to == "h3" && GetPiece(from) == "p" && LastMove == "h2h4" && GetPiece("h4") == "P")
                {
                    SetPiece("g4", " "); SetPiece("h4", " "); SetPiece("h3", "p"); return true;
                }

                if (from == "b4" && to == "a3" && GetPiece(from) == "p" && LastMove == "a2a4" && GetPiece("a4") == "P")
                {
                    SetPiece("b4", " "); SetPiece("a4", " "); SetPiece("a3", "p"); return true;
                }
                if (from == "c4" && to == "b3" && GetPiece(from) == "p" && LastMove == "b2b4" && GetPiece("b4") == "P")
                {
                    SetPiece("c4", " "); SetPiece("b4", " "); SetPiece("b3", "p"); return true;
                }
                if (from == "d4" && to == "c3" && GetPiece(from) == "p" && LastMove == "c2c4" && GetPiece("c4") == "P")
                {
                    SetPiece("d4", " "); SetPiece("c4", " "); SetPiece("c3", "p"); return true;
                }
                if (from == "e4" && to == "d3" && GetPiece(from) == "p" && LastMove == "d2d4" && GetPiece("d4") == "P")
                {
                    SetPiece("e4", " "); SetPiece("d4", " "); SetPiece("d3", "p"); return true;
                }
                if (from == "f4" && to == "e3" && GetPiece(from) == "p" && LastMove == "e2e4" && GetPiece("e4") == "P")
                {
                    SetPiece("f4", " "); SetPiece("e4", " "); SetPiece("e3", "p"); return true;
                }
                if (from == "g4" && to == "f3" && GetPiece(from) == "p" && LastMove == "f2f4" && GetPiece("f4") == "P")
                {
                    SetPiece("g4", " "); SetPiece("f4", " "); SetPiece("f3", "p"); return true;
                }
                if (from == "h4" && to == "g3" && GetPiece(from) == "p" && LastMove == "g2g4" && GetPiece("g4") == "P")
                {
                    SetPiece("h4", " "); SetPiece("g4", " "); SetPiece("g3", "p"); return true;
                }
            }

            return false;
        }
        public static Boolean DoCastling(Tuple<int, int> from, Tuple<int, int> to)
        {
            var king = GetPiece(from);
            if (Math.Abs(from.Item2 - to.Item2) == 2 && ((WhiteToMove && king == "K") || (!WhiteToMove && king == "k")))
            {
                SetPiece(to, king);
                SetPiece(from, " ");

                if(to.Item2 > from.Item2)
                {
                    var rookAt = Tuple.Create<int, int>(from.Item1, 7);
                    var rook = GetPiece(rookAt);
                    SetPiece(rookAt, " ");

                    var rookNewAt = Tuple.Create<int, int>(from.Item1, (from.Item2 + to.Item2) / 2);
                    SetPiece(rookNewAt, rook);

                    if(Math.Abs(from.Item2 - 7) == 3)
                    {
                        if (WhiteToMove)
                            CWK = false;
                        else
                            CBK = false;
                    }
                    else if (Math.Abs(from.Item2 - 7) == 4)
                    {
                        if (WhiteToMove)
                            CWQ = false;
                        else
                            CBQ = false;
                    }
                }
                else
                {
                    var rookAt = Tuple.Create<int, int>(from.Item1, 0);
                    var rook = GetPiece(rookAt);
                    SetPiece(rookAt, " ");

                    var rookNewAt = Tuple.Create<int, int>(from.Item1, (from.Item2 + to.Item2) / 2);
                    SetPiece(rookNewAt, rook);

                    if (Math.Abs(from.Item2 - 0) == 3)
                    {
                        if (WhiteToMove)
                            CWK = false;
                        else
                            CBK = false;
                    }
                    else if (Math.Abs(from.Item2 - 0) == 4)
                    {
                        if (WhiteToMove)
                            CWQ = false;
                        else
                            CBQ = false;
                    }
                }

                return true;
            }

            return false;
        }

        public static void SetPiece(Tuple<int, int> at, string piece)
        {
            ChessBoard[at.Item1, at.Item2] = piece;
        }

        public static void SetPiece(string pos, string piece)
        {
            SetPiece(ToCoordinates(pos), piece);
        }

        public static string GetPiece(Tuple<int, int> at)
        {
            return ChessBoard[at.Item1, at.Item2];
        }

        public static string GetPiece(string pos)
        {
            return GetPiece(ToCoordinates(pos));
        }

        public static string GetPiece(int row, int col)
        {
            return ChessBoard[row, col];
        }

        public static Boolean MakeMove(String move)
        {
            var from = ToCoordinates(move.Substring(0, 2));
            var to = ToCoordinates(move.Substring(2, 2));
            var piece = GetPiece(from);

            if(move.Length == 4)
            {
                if(!CapturePawnPassant(from, to) && !DoCastling(from, to))
                {
                    SetPiece(to, GetPiece(from));
                    SetPiece(from, " ");
                }

                if (piece == "K")
                {
                    CWK = false;
                    CWQ = false;
                }

                if (piece == "k")
                {
                    CBK = false;
                    CBQ = false;
                }
            }
            if(move.Length == 5)
            {
                var toPiece = (WhiteToMove) ? move.Substring(4, 1).ToUpper() : move.Substring(4, 1).ToLower();
                SetPiece(to, toPiece);
                SetPiece(from, " ");
            }

            LastMove = move;
            
            WhiteToMove = !WhiteToMove;
            return true;
        }

        public static void ImportFEN(String input)
        {
            input = input.Trim();

            ChessBoard = new String[8, 8]{
                {" "," "," "," "," "," "," "," "},
                {" "," "," "," "," "," "," "," "},
                {" "," "," "," "," "," "," "," "},
                {" "," "," "," "," "," "," "," "},
                {" "," "," "," "," "," "," "," "},
                {" "," "," "," "," "," "," "," "},
                {" "," "," "," "," "," "," "," "},
                {" "," "," "," "," "," "," "," "}};

            int index = 0;
            int row = 0;
            int col = 0;
            Boolean space = false;

            foreach(var c in input)
            {
                switch(c)
                {
                    case 'r':
                    case 'R':
                    case 'n':
                    case 'N':
                    case 'b':
                    case 'B':
                    case 'q':
                    case 'Q':
                    case 'k':
                    case 'K':
                    case 'p':
                    case 'P':
                    {
                        if (!space)
                        {
                            row = index / 8;
                            col = index % 8;
                            ChessBoard[row, col] = "" + c;
                            index++;
                        }
                        else 
                        {
                            switch (c)
                            {
                                case '-':
                                    break;
                                case 'K':
                                    CWK = true;
                                    break;
                                case 'Q':
                                    CWQ = true;
                                    break;
                                case 'k':
                                    CBK = true;
                                    break;
                                case 'q':
                                    CBQ = true;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    break;

                    case '1': index += 1; break;
                    case '2': index += 2; break;
                    case '3': index += 3; break;
                    case '4': index += 4; break;
                    case '5': index += 5; break;
                    case '6': index += 6; break;
                    case '7': index += 7; break;
                    case '8': index += 8; break;

                    case '/': break;

                    case ' ': space = true; break;

                    case 'w': WhiteToMove = true; break;

                    default: break;
                }
                
            }

            CB = !(CBQ || CBK);
            CW = !(CWQ || CWK);
        }
    }
}
