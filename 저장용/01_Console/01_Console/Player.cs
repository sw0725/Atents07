using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Console
{
    internal class Player : Character
    {
        public Player (string _name) : base (_name) { }
        public override void Skill()
        {
            base.Skill();
            Console.WriteLine("플레이어의 파이어볼");
        }
    }
}
