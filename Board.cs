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
            return possibilities.Trim();
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
            
            if(move.Length == 4)
            {
                if(!DoCastling(from, to))
                {
                    SetPiece(to, GetPiece(from));
                    SetPiece(from, " ");
                }
            }
            if(move.Length == 5)
            {
                SetPiece(to, move.Substring(4, 1));
                SetPiece(from, " ");
            }

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
        }
    }
}
