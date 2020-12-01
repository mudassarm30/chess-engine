using System;

namespace Originsoft
{
    class UCI
    {
        private static string ENGINENAME = "Originsoft";
        private static string AUTHOR = "Mudassar";
        private Board board = null;
        public UCI()
        {
            board = new Board();
        }

        static void Main(string[] args)
        {
            UCI engine = new UCI();

            while (true)
            {
                String inputString = Console.ReadLine();
                if ("uci".Equals(inputString))
                {
                    engine.InputUCI();
                }
                else if (inputString.StartsWith("setoption"))
                {
                    engine.InputSetOption(inputString);
                }
                else if ("isready".Equals(inputString))
                {
                    engine.InputIsReady();
                }
                else if ("ucinewgame".Equals(inputString))
                {
                    engine.InputUCINewGame();
                }
                else if (inputString.StartsWith("position"))
                {
                    engine.InputPosition(inputString);
                    engine.board.Print();
                }
                else if (inputString.StartsWith("go"))
                {
                    engine.InputGo();
                }
                else if (inputString.Equals("quit"))
                {
                    engine.InputQuit();
                }
                else if ("print".Equals(inputString))
                {
                    engine.InputPrint();
                }
            }
        }
        public void InputUCI()
        {
            Console.WriteLine("id name " + ENGINENAME);
            Console.WriteLine("id author " + AUTHOR);
            Console.WriteLine("uciok");
        }
        public void InputSetOption(String inputString)
        {
        }
        public void InputIsReady()
        {
            Console.WriteLine("readyok");
        }
        public void InputUCINewGame()
        {
        }
        public void InputPosition(String input)
        {
            String[] tokens = input.Split(" ");
            
            if(tokens[1] == "startpos")
            {
                board.ImportFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            }
            else if(tokens[1] == "fen")
            {
                input = input.Substring(tokens[0].Length + tokens[1].Length + 2);
                board.ImportFEN(input);
            }

            if (tokens.Length <= 2)
                return;

            if(tokens[2] == "moves") 
            { 
                for(var m = 3; m < tokens.Length; m++)
                {
                    var move = tokens[m];
                    board.MakeMove(move);
                }
            }
        }
        public void InputGo()
        {
            var strmoves = board.GetAllPossibleMoves();
            var moves = strmoves.Split(" ");
            var rand = new Random(DateTimeOffset.UtcNow.Millisecond);
            var move = moves[rand.Next(0, moves.Length)];
            board.MakeMove(move);
            Console.WriteLine("info possible moves: " + strmoves);
            Console.WriteLine("bestmove " + move);
            board.Print();
        }
        public void InputQuit()
        {
        }
        public void InputPrint()
        {
            board.Print();
        }
    }
}
