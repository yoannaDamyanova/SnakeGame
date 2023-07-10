string dir = "down";
string enter=Console.ReadLine();
if (AreDirectionsOpposite(dir, enter))
{
    if (dir == "up" && enter == "down")
    {
        dir = dir;
    }
}
Console.WriteLine(dir);

static bool AreDirectionsOpposite(string direction1, string direction2)
{
    if (direction1 == "up" && direction2 == "down")
        return true;
    else if (direction1 == "down" && direction2 == "up")
        return true;
    else if (direction1 == "left" && direction2 == "right")
        return true;
    else if (direction1 == "right" && direction2 == "left")
        return true;
    else
        return false;
}