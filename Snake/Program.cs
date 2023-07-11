using Microsoft.EntityFrameworkCore;
using Snake;
using Snake.Data;
using Snake.Data.Models;
using Snake.Migrations;
using System.Text.Json;

bool move = true;

int dimension = 10;

Directions direction = Directions.up;

SnakeObject snake = new SnakeObject();

FieldCLass field = new FieldCLass(dimension);

Fruit apple = new Fruit();


apple.Apple.Character = 'a';

Console.WriteLine("Press 'Enter' to start game: ");

ConsoleKeyInfo start = Console.ReadKey();

int score = 0;
if (start.Key == ConsoleKey.Enter)
{
    FillFieldWithValues(field.Field, apple);
    foreach (var item in snake.SnakeBody)
    {
        field.Field[item.X, item.Y] = 's';
    }
    for (int i = 0; i <= field.Field.GetLength(0) - 1; i++)
    {
        for (int j = 0; j <= field.Field.GetLength(1) - 1; j++)
        {
            if (field.Field[i, j] == '░')
            {
                //green
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;

            }
            if (field.Field[i, j] == 's')
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            }
            if (field.Field[i, j] == 'a')
            {
                //red

                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.DarkRed;
            }
            Console.Write(field.Field[i, j]);
        }
        Console.WriteLine();
    }
    Console.WriteLine("Choose direction");
    ConsoleKeyInfo enteredDirection = Console.ReadKey();

    do
    {
        FillFieldWithValues(field.Field, apple);
        while (Console.KeyAvailable)
        {
            enteredDirection = Console.ReadKey();
        }
        // get key pressed by user
        if (enteredDirection.Key == ConsoleKey.P)
        {
            while (!Console.KeyAvailable)
            {
                Thread.Sleep(1000);
            }
            GameStateJson gameState = new GameStateJson
            {
                Snake = snake.SnakeBody,
                Apple=apple,
                Direction=direction,
                Score=score
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
            if (enteredDirection.Key == ConsoleKey.P)
            {
                continue;
            }
        }
        Directions dir = SetDirection(enteredDirection);

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
        string name = Console.ReadLine();
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
    if (crash)
    {
        move = false;
    }
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
Directions SetDirection(ConsoleKeyInfo enteredDirection)
{
    switch (enteredDirection.Key)
    {
        case ConsoleKey.UpArrow:
            return Directions.up;
        case ConsoleKey.DownArrow:
            return Directions.down;
        case ConsoleKey.RightArrow:
            return Directions.right;
        case ConsoleKey.LeftArrow:
            return Directions.left;
        default: return Directions.down;
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

    bool crash = false;

    PlaceSnakeOnField(snake, field, crash);

    // get head
    Cell lastCell = snake.Last();

    // get tail
    Cell firstCell = snake.First();

    // check for apple
    bool appleIsEaten = false;
    // depending on direction, move head
    switch (direction)
    {
        case Directions.up:

            if (CheckIfSnakeRunsIntoItself(field, lastCell.X - 1, lastCell.Y))
            {
                crash = true;
            }

            else
            {
                // check if snake eats apple
                if (CheckIfSnakeEatsApple(field, lastCell.X - 1, lastCell.Y))
                {
                    // add additional head
                    appleIsEaten = true;
                }
                snake.Add(new Cell(lastCell.X - 1, lastCell.Y));
                PlaceSnakeOnField(snake, field, crash);
            }
            break;

        case Directions.down:
            if (CheckIfSnakeRunsIntoItself(field, lastCell.X + 1, lastCell.Y))
            {
                crash = true;
            }
            if (CheckIfSnakeEatsApple(field, lastCell.X + 1, lastCell.Y))
            {
                // add additional head
                appleIsEaten = true;
            }
            snake.Add(new Cell(lastCell.X + 1, lastCell.Y));
            PlaceSnakeOnField(snake, field, crash);
            break;

        case Directions.left:
            if (CheckIfSnakeRunsIntoItself(field, lastCell.X, lastCell.Y - 1))
            {
                crash = true;
            }
            // check if snake eats apple
            if (CheckIfSnakeEatsApple(field, lastCell.X, lastCell.Y - 1))
            {
                // add additional head
                appleIsEaten = true;
            }
            snake.Add(new Cell(lastCell.X, lastCell.Y - 1));
            PlaceSnakeOnField(snake, field, crash);
            break;

        case Directions.right:
            if (CheckIfSnakeRunsIntoItself(field, lastCell.X, lastCell.Y + 1))
            {
                crash = true;
            }
            // check if snake eats apple
            if (CheckIfSnakeEatsApple(field, lastCell.X, lastCell.Y + 1))
            {
                // add additional head
                appleIsEaten = true;
            }
            snake.Add(new Cell(lastCell.X, lastCell.Y + 1));
            PlaceSnakeOnField(snake, field, crash);
            break;

    }
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

