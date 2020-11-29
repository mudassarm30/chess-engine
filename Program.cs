using System;

namespace Originsoft
{
    class Program
    {
        private static string ENGINENAME = "Originsoft";
        private static string AUTHOR = "Mudassar";

        static void Main(string[] args)
        {

            while (true)
            {
                String inputString = Console.ReadLine();
                if ("uci".Equals(inputString))
                {
                    inputUCI();
                }
                else if (inputString.StartsWith("setoption"))
                {
                    inputSetOption(inputString);
                }
                else if ("isready".Equals(inputString))
                {
                    inputIsReady();
                }
                else if ("ucinewgame".Equals(inputString))
                {
                    inputUCINewGame();
                }
                else if (inputString.StartsWith("position"))
                {
                    inputPosition(inputString);
                    Board.Print();
                }
                else if (inputString.StartsWith("go"))
                {
                    inputGo();
                }
                else if (inputString.Equals("quit"))
                {
                    inputQuit();
                }
                else if ("print".Equals(inputString))
                {
                    inputPrint();
                }
            }
        }
        public static void inputUCI()
        {
            Console.WriteLine("id name " + ENGINENAME);
            Console.WriteLine("id author " + AUTHOR);
            Console.WriteLine("uciok");
        }
        public static void inputSetOption(String inputString)
        {
        }
        public static void inputIsReady()
        {
            Console.WriteLine("readyok");
        }
        public static void inputUCINewGame()
        {
        }
        public static void inputPosition(String input)
        {
            String[] tokens = input.Split(" ");
            
            if(tokens[1] == "startpos")
            {
                Board.ImportFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            }
            else if(tokens[1] == "fen")
            {
                input = input.Substring(tokens[0].Length + tokens[1].Length + 2);
                Board.ImportFEN(input);
            }

            if (tokens.Length <= 2)
                return;

            if(tokens[2] == "moves") 
            { 
                for(var m = 3; m < tokens.Length; m++)
                {
                    var move = tokens[m];
                    Board.MakeMove(move);
                }
            }
        }
        public static void inputGo()
        {
            var strmoves = Board.GetAllPossibleMoves();
            var moves = strmoves.Split(" ");
            var rand = new Random(DateTimeOffset.UtcNow.Millisecond);
            var move = moves[rand.Next(0, moves.Length)];
            Board.MakeMove(move);
            Console.WriteLine("info possible moves: " + strmoves);
            Console.WriteLine("bestmove " + move);
            Board.Print();
        }
        public static void inputQuit()
        {
        }
        public static void inputPrint()
        {
            Board.Print();
        }
    }
}
