using Microsoft.Azure.Amqp.Framing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Snake;
using Snake.Data;
using Snake.Data.Models;
using Snake.Migrations;
using System;
using System.Data;
using System.Text.Json;
using System.Threading.Channels;
using static System.Formats.Asn1.AsnWriter;
using System.Xml.Linq;


Console.WriteLine("Press 'Enter' to start new game: ");
Console.WriteLine("Press 'L' to load game");

bool move;
int dimension;
Directions direction;
SnakeObject snake;
FieldCLass field;
Fruit apple;
List<SaveGame> savedGamesList;
bool isPaused;
bool isEnded;
bool crash;
int score;
string name;

ConsoleKeyInfo? enteredDirection;

while (true)
{
    ConsoleKeyInfo start = Console.ReadKey();
    if (start.Key == ConsoleKey.Enter)
    {
        StartNewGame();
        break;
    }
    else if (start.Key == ConsoleKey.L)
    {
        LoadGame();
        break;
    }
    else
    {
        Console.WriteLine("Invalid! Try again");
    }
}
FillFieldWithValues(field.Field, apple);
PlaceSnakeOnField(snake.SnakeBody, field.Field, !move);
PrintFieldWithSnake(field.Field);
do
{
    FillFieldWithValues(field.Field, apple);
    direction = SetDirection(enteredDirection) ?? direction;
    while (Console.KeyAvailable)
    {
        enteredDirection = Console.ReadKey();
        isPaused = enteredDirection.Value.Key == ConsoleKey.P;
        isEnded = enteredDirection.Value.Key == ConsoleKey.B;
    }
    // get key pressed by user
    if (isPaused)
    {
        while (!Console.KeyAvailable)
        {
            Thread.Sleep(1000);
        }
        GameStateJson gameState = new GameStateJson
        {
            Snake = snake.SnakeBody,
            Apple = apple,
            Direction = direction,
            Score = score,
            FieldDimension = dimension
        };
        string jsonString = JsonSerializer.Serialize(gameState);
        using (var db = new AppDbContext())
        {
            SaveGame savedGame = new SaveGame
            {
                GameState = jsonString
            };
            db.Add(savedGame);
            db.SaveChanges();
        }
        enteredDirection = Console.ReadKey();
        if (enteredDirection.Value.Key == ConsoleKey.P)
        {
            isPaused = false;
            continue;
        }
    }
    else if (isEnded)
    {
        GameStateJson gameState = new GameStateJson
        {
            Snake = snake.SnakeBody,
            Apple = apple,
            Direction = direction,
            Score = score,
            FieldDimension = dimension
        };

        string jsonString = JsonSerializer.Serialize(gameState);
        using (var db = new AppDbContext())
        {
            SaveGame savedGame = new SaveGame
            {
                GameState = jsonString
            };
            db.Add(savedGame);
            db.SaveChanges();
        }
        move = false;
        break;
    }

    Directions dir = SetDirection(enteredDirection) ?? direction;

    if (!AreDirectionsOpposite(dir, direction))
    {
        direction = dir;
    }
    // pause for 1 second
    Thread.Sleep(1000);
    Console.Clear();
    PerformDirectionLogic(direction, snake.SnakeBody, field.Field);
} while (move);
if (!move)
{
    Console.Clear();
    Console.WriteLine("Game over!");
    Console.WriteLine("Enter your name: ");
    name = Console.ReadLine();
    Console.Clear();
    Console.WriteLine("High Scores:");
    using (var db = new AppDbContext())
    {
        HighScores highscore = new HighScores();
        highscore.Name = name;
        highscore.Score = score;
        db.Add(highscore);
        db.SaveChanges();
        var scores = db.HighScores
            .GroupBy(x => x.Name)
            .Select(grp => new
            {
                Name = grp.Key,
                HighScore = grp.Max(y => y.Score)
            })
            .ToList();
        foreach (var personalScore in scores)
        {
            Console.WriteLine($"{personalScore.Name}...{personalScore.HighScore}");
        }
    }

}
bool AreDirectionsOpposite(Directions direction1, Directions direction2)
{
    if (direction1 == Directions.up && direction2 == Directions.down)
        return true;
    else if (direction1 == Directions.down && direction2 == Directions.up)
        return true;
    else if (direction1 == Directions.left && direction2 == Directions.right)
        return true;
    else if (direction1 == Directions.right && direction2 == Directions.left)
        return true;
    else
        return false;
}

