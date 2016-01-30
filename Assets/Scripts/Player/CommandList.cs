using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Assets.Scripts.Player
{
    class CommandList
    {
        int CommandIndex;
        ArrayList List;

        public CommandList()
        {
            List = new ArrayList();
            CommandIndex = -1;
        }

        public bool Compare(object sender, EventArgs a)
        {
            bool result = false;
            Command item = (Command)List[CommandIndex];
            if(command.Left == item.Left && command.Right == item.Right)
            {
                result = true;
            }
            return result;
        }

        public void Add(Command command)
        {
            CommandIndex++;
            List.Add(command);
        }

        public void Next()
        {
            CommandIndex++;
     
            if(CommandIndex >= List.Count)
            {
                CommandIndex = 0;
            }
        }
    }
}
