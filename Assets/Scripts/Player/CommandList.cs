using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Assets.Scripts.Enums;

class CommandList : MonoBehaviour
    {
        int CommandIndex;
        public ArrayList List;

        private System.Random _random = new System.Random();
        private Array _valuesCommandTypes = Enum.GetValues(typeof(CommandType));

        public Player player;

        void Start()
        {
            List = new ArrayList();
            CommandIndex = -1;
            player.CommandEventHandler += Compare;
            this.Add(this.getRandomCommand());
        }

        public Command getRandomCommand()
        {
            CommandType randomRight = (CommandType)_valuesCommandTypes.GetValue(_random.Next(_valuesCommandTypes.Length));
            CommandType randomLeft = (CommandType)_valuesCommandTypes.GetValue(_random.Next(_valuesCommandTypes.Length));
            return new Command(randomLeft, randomRight);
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
