using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public class Fruit
    {
        public Cell Apple { get; set; }
        public char Character;

        public Fruit() 
        {
            Apple = new Cell(3, 4);
            Character= 'a';
        }
    }
}
