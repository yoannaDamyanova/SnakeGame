using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public char Character;

        public Cell(int x, int y) 
        { 
            X = x;
            Y = y;
            Character = 'a';
        }
    }
}
