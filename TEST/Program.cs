bool keyHold = false;
ConsoleKeyInfo key = Console.ReadKey();

while (true)
{
    if (key.Key==ConsoleKey.W)
    {
        //while (key.Key == ConsoleKey.W)
        //{

            Console.WriteLine("up");
            key = Console.ReadKey();
            if (key.Key != ConsoleKey.W)
            {

                continue;
            }
        //}
    }

    if (key.Key == ConsoleKey.S)
    {
        //while (key.Key == ConsoleKey.S)
        //{
            Console.WriteLine("down");
            key = Console.ReadKey();
            if (key.Key != ConsoleKey.S)
            {

                continue;
            }
        //}
    }

    if (key.Key == ConsoleKey.D)
    {
        //while (key.Key == ConsoleKey.D)
        //{
            Console.WriteLine("right");
            key = Console.ReadKey();
            if (key.Key != ConsoleKey.D)
            {

                continue;
            }
        //}
    }

    if (key.Key == ConsoleKey.A)
    {
        while (key.Key == ConsoleKey.A)
        {
            Console.WriteLine("left");
            key = Console.ReadKey();
            if (key.Key != ConsoleKey.A)
            {
                key = Console.ReadKey();

                break;
            }
        }
    }
 
    //key = Console.ReadKey();
}

