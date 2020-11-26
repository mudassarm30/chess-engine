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

        public static void Print()
        {
            Console.WriteLine("");
            for(var r = 0; r < 8; r++)
            {
                Console.Write("[");
                for(var c = 0; c < 8; c++)
                {
                    Console.Write(ChessBoard[r, c]);
                    if (c < 7)
                        Console.Write(",");
                }
                Console.Write("]\n");
            }
        }

        public static Tuple<int, int> ToCoordinates(String pos)   
        {
            int col = pos.ToCharArray()[0] - 'a';
            int row = 8 - (pos.ToCharArray()[1] - '0');

            return Tuple.Create<int, int>(row, col);
        }

        public static Boolean DoCastling(Tuple<int, int> from, Tuple<int, int> to)
        {
            if(Math.Abs(from.Item2 - to.Item2) == 2)
            {
                var king = GetPiece(from);
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
