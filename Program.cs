using System;

public class ConnectFour
{
    private const int Rows = 6;
    private const int Columns = 7;
    private char[,] board = new char[Rows, Columns];
    private string player1;
    private string player2;
    private string currentPlayer;
    private char player1Symbol;  // Symbols will be determined based on player names
    private char player2Symbol;
    private ConsoleColor player1Color = ConsoleColor.Red;
    private ConsoleColor player2Color = ConsoleColor.Blue;
    private char currentSymbol;
    private ConsoleColor currentColor;

    public ConnectFour()
    {
        ClearBoard();
    }

    private void ClearBoard()
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                board[i, j] = '.';
            }
        }
    }

    public void InitializePlayers()
    {
        Console.WriteLine("Enter Player 1's name: \n");
        player1 = Console.ReadLine();
        player1Symbol = Char.ToUpper(player1[0]);

        Console.WriteLine("\nEnter Player 2's name: \n");
        player2 = Console.ReadLine();

        // Ensure unique symbols
        player2Symbol = GetUniqueSymbol(player2, player1Symbol);

        currentPlayer = player1;
        currentSymbol = player1Symbol;
        currentColor = player1Color;
        Console.WriteLine();  // Ensures the game starts on a new line.
    }

    private char GetUniqueSymbol(string name, char otherSymbol)
    {
        char symbol = Char.ToUpper(name[0]);
        if (symbol == otherSymbol)
        {
            int i = 1;  // Start checking from the second character
            while (i < name.Length && Char.ToUpper(name[i]) == otherSymbol)
            {
                i++;
            }
            return i < name.Length ? Char.ToUpper(name[i]) : 'X';  // Default if no unique character found
        }
        return symbol;
    }

    public void PlayGame()
    {
        InitializePlayers();
        bool gameRunning = true;
        while (gameRunning)
        {
            PrintBoard();
            Console.WriteLine($"{currentPlayer}'s turn ({currentSymbol}). Enter column (1-7) or '0' to restart: \n");
            int column;
            string input = Console.ReadLine();
            if (input == "0")
            {
                Console.WriteLine("\nRestarting game...\n");
                ClearBoard();  // Clear the board
                InitializePlayers();  // Reinitialize players
                continue;
            }
            else if (int.TryParse(input, out column) && column >= 1 && column <= Columns)
            {
                if (PlaceDisc(column - 1))  // Adjust index for 0-based array
                {
                    if (CheckWin(currentSymbol))
                    {
                        PrintBoard();
                        Console.WriteLine($"{currentPlayer} ({currentSymbol}) wins!\n");
                        gameRunning = false;
                    }
                    else if (IsBoardFull())
                    {
                        PrintBoard();
                        Console.WriteLine("\nThe game is a draw!\n");
                        gameRunning = false;
                    }
                    SwitchPlayer();
                }
                else
                {
                    Console.WriteLine("\nInvalid move. Column is full. Try again.\n");
                }
            }
            else
            {
                Console.WriteLine("\nInvalid input. Please enter a number between 1 and 7.\n");
            }
        }
    }

    private void PrintBoard()
    {
        Console.WriteLine();  // Ensures the board is printed on a new line.
        // Print column numbers from 1 to 7
        Console.Write("  ");
        for (int j = 1; j <= Columns; j++)
        {
            Console.Write(j + " ");
        }
        Console.WriteLine("\n");

        // Print the board with colors
        for (int i = 0; i < Rows; i++)
        {
            Console.Write("| ");
            for (int j = 0; j < Columns; j++)
            {
                if (board[i, j] == player1Symbol)
                {
                    Console.ForegroundColor = player1Color;
                }
                else if (board[i, j] == player2Symbol)
                {
                    Console.ForegroundColor = player2Color;
                }
                else
                {
                    Console.ResetColor();
                }
                Console.Write(board[i, j] + " ");
                Console.ResetColor();
            }
            Console.WriteLine("|");
        }
        Console.WriteLine();
    }

    private bool PlaceDisc(int column)
    {
        for (int i = Rows - 1; i >= 0; i--)
        {
            if (board[i, column] == '.')
            {
                board[i, column] = currentSymbol;
                return true;
            }
        }
        return false;
    }

    private void SwitchPlayer()
    {
        if (currentPlayer == player1)
        {
            currentPlayer = player2;
            currentSymbol = player2Symbol;
            currentColor = player2Color;
        }
        else
        {
            currentPlayer = player1;
            currentSymbol = player1Symbol;
            currentColor = player1Color;
        }
        Console.WriteLine();  // Ensures the switch message starts on a new line.
    }

    private bool CheckWin(char playerSymbol)
    {
        // Horizontal checks
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns - 3; j++)
            {
                if (board[i, j] == playerSymbol && board[i, j + 1] == playerSymbol &&
                    board[i, j + 2] == playerSymbol && board[i, j + 3] == playerSymbol)
                {
                    return true;
                }
            }
        }

        // Vertical checks
        for (int i = 0; i < Rows - 3; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                if (board[i, j] == playerSymbol && board[i + 1, j] == playerSymbol &&
                    board[i + 2, j] == playerSymbol && board[i + 3, j] == playerSymbol)
                {
                    return true;
                }
            }
        }

        // Diagonal checks
        // Ascending diagonal check
        for (int i = 3; i < Rows; i++)
        {
            for (int j = 0; j < Columns - 3; j++)
            {
                if (board[i, j] == playerSymbol && board[i - 1, j + 1] == playerSymbol &&
                    board[i - 2, j + 2] == playerSymbol && board[i - 3, j + 3] == playerSymbol)
                {
                    return true;
                }
            }
        }

        // Descending diagonal check
        for (int i = 3; i < Rows; i++)
        {
            for (int j = 3; j < Columns; j++)
            {
                if (board[i, j] == playerSymbol && board[i - 1, j - 1] == playerSymbol &&
                    board[i - 2, j - 2] == playerSymbol && board[i - 3, j - 3] == playerSymbol)
                {
                    return true;
                }
            }
        }

        return false;
    }


    private bool IsBoardFull()
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                if (board[i, j] == '.')
                {
                    return false;
                }
            }
        }
        return true;
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Welcome to Connect Four.\n");
        Console.WriteLine("Press 1 to Start the game.\n");
        Console.WriteLine("Press 2 to Quit the application.\n");
        Console.WriteLine("Enter your choice :  \n");
        string input = Console.ReadLine();
        Console.WriteLine();  // Ensure that the game starts or ends on a new line after input.
        //

        switch (input)
        {
            case "1":
                ConnectFour game = new ConnectFour();
                game.PlayGame();
                break;
            case "2":
                Console.WriteLine("Thank you for playing.\n");
                return;
            default:
                Console.WriteLine("Invalid option, please restart the application and select a valid option.\n");
                break;
        }
    }
}
