using System.Collections;
using System.Collections.Generic;

namespace BattleLogic
{
    public class BattleDirector
    {
        List<Status> statuses = new List<Status>();
        CommandExecuter commandExecuter = new CommandExecuter();

        public event OnCommandAddedEvent OnCommandAdded;
        public event OnCommandExecuteBeforeEvent OnCommandExecuteBefore;
        public event OnCommandExecuteAfterEvent OnCommandExecuteAfter;

        public event OnBattleStartEvent OnBattleStart;
        public event OnBattleEndEvent OnBattleEnd;
        public event OnBattleTurnStartEvent OnBattleTurnStart;
        public event OnBattleTurnEndEvent OnBattleTurnEnd;


        public BattleDirector()
        {
            commandExecuter.OnCommandAdded += (command, commands) => OnCommandAdded?.Invoke(command, commands);
            commandExecuter.OnCommandExecuteBefore += (command) => OnCommandExecuteBefore?.Invoke(command);
            commandExecuter.OnCommandExecuteAfter += (command) => OnCommandExecuteAfter?.Invoke(command); ;
        }

        public void AddStatus(Status status)
        {
            statuses.Add(status);
        }

        public void AddCommand(Command command)
        {
            commandExecuter.AddCommand(command);
        }

        public void BattleStart()
        {
            statuses.ForEach(o => o.OnBattleStart());
            OnBattleStart?.Invoke();
        }

        public IEnumerator ExecuteCommandsAsync()
        {
            OnBattleTurnStart?.Invoke();
            statuses.ForEach(o => o.OnTurnStart());
            var enumerator = commandExecuter.ExecuteCommandsAsync();
            while (enumerator.MoveNext())
            {
                yield return null;
            }
            commandExecuter.ClearCommands();
            statuses.ForEach(o => o.OnTurnEnd());
            OnBattleEnd?.Invoke();
        }

        public void BattleEnd()
        {
            statuses.ForEach(o => o.OnBattleEnd());
            OnBattleEnd?.Invoke();
        }
    }
}