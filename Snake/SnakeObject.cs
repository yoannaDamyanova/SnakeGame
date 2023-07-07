using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public class SnakeObject
    {
        public List<Cell> SnakeBody;

        public SnakeObject()
        {
            SnakeBody = new List<Cell>() 
            { 
                new Cell(6,4),
                new Cell(6,5),
                new Cell(6,6),
                new Cell(6,7),
                new Cell(5,7),
                new Cell(4,7),
                new Cell(3,7),
            };           
        }
    }
}
