using System;

namespace GemHunters
{
    // Position class
    class Position
    {
        //Properties of the Position class
        public int X { get; set; }
        public int Y { get; set; }

        //Constructor
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    // Player class
    class Player
    {
        //Properties of the Player class
        public string Name { get; set; }
        public Position Position { get; set; }
        public int GemCount { get; set; }

        //Constructor 
        public Player(string name, Position position)
        {
            Name = name;
            Position = position;
            GemCount = 0;
        }

        //Move the player as per input
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
        //Property to store the occupant
        public string Occupant { get; set; }

        //Constructor
        public Cell(string occupant = "-")
        {
            Occupant = occupant;
        }
    }

    // Board class
    class Board
    {
        //Property to store game board grid
        public Cell[,] Grid { get; set; }

        //Random object for the placements of gems and obstacles
        private static Random random = new Random();

        //Game object
        private Game Game;

        //Constructor - intialize the game board, place players, obstacles, and gems
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

            //Placement of players - Player 1 starts in the top-left corner and Player 2 starts in the bottom-right corner
            PlacePlayer(new Player("P1", new Position(0, 0)));
            PlacePlayer(new Player("P2", new Position(5, 5)));

            //Call methods to place obstacles and gems
            PlaceObstacles();
            PlaceGems();
        }

        //Place a player on the board at initial position
        private void PlacePlayer(Player player)
        {
            Grid[player.Position.X, player.Position.Y].Occupant = player.Name;
        }

        //Place 10 obstacles randomly on the game board
        private void PlaceObstacles()
        {
            for (int i = 0; i < 6; i++)
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

        //Place 20 gems randomly on the game board
        private void PlaceGems()
        {
            for (int i = 0; i < 24; i++)
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

        //Clear the console after every player move and display current board
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

        //Check if the move is valid in the bounds of grid and target cell is not obstacle
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

        //Move the player in specific direction
        public void MovePlayer(Player player, char direction)
        {
            Grid[player.Position.X, player.Position.Y].Occupant = "-";
            player.Move(direction);
            CollectGem(player);
            Grid[player.Position.X, player.Position.Y].Occupant = player.Name;
        }

        //Check if the player moved to a gem and collect it
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
        //Property to store the game board
        private Board Board { get; set; }

        //Properties to store 2 players
        private Player Player1 { get; set; }
        private Player Player2 { get; set; }

        //Property to track the player turn
        private Player CurrentTurn { get; set; }

        //Property to count the total turns in the game
        public int TotalTurns { get; set; }

        //Constructor - Initialize the board, players and set the initial turn.
        public Game()
        {
            Board = new Board(this);
            Player1 = new Player("P1", new Position(0, 0));
            Player2 = new Player("P2", new Position(5, 5));
            CurrentTurn = Player1;
            TotalTurns = 0;
        }

        //Start the game and control the game loop
        public void Start()
        {
            while (!IsGameOver())
            {
                //Display the current board
                Board.Display();

                //Print current player and their gems count
                Console.WriteLine($"{CurrentTurn.Name}'s turn. Gems collected: {CurrentTurn.GemCount}");

                //Prompt to move
                Console.Write("Enter move (U/D/L/R): ");

                //Read user input
                char move = Console.ReadKey().KeyChar;
                Console.WriteLine();

                //Check the move is valid, move the current player 
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
            //After the game is over, announce the winner
            AnnounceWinner();
        }

        //Switch the turn between players
        private void SwitchTurn()
        {
            CurrentTurn = (CurrentTurn == Player1) ? Player2 : Player1;
        }

        //The game ends after 30 moves (15 turns for each player)
        private bool IsGameOver()
        {
            return TotalTurns >= 30;
        }

        //Display winner at the end with Gems count for both the players
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
        //Main method of the program
        static void Main(string[] args)
        {
            bool playAgain = false;
            do
            {
                //Create new instance of Game class
                Game game = new Game();

                //Start the game
                game.Start();

                //Check with user if want to play again
                Console.WriteLine("Do you want to play again? [Y/N] (Y)");

                //Read the user input
                string input = Console.ReadLine() ?? "Y";
                playAgain = (input == "Y");

                //Continue the loop if play again is true
            } while (playAgain);

        }
    }
}
