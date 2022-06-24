using System.Collections;
using System.Collections.Generic;

namespace BattleLogic
{
    public class CommandExecuter
    {
        List<Command> commands = new List<Command>();

        public event OnCommandAddedEvent OnCommandAdded;
        public event OnCommandExecuteBeforeEvent OnCommandExecuteBefore;
        public event OnCommandExecuteAfterEvent OnCommandExecuteAfter;

        public void AddCommand(Command command)
        {
            commands.Add(command);
            OnCommandAdded?.Invoke(command, commands);
        }

        public IEnumerator ExecuteCommandsAsync()
        {
            for (int i = 0; i < commands.Count; ++i)
            {
                OnCommandExecuteBefore?.Invoke(commands[i]);
                commands[i].Execute();
                OnCommandExecuteAfter?.Invoke(commands[i]);
                yield return null;
            }
        }

        public void ClearCommands()
        {
            commands.Clear();
        }
    }
}