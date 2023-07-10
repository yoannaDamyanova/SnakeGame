using System;

public class CoordinateGenerator
{
    private Random random;

    public CoordinateGenerator()
    {
        random = new Random();
    }

    public (int x, int y) GenerateCoordinates(int min, int max)
    {
        int x = random.Next(min, max + 1);
        int y = random.Next(min, max + 1);
        return (x, y);
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        CoordinateGenerator generator = new CoordinateGenerator();

        int min = 0;
        int max = 10;

        (int x1, int y1) = generator.GenerateCoordinates(min, max);
        (int x2, int y2) = generator.GenerateCoordinates(min, max);

        Console.WriteLine($"Coordinate 1: ({x1}, {y1})");
        Console.WriteLine($"Coordinate 2: ({x2}, {y2})");
    }
}