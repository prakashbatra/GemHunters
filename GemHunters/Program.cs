using System;

namespace GemHunters
{
    // Position class
    class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    // Player class
    class Player
    {
        public string Name { get; set; }
        public Position Position { get; set; }
        public int GemCount { get; set; }

        public Player(string name, Position position)
        {
            Name = name;
            Position = position;
            GemCount = 0;
        }

        public void Move(char direction)
        {
            switch (direction)
            {
                case 'U': Position.X--; break;
                case 'D': Position.X++; break;
                case 'L': Position.Y--; break;
                case 'R': Position.Y++; break;
            }
        }
    }

    // Cell class
    class Cell
    {
        public string Occupant { get; set; }

        public Cell(string occupant = "-")
        {
            Occupant = occupant;
        }
    }

    // Board class
    class Board
    {
        public Cell[,] Grid { get; set; }
        private static Random random = new Random();
        private Game Game;

        public Board(Game game)
        {
            this.Game = game;
            Grid = new Cell[6, 6];
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    Grid[i, j] = new Cell();
                }
            }

            PlacePlayer(new Player("P1", new Position(0, 0)));
            PlacePlayer(new Player("P2", new Position(5, 5)));

            PlaceObstacles();
            PlaceGems();
        }

        private void PlacePlayer(Player player)
        {
            Grid[player.Position.X, player.Position.Y].Occupant = player.Name;
        }

        private void PlaceObstacles()
        {
            for (int i = 0; i < 5; i++)
            {
                int x, y;
                do
                {
                    x = random.Next(0, 6);
                    y = random.Next(0, 6);
                } while (Grid[x, y].Occupant != "-");
                Grid[x, y].Occupant = "O";
            }
        }

        private void PlaceGems()
        {
            for (int i = 0; i < 8; i++)
            {
                int x, y;
                do
                {
                    x = random.Next(0, 6);
                    y = random.Next(0, 6);
                } while (Grid[x, y].Occupant != "-");
                Grid[x, y].Occupant = "G";
            }
        }

        public void Display()
        {
            Console.Clear();
            Console.WriteLine(string.Format("Total turns: {0}", Game.TotalTurns));
            Console.WriteLine("---------------------------------------------");
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    Console.Write(Grid[i, j].Occupant + " ");
                }
                Console.WriteLine();
            }
        }

        public bool IsValidMove(Player player, char direction)
        {
            int newX = player.Position.X, newY = player.Position.Y;
            switch (direction)
            {
                case 'U': newX--; break;
                case 'D': newX++; break;
                case 'L': newY--; break;
                case 'R': newY++; break;
            }
            return newX >= 0 && newX < 6 && newY >= 0 && newY < 6 && Grid[newX, newY].Occupant != "O";
        }

        public void MovePlayer(Player player, char direction)
        {
            Grid[player.Position.X, player.Position.Y].Occupant = "-";
            player.Move(direction);
            CollectGem(player);
            Grid[player.Position.X, player.Position.Y].Occupant = player.Name;
        }

        private void CollectGem(Player player)
        {
            if (Grid[player.Position.X, player.Position.Y].Occupant == "G")
            {
                player.GemCount++;
            }
        }
    }

    // Game class
    class Game
    {
        private Board Board { get; set; }
        private Player Player1 { get; set; }
        private Player Player2 { get; set; }
        private Player CurrentTurn { get; set; }
        public int TotalTurns { get; set; }

        public Game()
        {
            Board = new Board(this);
            Player1 = new Player("P1", new Position(0, 0));
            Player2 = new Player("P2", new Position(5, 5));
            CurrentTurn = Player1;
            TotalTurns = 0;
        }

        public void Start()
        {
            while (!IsGameOver())
            {
                Board.Display();
                Console.WriteLine($"{CurrentTurn.Name}'s turn. Gems collected: {CurrentTurn.GemCount}");
                Console.Write("Enter move (U/D/L/R): ");
                char move = Console.ReadKey().KeyChar;
                Console.WriteLine();

                if (Board.IsValidMove(CurrentTurn, move))
                {
                    Board.MovePlayer(CurrentTurn, move);
                    TotalTurns++;
                    SwitchTurn();
                }
                else
                {
                    Console.WriteLine("Invalid move. Try again.");
                }
            }
            AnnounceWinner();
        }

        private void SwitchTurn()
        {
            CurrentTurn = (CurrentTurn == Player1) ? Player2 : Player1;
        }

        private bool IsGameOver()
        {
            return TotalTurns >= 10;
        }

        private void AnnounceWinner()
        {
            Console.WriteLine("Game Over!");
            Console.WriteLine($"P1 Gems: {Player1.GemCount}");
            Console.WriteLine($"P2 Gems: {Player2.GemCount}");
            if (Player1.GemCount > Player2.GemCount)
            {
                Console.WriteLine("Player 1 wins!");
            }
            else if (Player2.GemCount > Player1.GemCount)
            {
                Console.WriteLine("Player 2 wins!");
            }
            else
            {
                Console.WriteLine("It's a tie!");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            bool playAgain = false;
            do
            {
                Game game = new Game();
                game.Start();

                Console.WriteLine("Do you want to play again? [y/n] (y)");
                string input = Console.ReadLine() ?? "y";
                playAgain = (input == "y");
            } while (playAgain);

        }
    }
}
