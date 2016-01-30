using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Enums;

namespace Assets.Scripts.Player
{
    class Command
    {
        public CommandType Left;
        public CommandType Right;

        public Command(CommandType left = CommandType.none, CommandType right = CommandType.none)
        {
            Left = left;
            Right = right;
        }
    }
}
