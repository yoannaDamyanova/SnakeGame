using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public class FieldCLass
    {
        public char[,] Field;
        public int Dimention;
        
        public FieldCLass(int dimention)
        {
            Dimention = dimention;
            Field = new char[dimention, dimention];
        }

    }
}
