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
}
FillFieldWithValues(field.Field);

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

void FillFieldWithValues(char[,] array)
{
    for (int i = 0; i < array.GetLength(0); i++)
    {
        for (int j = 0; j < array.GetLength(1); j++)
        {
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
        if (yHead==yHead+1 || yHead==yHead-1) 
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
void PerformDirectionLogic(Directions direction, List<Cell> snake, char[,] field)
{
    // get head
    Cell lastCell = snake.Last();
    // depending on direction, move head
    switch (direction)
    {
        case Directions.up:
            snake.Add(new Cell(lastCell.X - 1, lastCell.Y));

            PlaceSnakeOnField(snake, field, lastCell.X - 1, lastCell.Y);
            break;
        case Directions.down:
            snake.Add(new Cell(lastCell.X + 1, lastCell.Y));
            PlaceSnakeOnField(snake, field, lastCell.X + 1, lastCell.Y);

            break;
        case Directions.left:
            snake.Add(new Cell(lastCell.X, lastCell.Y - 1));
            PlaceSnakeOnField(snake, field, lastCell.X, lastCell.Y - 1);

            break;
        case Directions.right:
            snake.Add(new Cell(lastCell.X, lastCell.Y + 1));
            PlaceSnakeOnField(snake, field, lastCell.X, lastCell.Y + 1);

            break;
    }

    // get tail
    Cell firstCell = snake.First();
    int xToRemove = firstCell.X;
    int yToRemove = firstCell.Y;
    // remove tail from field
    field[xToRemove, yToRemove] = '0';
    snake.RemoveAt(0);
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