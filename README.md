**GemHunters**
A Console-based 2D game called "Gem Hunters" where players compete to collect the most gems within a set number of turns. It is a simple game where two players compete to collect gems on a 6x6 grid filled with obstacles. Each player takes turns to move around the board, and the player with the most gems collected after a fixed number of turns wins the game.

Game board Size:
A 6x6 square board.

**Game Elements:**
Players: Player 1 starts in the top-left corner and Player 2 starts in the bottom-right corner. 
P1 starts at (0, 0).
P2 starts at (5, 5).
Gems: Randomly placed on the board at the start of the game. They do not move once placed.
Obstacles: Random positions on the board (e.g., represented by an "O") that players cannot pass through.

**Rules:**
- The board contains 10 randomly placed obstacles ('O') and 20 randomly placed gems ('G').
- Players take turns to move in one of four directions: Up (U), Down (D), Left (L), Right (R).
- Players cannot move diagonally.
- Players cannot move onto or through squares with obstacles.
- Players can move to an empty cell ('-') or a cell containing a gem.
- Players cannot move into cells occupied by obstacles or outside the grid.
- The game ends after 30 total moves (15 moves per player).
- The player with the most gems collected at the end of the game wins. If both players have the same number of gems, the game is a tie.

**How to Play**
- The game starts automatically when you run the program.
- Players are prompted to enter their move (U/D/L/R) during their turn.
- After entering a move, the game will validate the move and update the board.
- The game will display the current state of the board after each move.
- Once 30 moves are made, the game will announce the winner.
- After the game ends, you have the option to play again.

**Code Structure**
- Position Class: Represents the coordinates (X, Y) of a player or cell on the board.
- Player Class: Represents a player with a name, position, and gem count. It includes methods to move the player.
- Cell Class: Represents a cell on the game board. Each cell has an occupant ('-', 'P1', 'P2', 'O', 'G').
- Board Class: Manages the game board, including the placement of players, obstacles, and gems. It also handles player movement and gem collection.
- Game Class: Controls the game flow, including turn management, move validation, and winner announcement.
- Program Class: Contains the Main method, which starts the game and handles the option to play again.