void FillFieldWithValues(char[,] array, Fruit apple)
{
    int appleX = apple.Apple.X;
    int appleY = apple.Apple.Y;

    for (int i = 0; i < array.GetLength(0); i++)
    {
        for (int j = 0; j < array.GetLength(1); j++)
        {
            array[i, j] = '░';
        }
    }
    array[appleX, appleY] = 'a';

}
void PlaceSnakeOnField(List<Cell> snake, char[,] field, bool crash)
{
    foreach (var item in snake)
    {
        int currX = item.X;
        int currY = item.Y;
        // draw snake if it is inside field
        if (currX >= 0
            && currX < field.GetLength(0)
            && currY >= 0
            && currY < field.GetLength(1))
        {
            field[currX, currY] = 's';
        }
    }
    foreach (var item in snake)
    {

        int currX = item.X;
        int currY = item.Y;
        if (currX < 0
            || currX + 1 > field.GetLength(0)
            || currY < 0
            || currY + 1 > field.GetLength(1))
        {
            move = false; break;
        }
    }

}
void PrintFieldWithSnake(char[,] field)
{
    int parameters = field.GetLength(0);

    // draw field and snake
    for (int i = 0; i <= parameters - 1; i++)
    {
        for (int j = 0; j <= parameters - 1; j++)
        {
            if (field[i, j] == '░')
            {
                //green
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;

            }
            if (field[i, j] == 's')
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            }
            if (field[i, j] == 'a')
            {
                //red

                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.DarkRed;
            }
            Console.Write(field[i, j]);
        }
        Console.WriteLine();
    }
}
Directions? SetDirection(ConsoleKeyInfo? enteredDirection)
{
    if (!enteredDirection.HasValue)
    {
        return null;
    }
    switch (enteredDirection.Value.Key)
    {
        case ConsoleKey.UpArrow:
            return Directions.up;
        case ConsoleKey.DownArrow:
            return Directions.down;
        case ConsoleKey.RightArrow:
            return Directions.right;
        case ConsoleKey.LeftArrow:
            return Directions.left;
        default: return null;
    }
}

bool CheckIfSnakeEatsApple(char[,] field, int x, int y)
{
    if (x >= 0
        && x < field.GetLength(0)
        && y >= 0
        && y < field.GetLength(1))
    {
        // if head eats apple
        if (x == apple.Apple.X && y == apple.Apple.Y)
        {
            return true;
        }
    }
    return false;
}
bool CheckIfRandomizedAppleMatchesSnake(char[,] field, int x, int y)
{
    if (x >= 0
    && x < field.GetLength(0)
    && y >= 0
    && y < field.GetLength(1))
    {
        if (field[x, y] == 's')
        {
            return true;
        }
    }
    return false;
}
bool CheckIfSnakeRunsIntoItself(char[,] field, int x, int y)
{
    if (x >= 0
        && x < field.GetLength(0)
        && y >= 0
        && y < field.GetLength(1))
    {
        if (field[x, y] == 's')
        {
            return true;
        }
    }
    return false;
}
void PerformDirectionLogic(Directions direction, List<Cell> snake, char[,] field)
{
    // check for crash
    PlaceSnakeOnField(snake, field, crash);
    // get head
    Cell lastCell = snake.Last();

    // get tail
    Cell firstCell = snake.First();

    // check for apple
    bool appleIsEaten = false;
    // depending on direction, move head
    int x = 0;
    int y = 0;
    switch (direction)
    {
        case Directions.up:
            x = lastCell.X - 1;
            y = lastCell.Y;
            break;
        case Directions.down:
            x = lastCell.X + 1;
            y = lastCell.Y;
            break;
        case Directions.left:
            x = lastCell.X;
            y = lastCell.Y - 1;
            break;
        case Directions.right:
            x = lastCell.X;
            y = lastCell.Y + 1;
            break;
    }
    if (CheckIfSnakeRunsIntoItself(field, x, y))
    {
        move = false;
    }
    // check if snake eats apple
    if (CheckIfSnakeEatsApple(field, x, y))
    {
        // add additional head
        appleIsEaten = true;
    }
    snake.Add(new Cell(x, y));
    PlaceSnakeOnField(snake, field, crash);
    if (!appleIsEaten)
    {
        // remove tail from field
        field[firstCell.X, firstCell.Y] = '░';
        snake.RemoveAt(0);
    }
    else if (appleIsEaten)
    {
        score += 5;
        while (CheckIfRandomizedAppleMatchesSnake(field, apple.Apple.X, apple.Apple.Y))
        {
            apple.Apple.X = new Random().Next(dimension);
            apple.Apple.X = new Random().Next(dimension);
        }
    }
    PrintFieldWithSnake(field);
    Console.WriteLine($"Current score: {score}");
}

void StartNewGame()
{
    move = true;
    dimension = 15;
    direction = Directions.up;
    snake = new SnakeObject();
    field = new FieldCLass(dimension);
    apple = new Fruit();
    savedGamesList = new List<SaveGame>();
    apple.Apple.Character = 'a';
    isPaused = false;
    isEnded = false;
    crash = false;
    score = 0;
    name = string.Empty;

    Console.WriteLine("Choose direction");
    enteredDirection = Console.ReadKey();
}
void LoadGame()
{
    using (var db = new AppDbContext())
    {
        savedGamesList = db.SavedGames.ToList();
    }
    Console.Clear();
    foreach (var savedGame in savedGamesList)
    {
        Console.WriteLine($"{savedGame.Id}");
    }
    Console.Write("Choose game: ");
    int id = int.Parse(Console.ReadLine());
    string jsonSavedGame = string.Empty;
    jsonSavedGame = savedGamesList.FirstOrDefault(x => x.Id == id).GameState;
    var currentSavedGame = JsonSerializer.Deserialize<GameStateJson>(jsonSavedGame);

    move = true;
    dimension = currentSavedGame.FieldDimension;
    direction = currentSavedGame.Direction;
    snake = new SnakeObject();
    snake.SnakeBody = currentSavedGame.Snake;
    field = new FieldCLass(dimension);
    apple = currentSavedGame.Apple;
    isPaused = false;
    isEnded = false;
    crash = false;
    score = currentSavedGame.Score;
    name = string.Empty;
    Console.WriteLine("Press 'S' to start game");
    enteredDirection = Console.ReadKey();
}