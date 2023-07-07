using Snake;

bool move = true;

int dimention = 10;

Directions direction = Directions.down;

SnakeObject snake = new SnakeObject();

FieldCLass field = new FieldCLass(dimention);

Fruit apple = new Fruit();

apple.Apple.Character = 'a';

Console.WriteLine("Press 'Enter' to start game: ");

ConsoleKeyInfo start = Console.ReadKey();


if (start.Key == ConsoleKey.Enter)
{
    Console.WriteLine("Choose direction");

    FillFieldWithValues(field.Field, apple);

    ConsoleKeyInfo enteredDirection;

    do
    {
        // get key pressed by user
        enteredDirection = Console.ReadKey();
        // pause for 1 second
        Thread.Sleep(1000);
        Console.Clear();

        SetDirection(enteredDirection);

        PerformDirectionLogic(direction, snake.SnakeBody, field.Field);

        PrintFieldWithSnake(field.Field);


    } while (move);
    if (!move)
    {
        Console.WriteLine("Game over!");
    }
}


void FillFieldWithValues(char[,] array, Fruit apple)
{
    int appleX = apple.Apple.X;
    int appleY = apple.Apple.Y;
    for (int i = 0; i < array.GetLength(0); i++)
    {
        for (int j = 0; j < array.GetLength(1); j++)
        {
            if (appleX == i && appleY == j)
            {
                array[i, j] = 'a';
            }
            else
                array[i, j] = '0';
        }
    }

}
void PlaceSnakeOnField(List<Cell> snake, char[,] field, int xHead, int yHead)
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
        // check for game over
        if (yHead == yHead + 1 || yHead == yHead - 1)
        {
            move = false; break;
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
            Console.Write(field[i, j]);
        }
        Console.WriteLine();
    }
}
void SetDirection(ConsoleKeyInfo enteredDirection)
{
    switch (enteredDirection.Key)
    {
        case ConsoleKey.W:
            direction = Directions.up;
            break;
        case ConsoleKey.S:
            direction = Directions.down;
            break;
        case ConsoleKey.D:
            direction = Directions.right;
            break;
        case ConsoleKey.A:
            direction = Directions.left;
            break;
        default:
            break;
    }
}
bool CheckIfSnakeEatsApple(char[,] field, int x, int y)
{
    if (x >= 0
        && x < field.GetLength(0)
        && y >= 0
        && y < field.GetLength(1))
    {
        if (field[x, y] == 'a')
        {
            return true;

        }
    }
    return false;
}
void PerformDirectionLogic(Directions direction, List<Cell> snake, char[,] field)
{
    // get head
    Cell lastCell = snake.Last();

    // get tail
    Cell firstCell = snake.First();

    bool appleIsEaten = false;
    // depending on direction, move head
    switch (direction)
    {
        case Directions.up:
            // check if snake eats apple
            if (CheckIfSnakeEatsApple(field, lastCell.X - 1, lastCell.Y))
            {
                // add additional head
                appleIsEaten = true;
            }
            snake.Add(new Cell(lastCell.X - 1, lastCell.Y));
            PlaceSnakeOnField(snake, field, lastCell.X - 1, lastCell.Y);
            break;

        case Directions.down:
            // check if snake eats apple
            if (CheckIfSnakeEatsApple(field, lastCell.X + 1, lastCell.Y))
            {
                // add additional head
                appleIsEaten = true;
            }
            snake.Add(new Cell(lastCell.X + 1, lastCell.Y));
            PlaceSnakeOnField(snake, field, lastCell.X + 1, lastCell.Y);
            break;

        case Directions.left:
            // check if snake eats apple
            if (CheckIfSnakeEatsApple(field, lastCell.X, lastCell.Y - 1))
            {
                // add additional head
                appleIsEaten = true;
            }
            snake.Add(new Cell(lastCell.X, lastCell.Y - 1));
            PlaceSnakeOnField(snake, field, lastCell.X, lastCell.Y - 1);
            break;

        case Directions.right:
            // check if snake eats apple
            if (CheckIfSnakeEatsApple(field, lastCell.X, lastCell.Y + 1))
            {
                // add additional head
                appleIsEaten = true;
            }
            snake.Add(new Cell(lastCell.X, lastCell.Y + 1));
            PlaceSnakeOnField(snake, field, lastCell.X, lastCell.Y + 1);
            break;

    }
    if (!appleIsEaten)
    {
        // remove tail from field
        field[firstCell.X, firstCell.Y] = '0';
        snake.RemoveAt(0);
    }

}

//bool CheckIfSnakeRunIntoItself(Directions directon, List<Cell> snake, char[,] field)
//{
//    var last = snake.Last();
//    int xToCheck = last.X;
//    int yToCheck = last.Y;
//    bool crash = false;
//    switch (direction)
//    {
//        case Directions.left:
//            if (field[xToCheck, yToCheck - 1] == 's')
//            {
//                crash = true; break;
//            }
//            break;
//        case Directions.right:
//            if (field[xToCheck, yToCheck + 1] == 's')
//            {
//                crash = true; break;
//            }
//            break;
//        default:
//            break;
//    }

//}




//var lastCell = snake.SnakeBody.Last();
//if (enteredDirection.Key == ConsoleKey.W)
//{
//    snake.SnakeBody.Add(new Cell(lastCell.X - 1, lastCell.Y));
//    enteredDirection = Console.ReadKey();
//    if (enteredDirection.Key != ConsoleKey.W)
//    {
//        break;
//    }
//}
//if (enteredDirection.Key == ConsoleKey.S)
//{
//    snake.SnakeBody.Add(new Cell(lastCell.X + 1, lastCell.Y));
//    enteredDirection = Console.ReadKey();
//    if (enteredDirection.Key != ConsoleKey.S)
//    {
//        break;
//    }
//}
//if (enteredDirection.Key == ConsoleKey.A)
//{
//    snake.SnakeBody.Add(new Cell(lastCell.X, lastCell.Y - 1));
//    enteredDirection = Console.ReadKey();
//    if (enteredDirection.Key != ConsoleKey.A)
//    {
//        break;

//    }
//}
//if (enteredDirection.Key == ConsoleKey.D)
//{
//    snake.SnakeBody.Add(new Cell(lastCell.X, lastCell.Y + 1));
//    enteredDirection = Console.ReadKey();
//    if (enteredDirection.Key != ConsoleKey.D)
//    {
//        break;
//    }
//}