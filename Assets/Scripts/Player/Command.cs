using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Enums;


    public class Command
    {
        public CommandType Left;
        public CommandType Right;
        public bool IsSpecial;

        public Command(CommandType left = CommandType.none, CommandType right = CommandType.none, bool isSpecial = false)
        {
            Left = left;
            Right = right;
            IsSpecial = isSpecial;
        }
    }
