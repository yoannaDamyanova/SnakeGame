using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public class GameStateJson
    {
        public List<Cell> Snake { get; set; }
        public Fruit Apple { get; set; }
        public Directions Direction { get; set; }
        public int Score { get; set; }
    }
}
