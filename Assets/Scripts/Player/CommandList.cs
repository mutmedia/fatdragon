using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;


    class CommandList : MonoBehaviour
    {
        int CommandIndex;
        public ArrayList List;

        public Player player;

        void Start()
        {
            List = new ArrayList();
            CommandIndex = -1;
            player.CommandEventHandler += Compare;
            this.Add(player.getRandomCommand());
        }

        public void Compare(object sender, CommandEventArgs a)
        {
            bool result = false;
            Command command = (Command)a.Command;
            Command item = (Command)List[CommandIndex];
            if(command.Left == item.Left && command.Right == item.Right)
            {
                result = true;
            }

            player.ResolveCommandResult(result);
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
